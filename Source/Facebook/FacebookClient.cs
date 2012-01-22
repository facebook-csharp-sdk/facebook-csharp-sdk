// --------------------------------
// <copyright file="FacebookClient.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Globalization;
    using System.Linq;
    using System.Net;
#if TYPEINFO
    using System.Reflection;
#endif
    using System.Text;
    using FluentHttp;

    /// <summary>
    /// Provides access to the Facbook Platform.
    /// </summary>
    public partial class FacebookClient
    {
        private const int BufferSize = 1024 * 4; // 4kb
        private const string AttachmentMustHavePropertiesSetError = "Attachment (FacebookMediaObject/FacebookMediaStream) must have a content type, file name, and value set.";
        private const string AttachmentValueIsNull = "The value of attachment (FacebookMediaObject/FacebookMediaStream) is null.";
        private const string UnknownResponse = "Unknown facebook response.";
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

        private Func<Uri, HttpWebRequestWrapper> _httpWebRequestFactory;
        private static Func<Uri, HttpWebRequestWrapper> _defaultHttpWebRequestFactory;

        private static IFacebookApplication _defaultFacebookApplication;

        /// <summary>
        /// Gets or sets the default facebook application.
        /// </summary>
        public static IFacebookApplication DefaultFacebookApplication
        {
            get { return _defaultFacebookApplication; }
            set { _defaultFacebookApplication = value ?? (_defaultFacebookApplication ?? new FacebookApplication()); }
        }

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
            get { return _deserializeJson; }
            set { _deserializeJson = value ?? (_deserializeJson = _defaultJsonDeserializer); ; }
        }

        /// <summary>
        /// Http web request factory.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual Func<Uri, HttpWebRequestWrapper> HttpWebRequestFactory
        {
            get { return _httpWebRequestFactory; }
            set { _httpWebRequestFactory = value ?? (_httpWebRequestFactory = _defaultHttpWebRequestFactory); }
        }

        static FacebookClient()
        {
            SetDefaultJsonSerializers(null, null);
            DefaultFacebookApplication = null;
            SetDefaultHttpWebRequestFactory(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        public FacebookClient()
        {
            _deserializeJson = _defaultJsonDeserializer;
            _httpWebRequestFactory = _defaultHttpWebRequestFactory;

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
        public FacebookClient(string accessToken)
            : this()
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException("accessToken");

            _accessToken = accessToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public FacebookClient(string appId, string appSecret)
            : this()
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException("appId");
            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException("appSecret");

            _accessToken = string.Concat(appId, "|", appSecret);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        /// <param name="facebookApplication">The facebook application.</param>
        public FacebookClient(IFacebookApplication facebookApplication)
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

        /// <summary>
        /// Sets the default http web request factory.
        /// </summary>
        /// <param name="httpWebRequestFactory"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetDefaultHttpWebRequestFactory(Func<Uri, HttpWebRequestWrapper> httpWebRequestFactory)
        {
            _defaultHttpWebRequestFactory = httpWebRequestFactory;
        }

        private HttpHelper PrepareRequest(string httpMethod, string path, object parameters, Type resultType, out Stream input, out bool isLegacyRestApi)
        {
            if (string.IsNullOrEmpty(httpMethod))
                throw new ArgumentNullException("httpMethod");
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            input = null;
            httpMethod = httpMethod.ToUpperInvariant();

            IDictionary<string, FacebookMediaObject> mediaObjects;
            IDictionary<string, FacebookMediaStream> mediaStreams;
            IDictionary<string, object> parametersWithoutMediaObjects = ToDictionary(parameters, out mediaObjects, out mediaStreams) ?? new Dictionary<string, object>();

            if (!parametersWithoutMediaObjects.ContainsKey("access_token") && !string.IsNullOrEmpty(AccessToken))
                parametersWithoutMediaObjects["access_token"] = AccessToken;
            if (!parametersWithoutMediaObjects.ContainsKey("return_ssl_resources") && IsSecureConnection)
                parametersWithoutMediaObjects["return_ssl_resources"] = true;

            isLegacyRestApi = false;
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
                        isLegacyRestApi = true;
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

            path = ParseUrlQueryString(path, parametersWithoutMediaObjects);

            if (parametersWithoutMediaObjects.ContainsKey("format"))
            {
                if (isLegacyRestApi)
                    parametersWithoutMediaObjects["format"] = "json-strings";
                else
                    parametersWithoutMediaObjects.Remove("format");
            }

            string restMethod = null;
            if (parametersWithoutMediaObjects.ContainsKey("method"))
            {
                restMethod = (string)parametersWithoutMediaObjects["method"];
                isLegacyRestApi = true;
            }
            else
            {
                if (isLegacyRestApi)
                    throw new InvalidOperationException("Legacy rest api 'method' required in parameters.");
            }

            UriBuilder uriBuilder;
            if (uri == null)
            {
                uriBuilder = new UriBuilder { Scheme = "https" };

                if (isLegacyRestApi)
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
                if (accessToken == null || (accessToken is string && (string.IsNullOrEmpty((string)accessToken))))
                    parametersWithoutMediaObjects.Remove("access_token");
                else
                    queryString.AppendFormat("access_token={0}&", accessToken);
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
            }
            else
            {
                if (mediaObjects.Count == 0 && mediaStreams.Count == 0)
                {
                    contentType = "application/x-www-form-urlencoded";
                    var sb = new StringBuilder();
                    foreach (var kvp in parametersWithoutMediaObjects)
                        sb.AppendFormat("{0}={1}", HttpHelper.UrlEncode(kvp.Key), HttpHelper.UrlEncode(BuildHttpQuery(kvp.Value, HttpHelper.UrlEncode)));
                    input = sb.Length == 0 ? null : new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
                }
                else
                {
                    string boundary = Boundary == null
                                          ? DateTime.UtcNow.Ticks.ToString("x", CultureInfo.InvariantCulture) // for unit testing
                                          : Boundary();

                    contentType = string.Concat("multipart/form-data; boundary=", boundary);

                    var streams = new List<Stream>();

                    // Build up the post message header
                    var sb = new StringBuilder();

                    foreach (var kvp in parametersWithoutMediaObjects)
                    {
                        sb.Append(MultiPartFormPrefix).Append(boundary).Append(MultiPartNewLine);
                        sb.Append("Content-Disposition: form-data; name=\"").Append(kvp.Key).Append("\"");
                        sb.Append(MultiPartNewLine).Append(MultiPartNewLine);
                        sb.Append(BuildHttpQuery(kvp.Value, HttpHelper.UrlEncode));
                        sb.Append(MultiPartNewLine);
                    }

                    streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())));

                    foreach (var facebookMediaObject in mediaObjects)
                    {
                        var sbMediaObject = new StringBuilder();
                        var mediaObject = facebookMediaObject.Value;

                        if (mediaObject.ContentType == null || mediaObject.GetValue() == null || string.IsNullOrEmpty(mediaObject.FileName))
                            throw new InvalidOperationException(AttachmentMustHavePropertiesSetError);

                        sbMediaObject.Append(MultiPartFormPrefix).Append(boundary).Append(MultiPartNewLine);
                        sbMediaObject.Append("Content-Disposition: form-data; name=\"").Append(facebookMediaObject.Key).Append("\"; filename=\"").Append(mediaObject.FileName).Append("\"").Append(MultiPartNewLine);
                        sbMediaObject.Append("Content-Type: ").Append(mediaObject.ContentType).Append(MultiPartNewLine).Append(MultiPartNewLine);

                        streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(sbMediaObject.ToString())));

                        byte[] fileData = mediaObject.GetValue();

                        if (fileData == null)
                            throw new InvalidOperationException(AttachmentValueIsNull);

                        streams.Add(new MemoryStream(fileData));
                        streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(MultiPartNewLine)));
                    }

                    foreach (var facebookMediaStream in mediaStreams)
                    {
                        var sbMediaStream = new StringBuilder();
                        var mediaStream = facebookMediaStream.Value;

                        if (mediaStream.ContentType == null || mediaStream.GetValue() == null || string.IsNullOrEmpty(mediaStream.FileName))
                            throw new InvalidOperationException(AttachmentMustHavePropertiesSetError);

                        sbMediaStream.Append(MultiPartFormPrefix).Append(boundary).Append(MultiPartNewLine);
                        sbMediaStream.Append("Content-Disposition: form-data; name=\"").Append(facebookMediaStream.Key).Append("\"; filename=\"").Append(mediaStream.FileName).Append("\"").Append(MultiPartNewLine);
                        sbMediaStream.Append("Content-Type: ").Append(mediaStream.ContentType).Append(MultiPartNewLine).Append(MultiPartNewLine);

                        streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(sbMediaStream.ToString())));

                        var stream = mediaStream.GetValue();

                        if (stream == null)
                            throw new InvalidOperationException(AttachmentValueIsNull);

                        streams.Add(stream);
                        streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(MultiPartNewLine)));
                    }

                    streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(String.Concat(MultiPartNewLine, MultiPartFormPrefix, boundary, MultiPartFormPrefix, MultiPartNewLine))));
                    input = new CombinationStream.CombinationStream(streams);
                }
            }

            if (queryString.Length > 0)
                queryString.Length--;

            uriBuilder.Query = queryString.ToString();

            var request = HttpWebRequestFactory == null
                             ? new HttpWebRequestWrapper((System.Net.HttpWebRequest)System.Net.WebRequest.Create(uriBuilder.Uri))
                             : HttpWebRequestFactory(uriBuilder.Uri);
            request.Method = httpMethod;
            request.ContentType = contentType;
            // request.AllowAutoRedirect = false;

