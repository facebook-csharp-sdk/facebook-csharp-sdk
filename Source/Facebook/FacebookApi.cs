namespace Facebook
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
#if FLUENTHTTP_CORE_TPL
    using System.Threading;
    using System.Threading.Tasks;
#endif
    using FluentHttp;

    /// <summary>
    /// Provides access to the Facbook Platform.
    /// </summary>
    public class FacebookApi
    {
        private const int BufferSize = 1024 * 4; // 4kb
        private const string InvalidSignedRequest = "Invalid signed_request";
        private const string MediaObjectMustHavePropertiesSetError = "The media object must have a content type, file name, and value set.";
        private const string MediaObjectValueIsNull = "The value of media object is null.";
        private const string MultiPartFormPrefix = "--";
        private const string MultiPartNewLine = "\r\n";

        private static readonly string[] LegacyRestApiReadOnlyCalls = new[] {
            "admin.getallocation",
            "admin.getappproperties",
            "admin.getbannedusers",
            "admin.getlivestreamvialink",
            "admin.getmetrics",
            "admin.getrestrictioninfo",
            "application.getpublicinfo",
            "auth.getapppublickey",
            "auth.getsession",
            "auth.getsignedpublicsessiondata",
            "comments.get",
            "connect.getunconnectedfriendscount",
            "dashboard.getactivity",
            "dashboard.getcount",
            "dashboard.getglobalnews",
            "dashboard.getnews",
            "dashboard.multigetcount",
            "dashboard.multigetnews",
            "data.getcookies",
            "events.get",
            "events.getmembers",
            "fbml.getcustomtags",
            "feed.getappfriendstories",
            "feed.getregisteredtemplatebundlebyid",
            "feed.getregisteredtemplatebundles",
            "fql.multiquery",
            "fql.query",
            "friends.arefriends",
            "friends.get",
            "friends.getappusers",
            "friends.getlists",
            "friends.getmutualfriends",
            "gifts.get",
            "groups.get",
            "groups.getmembers",
            "intl.gettranslations",
            "links.get",
            "notes.get",
            "notifications.get",
            "pages.getinfo",
            "pages.isadmin",
            "pages.isappadded",
            "pages.isfan",
            "permissions.checkavailableapiaccess",
            "permissions.checkgrantedapiaccess",
            "photos.get",
            "photos.getalbums",
            "photos.gettags",
            "profile.getinfo",
            "profile.getinfooptions",
            "stream.get",
            "stream.getcomments",
            "stream.getfilters",
            "users.getinfo",
            "users.getloggedinuser",
            "users.getstandardinfo",
            "users.hasapppermission",
            "users.isappuser",
            "users.isverified",
            "video.getuploadlimits" 
        };

        private string _accessToken;
        private bool _isSecureConnection;
        private bool _useFacebookBeta;

        private Func<object, string> _serializeJson;
        private static Func<object, string> _defaultJsonSerializer;

        private Func<string, Type, object> _deserializeJson;
        private static Func<string, Type, object> _defaultJsonDeserializer;

        /// <summary>
        /// Gets or sets the default facebook application.
        /// </summary>
        public static IFacebookApplication DefaultFacebookApplication { get; set; }

        /// <remarks>For unit testing</remarks>
        internal Func<string> Boundary { get; set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public virtual string AccessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether to add return_ssl_resource as default parameter in every request.
        /// </summary>
        public virtual bool IsSecureConnection
        {
            get { return _isSecureConnection; }
            set { _isSecureConnection = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether to use Facebook beta.
        /// </summary>
        public virtual bool UseFacebookBeta
        {
            get { return _useFacebookBeta; }
            set { _useFacebookBeta = value; }
        }

        /// <summary>
        /// Serialize object to json.
        /// </summary>
        public virtual Func<object, string> SerializeJson
        {
            get { return _serializeJson ?? (_serializeJson = _defaultJsonSerializer); }
            set { _serializeJson = value; }
        }

        /// <summary>
        /// Deserialize json to object.
        /// </summary>
        public virtual Func<string, Type, object> DeserializeJson
        {
            get { return _deserializeJson ?? (_deserializeJson = _defaultJsonDeserializer); }
            set { _deserializeJson = value; }
        }

        /// <summary>
        /// Http web request factory.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual Func<Uri, HttpWebRequestWrapper> HttpWebRequestFactory { get; set; }

        static FacebookApi()
        {
            SetDefaultJsonSerializers(null, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        public FacebookApi()
        {
            _deserializeJson = _defaultJsonDeserializer;
            if (DefaultFacebookApplication != null)
            {
                _useFacebookBeta = DefaultFacebookApplication.IsSecureConnection;
                _isSecureConnection = DefaultFacebookApplication.IsSecureConnection;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        /// <param name="accessToken">The facebook access_token.</param>
        /// <exception cref="ArgumentNullException">Access token in null or empty.</exception>
        public FacebookApi(string accessToken)
            : this()
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException("accessToken");

            _accessToken = accessToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        /// <param name="facebookApplication">The facebook application.</param>
        public FacebookApi(IFacebookApplication facebookApplication)
            : this()
        {
            if (facebookApplication == null)
                return;

            if (!string.IsNullOrEmpty(facebookApplication.AppId) && !string.IsNullOrEmpty(facebookApplication.AppSecret))
                _accessToken = string.Concat(facebookApplication.AppId, "|", facebookApplication.AppSecret);

            _useFacebookBeta = facebookApplication.UseFacebookBeta;
            _isSecureConnection = facebookApplication.IsSecureConnection;
        }

        /// <summary>
        /// Sets the default json seriazliers and deserializers.
        /// </summary>
        /// <param name="jsonSerializer">Json serializer</param>
        /// <param name="jsonDeserializer">Jsonn deserializer</param>
        public static void SetDefaultJsonSerializers(Func<object, string> jsonSerializer, Func<string, Type, object> jsonDeserializer)
        {
            _defaultJsonSerializer = jsonSerializer ?? SimpleJson.SimpleJson.SerializeObject;
            _defaultJsonDeserializer = jsonDeserializer ?? SimpleJson.SimpleJson.DeserializeObject;
        }

#if !(SILVERLIGHT || WINDOWS_PHONE || WINRT)

        /// <summary>
        /// Makes a request to the Facebook server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="resultType">The type of deserialize object into.</param>
        /// <returns>The json result with headers.</returns>
        public virtual object Api(string httpMethod, string path, object parameters, Type resultType)
        {
            Stream input;
            var httpHelper = PrepareRequest(httpMethod, path, parameters, resultType, out input);

            if (input != null)
            {
                try
                {
                    using (var stream = httpHelper.OpenWrite())
                    {
                        // write input to requestStream
                        var buffer = new byte[BufferSize];
                        while (true)
                        {
                            int bytesRead = input.Read(buffer, 0, buffer.Length);
                            if (bytesRead <= 0) break;
                            stream.Write(buffer, 0, bytesRead);
                            stream.Flush();
                        }
                    }
                }
                catch (WebExceptionWrapper ex)
                {
                    if (ex.GetResponse() == null) throw;
                }
                finally
                {
                    input.Dispose();
                }
            }

            Stream responseStream = null;
            object result = null;
            try
            {
                responseStream = httpHelper.OpenRead();
            }
            catch (WebExceptionWrapper ex)
            {
                if (ex.GetResponse() == null) throw;
                responseStream = httpHelper.OpenRead();
            }
            finally
            {
                if (responseStream != null)
                {
                    string responseString;
                    using (var stream = responseStream)
                    {
                        var response = httpHelper.HttpWebResponse;
                        using (var reader = new StreamReader(stream))
                        {
                            responseString = reader.ReadToEnd();
                        }
                    }

                    result = ProcessResponse(httpHelper, responseString, resultType);
                }
            }

            return result;
        }

#endif

        /// <summary>
        /// Cancels asynchronous requests.
        /// </summary>
        /// <remarks>
        /// Does not cancel requests created using XTaskAsync methods.
        /// </remarks>
        public virtual void CancelAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="resultType">The type of deserialize object into.</param>
        /// <param name="userState">The user state.</param>
#if FLUENTHTTP_CORE_TPL
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void ApiAsync(string httpMethod, string path, object parameters, Type resultType, object userState)
        {
            throw new NotImplementedException();
        }

#if FLUENTHTTP_CORE_TPL

        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="resultType">The type of deserialize object into.</param>
        /// <param name="userState">The user state.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="uploadProgress">The upload progress</param>
        /// <returns>The task of json result with headers.</returns>
        public virtual Task<object> ApiTaskAsync(string httpMethod, string path, object parameters, Type resultType, object userState, CancellationToken cancellationToken
#if ASYNC_AWAIT
, IProgress<FacebookUploadProgressChangedEventArgs> uploadProgress
#endif
)
        {
            throw new NotImplementedException();
        }

#if ASYNC_AWAIT

        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="resultType">The type of deserialize object into.</param>
        /// <param name="userState">The user state.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task of json result with headers.</returns>
        public virtual Task<object> ApiTaskAsync(string httpMethod, string path, object parameters, Type resultType, object userState, CancellationToken cancellationToken)
        {
            return ApiTaskAsync(httpMethod, path, parameters, resultType, userState, cancellationToken, null);
        }

#endif

#endif

        private HttpHelper PrepareRequest(string httpMethod, string path, object parameters, Type resultType, out Stream input)
        {
            if (string.IsNullOrEmpty(httpMethod))
                throw new ArgumentNullException();
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException();

            httpMethod = httpMethod.ToUpperInvariant();
            input = null;

            IDictionary<string, FacebookMediaObject> mediaObjects;
            IDictionary<string, FacebookMediaStream> mediaStreams;
            IDictionary<string, object> parametersWithoutMediaObjects = ToDictionary(parameters, out mediaObjects, out mediaStreams) ?? new Dictionary<string, object>();

            if (!parametersWithoutMediaObjects.ContainsKey("access_token") && !string.IsNullOrEmpty(AccessToken))
                parametersWithoutMediaObjects["access_token"] = AccessToken;
            if (!parametersWithoutMediaObjects.ContainsKey("return_ssl_resources") && IsSecureConnection)
                parametersWithoutMediaObjects["return_ssl_resources"] = true;

            bool legacyRestApi = false;
            Uri uri;
            if (Uri.TryCreate(path, UriKind.Absolute, out uri))
            {
                switch (uri.Host)
                {
                    // graph api
                    case "graph.facebook.com":
                    case "graph-video.facebook.com":
                    case "graph.beta.facebook.com":
                    case "graph-video.beta.facebook.com":
                        // If the host is graph.facebook.com the user has passed in the full url.
                        // We remove the host part and continue with the parsing.
                        path = string.Concat(uri.AbsolutePath, uri.Query);
                        break;
                    // legacy rest api
                    case "api.facebook.com":
                    case "api-read.facebook.com":
                    case "api-video.facebook.com":
                    case "api.beta.facebook.com":
                    case "api-read.beta.facebook.com":
                    case "api-video.beta.facebook.com":
                        // If the host is graph.facebook.com the user has passed in the full url.
                        // We remove the host part and continue with the parsing.
                        path = string.Concat(uri.AbsolutePath, uri.Query);
                        legacyRestApi = true;
                        break;
                    default:
                        // If the url is a valid absolute url we are passing the full url as the 'id'
                        // parameter of the query. For example, if path is something like
                        // http://www.microsoft.com/page.aspx?id=23 it means that we are trying
                        // to make the request https://graph.facebook.com/http://www.microsoft.com/page.aspx%3Fid%3D23
                        // So we are just going to return the path
                        uri = null;
                        break;
                }
            }

            // Clean the path, remove leading '/'.
            if (path.Length > 0 && path[0] == '/')
                path = path.Substring(1);

            // If the url does not have a host it means we are using a url
            // like /me or /me?fields=first_name,last_name so we want to
            // remove the querystring info and add it to parameters
            var parts = path.Split(new[] { '?' });
            path = parts[0]; // Set the path to only the path portion of the url

            if (parts.Length == 2 && parts[1] != null)
            {
                // Add the query string values to the parameters dictionary
                var qs = parts[1];
                var qsItems = qs.Split('&');

                foreach (var kvp in qsItems)
                {
                    if (!string.IsNullOrEmpty(kvp))
                    {
                        var qsPart = kvp.Split('=');
                        if (qsPart.Length == 2 && !string.IsNullOrEmpty(qsPart[0]))
                        {
                            var key = HttpHelper.UrlDecode(qsPart[0]);
                            if (!parametersWithoutMediaObjects.ContainsKey(key))
                                parametersWithoutMediaObjects[key] = HttpHelper.UrlDecode(qsPart[1]);
                        }
                        else
                            throw new ArgumentException("Invalid path", "path");
                    }
                }
            }
            else if (parts.Length > 2)
            {
                throw new ArgumentException("Invalid path", "path");
            }

            if (parametersWithoutMediaObjects.ContainsKey("format"))
                parametersWithoutMediaObjects["format"] = "json-strings";

            string restMethod = null;
            if (parametersWithoutMediaObjects.ContainsKey("method"))
            {
                restMethod = (string)parametersWithoutMediaObjects["method"];
                legacyRestApi = true;
            }
            else
            {
                if (legacyRestApi)
                    throw new InvalidOperationException("Legacy rest api 'method' required in parameters.");
            }

            UriBuilder uriBuilder;
            if (uri == null)
            {
                uriBuilder = new UriBuilder { Scheme = "https" };

                if (legacyRestApi)
                {
                    if (string.IsNullOrEmpty(restMethod))
                        throw new InvalidOperationException("Legacy rest api 'method' in parameters is null or empty.");
                    if (restMethod.Equals("video.upload"))
                        uriBuilder.Host = UseFacebookBeta ? "api-video.beta.facebook.com" : "api-video.facebook.com";
                    else if (LegacyRestApiReadOnlyCalls.Contains(restMethod))
                        uriBuilder.Host = UseFacebookBeta ? "api-read.beta.facebook.com" : "api-read.facebook.com";
                    else
                        uriBuilder.Host = UseFacebookBeta ? "api.beta.facebook.com" : "api.facebook.com";
                }
                else
                {
                    if (httpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) && path.EndsWith("/videos"))
                        uriBuilder.Host = UseFacebookBeta ? "graph-video.beta.facebook.com" : "graph-video.facebook.com";
                    else
                        uriBuilder.Host = UseFacebookBeta ? "graph.beta.facebook.com" : "graph.facebook.com";
                }
            }
            else
            {
                uriBuilder = new UriBuilder(uri.Host, uri.Scheme);
            }

            uriBuilder.Path = path;

            string contentType = null;
            var queryString = new StringBuilder();

            if (parametersWithoutMediaObjects.ContainsKey("access_token"))
            {
                var accessToken = parametersWithoutMediaObjects["access_token"];
                if (accessToken == null)
                    parametersWithoutMediaObjects.Remove("access_token");
            }

            if (!httpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                // for GET,DELETE
                if (mediaObjects.Count > 0 && mediaStreams.Count > 0)
                    throw new InvalidOperationException("Attachments (FacebookMediaObject/FacebookMediaStream) are valid only in POST requests.");

#if SILVERLIGHT
                if (httpMethod.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
                    queryString.Append("method=delete&");
#endif
                foreach (var kvp in parametersWithoutMediaObjects)
                    queryString.AppendFormat("{0}={1}&", HttpHelper.UrlEncode(kvp.Key), HttpHelper.UrlEncode(BuildHttpQuery(kvp.Value, HttpHelper.UrlEncode)));

                if (queryString.Length > 0)
                    queryString.Length--;
            }
            else
            {
                if (parametersWithoutMediaObjects.ContainsKey("access_token"))
                {
                    var accessToken = parametersWithoutMediaObjects["acess_token"];
                    queryString.AppendFormat("access_token={0}", parametersWithoutMediaObjects["access_token"]);
                    parametersWithoutMediaObjects.Remove("access_token");
                }

                if (mediaObjects.Count == 0 && mediaStreams.Count == 0)
                {
                    contentType = "application/x-www-form-urlencoded";
                    //var data = Encoding.UTF8.GetBytes(FacebookUtils.ToJsonQueryString(parameters));
                    //input = data.Length == 0 ? null : new MemoryStream(data);
                    throw new NotImplementedException();
                }
                else
                {
                    string boundary = Boundary == null
                                          ? DateTime.UtcNow.Ticks.ToString("x", CultureInfo.InvariantCulture)
                                          : Boundary();

                    contentType = string.Concat("multipart/form-data; boundary=", boundary);

                    var streams = new List<Stream>();

                    // Build up the post message header
                    var sb = new StringBuilder();

                    foreach (var kvp in parametersWithoutMediaObjects)
                    {
                        sb.Append(MultiPartFormPrefix).Append(boundary).Append(MultiPartNewLine);
                        sb.Append("Content-Disposition: form-data; name=\"").Append(kvp.Key).Append("\"");
                        sb.Append(MultiPartNewLine);
                        sb.Append(MultiPartNewLine);

                        // format object As json And Remove leading and trailing parenthesis
                        //string jsonValue = FacebookUtils.ToJsonString(kvp.Value);

                        //sb.Append(jsonValue);
                        sb.Append(FacebookUtils.MultiPartNewLine);
                    }

                    streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())));
                    foreach (var facebookMediaObject in mediaObjects)
                    {
                        var sbMediaObject = new StringBuilder();
                        var mediaObject = facebookMediaObject.Value;

                        if (mediaObject.ContentType == null || mediaObject.GetValue() == null || string.IsNullOrEmpty(mediaObject.FileName))
                            throw new InvalidOperationException(MediaObjectMustHavePropertiesSetError);

                        sbMediaObject.Append(MultiPartFormPrefix).Append(boundary).Append(MultiPartNewLine);
                        sbMediaObject.Append("Content-Disposition: form-data; name=\"").Append(facebookMediaObject.Key).Append("\"; filename=\"").Append(mediaObject.FileName).Append("\"").Append(MultiPartNewLine);
                        sbMediaObject.Append("Content-Type: ").Append(mediaObject.ContentType).Append(MultiPartNewLine).Append(MultiPartNewLine);

                        streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(sbMediaObject.ToString())));

                        byte[] fileData = mediaObject.GetValue();

                        if (fileData == null)
                            throw new InvalidOperationException(MediaObjectValueIsNull);

                        streams.Add(new MemoryStream(fileData));
                        streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(MultiPartNewLine)));
                    }

                    foreach (var facebookMediaStream in mediaStreams)
                    {
                        throw new NotImplementedException();
                    }

                    streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(String.Concat(FacebookUtils.MultiPartNewLine, FacebookUtils.MultiPartFormPrefix, boundary, FacebookUtils.MultiPartFormPrefix, FacebookUtils.MultiPartNewLine))));
                    input = new CombinationStream.CombinationStream(streams);
                }
            }

            uriBuilder.Query = queryString.ToString();

            var request = HttpWebRequestFactory == null
                             ? new HttpWebRequestWrapper((System.Net.HttpWebRequest)System.Net.WebRequest.Create(uriBuilder.Uri))
                             : HttpWebRequestFactory(uriBuilder.Uri);
            request.Method = httpMethod;
            request.ContentType = contentType;
            // request.AllowAutoRedirect = false;