#if !(SILVERLIGHT || WINRT)
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
#endif

            if (input != null)
                request.TrySetContentLength(input.Length);

            return new HttpHelper(request);
        }

        private object ProcessResponse(HttpHelper httpHelper, string responseString, Type resultType, bool isLegacyRestApi)
        {
            try
            {
                var response = httpHelper.HttpWebResponse;
                var json = new JsonObject();
                if (response == null)
                    throw new InvalidOperationException(UnknownResponse);

                var headers = new JsonObject();
                foreach (var headerName in response.Headers.AllKeys)
                    headers[headerName] = response.Headers[headerName];

                json["headers"] = headers;
                json["content"] = responseString;

                if (response.ContentType.Contains("application/json"))
                {
                    json["body"] = DeserializeJson(responseString, resultType);
                }
                else if (!isLegacyRestApi && response.StatusCode == HttpStatusCode.OK && response.ContentType.Contains("text/plain"))
                {
                    if (response.ResponseUri.AbsolutePath == "/oauth/access_token")
                    {
                        var body = new JsonObject();
                        foreach (var kvp in responseString.Split('&'))
                        {
                            var split = kvp.Split('=');
                            if (split.Length == 2)
                                body[split[0]] = split[1];
                        }

                        if (body.ContainsKey("expires"))
                            body["expires"] = Convert.ToInt64(body["expires"]);

                        json["body"] = resultType == null ? body : DeserializeJson(body.ToString(), resultType);

                        return json;
                    }
                    else
                    {
                        throw new InvalidOperationException(UnknownResponse);
                    }
                }
                else
                {
                    throw new InvalidOperationException(UnknownResponse);
                }

                #region Check for exceptions

                if (isLegacyRestApi)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }

                #endregion

            }
            catch (Exception)
            {
                if (httpHelper.InnerException != null)
                    throw httpHelper.InnerException;
                throw;
            }
        }

        /// <summary>
        /// Converts the parameters to IDictionary&lt;string,object&gt;
        /// </summary>
        /// <param name="parameters">The parameter to convert.</param>
        /// <param name="mediaObjects">The extracted Facebook media objects.</param>
        /// <param name="mediaStreams">The extracted Facebook media streams.</param>
        /// <returns>The converted dictionary.</returns>
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
#if TYPEINFO
                foreach (var propertyInfo in parameters.GetType().GetTypeInfo().DeclaredProperties.Where(p => p.CanRead))
                {
                    dictionary[propertyInfo.Name] = propertyInfo.GetValue(parameters, null);
                }
#else
                foreach (var propertyInfo in parameters.GetType().GetProperties())
                {
                    if (!propertyInfo.CanRead) continue;
                    dictionary[propertyInfo.Name] = propertyInfo.GetValue(parameters, null);
                }
#endif
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

        /// <summary>
        /// Converts the parameters to http query.
        /// </summary>
        /// <param name="parameter">The parameter to convert.</param>
        /// <param name="encode">Url encoder function.</param>
        /// <returns>The http query.</returns>
        /// <remarks>
        /// The result is not url encoded. The caller needs to take care of url encoding the result.
        /// </remarks>
        protected virtual string BuildHttpQuery(object parameter, Func<string, string> encode)
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

        private static string ParseUrlQueryString(string path, IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            if (string.IsNullOrEmpty(path))
                return string.Empty;

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
                            if (!parameters.ContainsKey(key))
                                parameters[key] = HttpHelper.UrlDecode(qsPart[1]);
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

            return path;
        }

    }
}