#if !(SILVERLIGHT || WINRT)
            request.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;
#endif

            if (input != null)
                request.TrySetContentLength(input.Length);

            return new HttpHelper(request);
        }

        private object ProcessResponse(HttpHelper httpHelper, string responseString, Type resultType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries parsing the facebook signed_request.
        /// </summary>
        /// <param name="appSecret">The app secret.</param>
        /// <param name="signedRequestValue">The signed_request value.</param>
        /// <param name="signedRequest">The parsed signed request.</param>
        /// <returns>True if signed request parsed successfully otherwise false.</returns>
        public virtual bool TryParseSignedRequest(string appSecret, string signedRequestValue, out object signedRequest)
        {
            signedRequest = null;
            try
            {
                signedRequest = ParseSignedRequest(appSecret, signedRequestValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Parse the facebook signed_request.
        /// </summary>
        /// <param name="appSecret">The appsecret.</param>
        /// <param name="signedRequestValue">The signed_request value.</param>
        /// <returns>The parse signed_request value.</returns>
        /// <exception cref="ArgumentNullException">Throws if appSecret or signedRequestValue is null or empty.</exception>
        /// <exception cref="InvalidOperationException">If the signedRequestValue is an invalid signed_request.</exception>
        public virtual object ParseSignedRequest(string appSecret, string signedRequestValue)
        {
            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException("appSecret");
            if (string.IsNullOrEmpty(signedRequestValue))
                throw new ArgumentNullException("signedRequestValue");

            string[] split = signedRequestValue.Split('.');
            if (split.Length != 2)
            {
                // need to have exactly 2 parts
                throw new InvalidOperationException(InvalidSignedRequest);
            }

            string encodedignature = split[0];
            string encodedEnvelope = split[1];

            if (string.IsNullOrEmpty(encodedignature) || string.IsNullOrEmpty(encodedEnvelope))
                throw new InvalidOperationException(InvalidSignedRequest);

            var base64UrlDecoded = Base64UrlDecode(encodedEnvelope);
            var envelope = (IDictionary<string, object>)DeserializeJson(Encoding.UTF8.GetString(base64UrlDecoded, 0, base64UrlDecoded.Length), null);
            var algorithm = (string)envelope["algorithm"];
            if (!algorithm.Equals("HMAC-SHA256"))
                throw new InvalidOperationException("Unknown algorithm. Expected HMAC-SHA256");

            byte[] key = Encoding.UTF8.GetBytes(appSecret);
            IEnumerable<byte> digest = ComputeHmacSha256Hash(Encoding.UTF8.GetBytes(encodedEnvelope), key);

            if (!digest.SequenceEqual(Base64UrlDecode(encodedignature)))
                throw new InvalidOperationException(InvalidSignedRequest);
            return envelope;
        }

        /// <summary>
        /// Base64 Url decode.
        /// </summary>
        /// <param name="base64UrlSafeString">
        /// The base 64 url safe string.
        /// </param>
        /// <returns>
        /// The base 64 url decoded string.
        /// </returns>c
        private static byte[] Base64UrlDecode(string base64UrlSafeString)
        {
            if (string.IsNullOrEmpty(base64UrlSafeString))
                throw new ArgumentNullException("base64UrlSafeString");

            base64UrlSafeString =
                base64UrlSafeString.PadRight(base64UrlSafeString.Length + (4 - base64UrlSafeString.Length % 4) % 4, '=');
            base64UrlSafeString = base64UrlSafeString.Replace('-', '+').Replace('_', '/');
            return Convert.FromBase64String(base64UrlSafeString);
        }

        /// <summary>
        /// Computes the Hmac Sha 256 Hash.
        /// </summary>
        /// <param name="data">
        /// The data to hash.
        /// </param>
        /// <param name="key">
        /// The hash key.
        /// </param>
        /// <returns>
        /// The Hmac Sha 256 hash.
        /// </returns>
        private static IEnumerable<byte> ComputeHmacSha256Hash(byte[] data, byte[] key)
        {
            using (var crypto = new HMACSHA256(key))
            {
                return crypto.ComputeHash(data);
            }
        }

        protected virtual IDictionary<string, object> ToDictionary(object parameters, out IDictionary<string, FacebookMediaObject> mediaObjects, out IDictionary<string, FacebookMediaStream> mediaStreams)
        {
            mediaObjects = new Dictionary<string, FacebookMediaObject>();
            mediaStreams = new Dictionary<string, FacebookMediaStream>();

            var dictionary = parameters as IDictionary<string, object>;

            // todo: IEnumerable<KeyValuePair<T1,T2>> and IEnumerable<Tuple<T1, T2>>

            if (dictionary == null)
            {
                if (parameters == null)
                    return null;

                // convert anonymous objects / unknown objects to IDictionary<string, object>
                dictionary = new Dictionary<string, object>();
                foreach (var propertyInfo in parameters.GetType().GetProperties())
                {
                    if (!propertyInfo.CanRead) continue;
                    dictionary[propertyInfo.Name] = propertyInfo.GetValue(parameters, null);
                }
                return dictionary;
            }

            foreach (var parameter in dictionary)
            {
                if (parameter.Value is FacebookMediaObject)
                    mediaObjects.Add(parameter.Key, (FacebookMediaObject)parameter.Value);
                else if (parameter.Value is FacebookMediaStream)
                    mediaStreams.Add(parameter.Key, (FacebookMediaStream)parameter.Value);
            }

            foreach (var mediaObject in mediaObjects)
                dictionary.Remove(mediaObject.Key);

            foreach (var mediaStream in mediaStreams)
                dictionary.Remove(mediaStream.Key);

            return dictionary;
        }

        protected string BuildHttpQuery(object parameter, Func<string, string> encode)
        {
            if (parameter == null)
                return "null";
            if (parameter is string)
                return (string)parameter;
            if (parameter is bool)
                return (bool)parameter ? "true" : "false";

            if (parameter is int || parameter is long ||
                parameter is float || parameter is double || parameter is decimal ||
                parameter is byte || parameter is sbyte ||
                parameter is short || parameter is ushort ||
                parameter is uint || parameter is ulong)
                return parameter.ToString();

            // todo: IEnumerable<KeyValuePair<T1,T2>> and IEnumerable<Tuple<T1, T2>>

            var sb = new StringBuilder();
            if (parameter is IEnumerable<KeyValuePair<string, object>>)
            {
                foreach (var kvp in (IEnumerable<KeyValuePair<string, object>>)parameter)
                    sb.AppendFormat("{0}={1}&", encode(kvp.Key), encode(BuildHttpQuery(kvp.Value, encode)));
            }
            else if (parameter is IEnumerable<KeyValuePair<string, string>>)
            {
                foreach (var kvp in (IEnumerable<KeyValuePair<string, string>>)parameter)
                    sb.AppendFormat("{0}={1}&", encode(kvp.Key), encode(kvp.Value));
            }
            else if (parameter is IEnumerable)
            {
                foreach (var obj in (IEnumerable)parameter)
                    sb.AppendFormat("{0},", encode(BuildHttpQuery(obj, encode)));
            }
            else if (parameter is DateTime)
            {
                return DateTimeConvertor.ToIso8601FormattedDateTime((DateTime)parameter);
            }
            else
            {
                IDictionary<string, FacebookMediaObject> mediaObjects;
                IDictionary<string, FacebookMediaStream> mediaStreams;
                var dict = ToDictionary(parameter, out mediaObjects, out mediaStreams);
                if (mediaObjects.Count > 0 || mediaStreams.Count > 0)
                    throw new InvalidOperationException("Parameter can contain attachements (FacebookMediaObject/FacebookMediaStream) only in the top most level.");
                return BuildHttpQuery(dict, encode);
            }

            if (sb.Length > 0)
                sb.Length--;

            return sb.ToString();
        }
    }
}