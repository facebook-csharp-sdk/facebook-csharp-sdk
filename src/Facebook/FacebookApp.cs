// --------------------------------
// <copyright file="FacebookApp.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using Facebook.Utilities;
using System.IO;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Facebook
{

    /// <summary>
    /// Provides access to the Facebook Platform.
    /// </summary>
    public class FacebookApp : FacebookAppBase
    {
        private const string _prefix = "--";
        private const string _newLine = "\r\n";
        private int _maxRetries = 2;  //How many times to retry a command if an error occurs until we give up.
        private int _retryDelay = 500; // How long in milliseconds to wait before retrying.
        private FacebookSession _session;
#if !SILVERLIGHT && !CLIENTPROFILE
        private FacebookSignedRequest _signedRequest;
#endif

        private static Collection<string> _retryErrorTypes = new Collection<string>()
        {
            "OAuthException", // Graph OAuth Exception
            "190", // Rest OAuth Exception
            "Unknown", // No error info returned by facebook.
        };



        /// <summary>
        /// Initializes the Facebook application with values
        /// stored in the application configuration file or 
        /// with only the default values if the configuration
        /// file does not have the values set.
        /// </summary>
        public FacebookApp()
        {
#if !SILVERLIGHT // Silverlight does not support System.Configuration
            var settings = FacebookSettings.Current;
            if (settings != null)
            {
                ApplySettings(settings);
            }
#endif
        }

        /// <summary>
        /// Initialized the Facebook application with
        /// values provided. Does not require configuration
        /// file to be set.
        /// </summary>
        /// <param name="settings">The facebook settings for the application.</param>
        public FacebookApp(IFacebookSettings settings)
        {
            Contract.Requires(settings != null);

            ApplySettings(settings);
        }

        /// <summary>
        /// Initializes the Facebook application with
        /// only an access_token set. From this state 
        /// sessions will not be accessable.
        /// </summary>
        /// <param name="accessToken">The Facebook access token.</param>
        public FacebookApp(string accessToken)
        {
            Contract.Requires(!String.IsNullOrEmpty(accessToken));

            this.Session = new FacebookSession
            {
                AccessToken = accessToken,
            };
        }

        /// <summary>
        /// How many times to retry a command if an error occurs until we give up.
        /// </summary>
        public int MaxRetries
        {
            get { return _maxRetries; }
            set { _maxRetries = value; }
        }

        /// <summary>
        /// How long in seconds to wait before retrying, with exponential roll off.
        /// </summary>
        public int RetryDelay
        {
            get { return _retryDelay; }
            set { _retryDelay = value; }
        }

        /// <summary>
        /// Gets a collection of Facebook error types that
        /// should be retried in the event of a failure.
        /// </summary>
        protected virtual Collection<string> RetryErrorTypes
        {
            get { return _retryErrorTypes; }
        }


#if !SILVERLIGHT && !CLIENTPROFILE
        /// <summary>
        /// Gets the Current URL, stripping it of known FB parameters that should not persist.
        /// </summary>
        protected override Uri CurrentUrl
        {
            get
            {
                if (System.Web.HttpContext.Current == null ||
                    System.Web.HttpContext.Current.Request == null)
                {
                    return new Uri("http://www.facebook.com/connect/login_success.html");
                }
                return CleanUrl(System.Web.HttpContext.Current.Request.Url);
            }
        }

        /// <summary>
        /// The data from the signed_request token.
        /// </summary>
        public FacebookSignedRequest SignedRequest
        {
            get
            {
                if (this._signedRequest == null &&
                    System.Web.HttpContext.Current != null &&
                    System.Web.HttpContext.Current.Request != null)
                {
                    var request = System.Web.HttpContext.Current.Request;
                    if (request.Params.AllKeys.Contains("signed_request"))
                    {
                        this._signedRequest = ParseSignedRequest(request.Params["signed_request"]);
                    }
                }
                return this._signedRequest;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public override FacebookSession Session
        {
            get
            {
                if (!this.SessionLoaded &&
                    System.Web.HttpContext.Current != null &&
                    System.Web.HttpContext.Current.Request != null)
                {
                    try
                    {
                        var request = System.Web.HttpContext.Current.Request;

                        // try loading session from signed_request
                        if (this.SignedRequest != null)
                        {
                            this._session = CreateSessionFromSignedRequest(this.SignedRequest);
                        }

                        // try loading session from querystring
                        if (this._session == null && request.Params.AllKeys.Contains("session"))
                        {
                            this._session = ParseFromQueryString(request.Params["session"]);
                            ValidateSessionObject(this._session);
                        }

                        // try loading session from cookie if necessary
                        if (this._session == null && this.CookieSupport)
                        {
                            if (request.Params.AllKeys.Contains(this.SessionCookieName))
                            {
                                this._session = ParseFromCookie(request.Params[this.SessionCookieName]);
                                ValidateSessionObject(this._session);
                            }
                        }
                    }
                    catch
                    {
                        this._session = null;
                    }
                    finally
                    {
                        this.SessionLoaded = true;
                        this.SetCookieFromSession(this._session);
                    }
                }
                return this._session;
            }
            set
            {
                this.SessionLoaded = true;
                this._session = value;
                // facebook.php validates the session when you set it.
                // We dont think this is the best approach and so 
                // we have commented this out. If you want to validate
                // the session object before it is set, uncomment the
                // next four lines.
                // if (!ValidateSessionObject(this._session))
                // {
                //     throw new FacebookApiException("The session is not valid.");
                // }
                this.SetCookieFromSession(value);
            }
        }

        /// <summary>
        /// Set a JS Cookie based on the _passed in_ session. It does not use the
        /// currently stored session -- you need to explicitly pass it in.
        /// </summary>
        /// <param name="session">The session to use for setting the cookie. Can be null.</param>
        protected void SetCookieFromSession(FacebookSession session)
        {
            // Check to make sure cookies are supported
            // based on the Facebook Settings.
            if (!this.CookieSupport ||
                System.Web.HttpContext.Current == null ||
                System.Web.HttpContext.Current.Request == null ||
                System.Web.HttpContext.Current.Response == null)
            {
                return;
            }

            var request = System.Web.HttpContext.Current.Request;
            var response = System.Web.HttpContext.Current.Response;

            string value = "deleted";
            DateTime expires = DateTime.Now.AddSeconds(-3600);
            if (session != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                sb.Append(session.Dictionary.ToJsonQueryString());
                sb.Append("\"");
                value = sb.ToString();
                expires = session.Expires;
            }

            // if an existing cookie is not set, we dont need to delete it
            if (value == "deleted" && !request.Cookies.AllKeys.Contains(this.SessionCookieName))
            {
                return;
            }

            // Set the cookie data
            if (request.Cookies.AllKeys.Contains(this.SessionCookieName))
            {
                var cookie = response.Cookies[this.SessionCookieName];
                cookie.Value = value;
                cookie.Expires = expires;
                cookie.Domain = this.BaseDomain;
            }
            else
            {
                response.Cookies.Add(new System.Web.HttpCookie(this.SessionCookieName)
                {
                    Expires = expires,
                    Value = value,
                    Domain = this.BaseDomain
                });
            }
        }
#endif

#if !SILVERLIGHT
        protected override void ValidateSessionObject(FacebookSession session)
        {
            if (session == null)
            {
                return;
            }

            var signature = GenerateSignature(session);
            if (session.Signature == signature.ToString())
            {
                return;
            }
            session = null;
        }

        /// <summary>
        /// Generates a MD5 signature for the facebook session.
        /// </summary>
        /// <param name="session">The session to generate a signature.</param>
        /// <returns>An MD5 signature.</returns>
        /// <exception cref="System.ArgumentNullException">If the session is null.</exception>
        /// <exception cref="System.InvalidOperationException">If there is a problem generating the hash.</exception>
        protected override string GenerateSignature(FacebookSession session)
        {
            var args = session.Dictionary;
            StringBuilder payload = new StringBuilder();
            var parts = (from a in args
                         orderby a.Key
                         where a.Key != "sig"
                         select string.Format(CultureInfo.InvariantCulture, "{0}={1}", a.Key, a.Value)).ToList();
            parts.ForEach((s) => { payload.Append(s); });
            payload.Append(this.ApiSecret);
            byte[] hash = null;
            using (var md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create())
            {
                if (md5 != null)
                {
                    hash = md5.ComputeHash(Encoding.UTF8.GetBytes(payload.ToString()));
                }
            }
            if (hash == null)
            {
                throw new InvalidOperationException("Hash is not valid.");
            }
            StringBuilder signature = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                signature.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
            }
            return signature.ToString();
        }

        /// <summary>
        /// Invoke the old restserver.php endpoint.
        /// </summary>
        /// <param name="parameters">The parameters of the method call.</param>
        /// <param name="httpMethod">The http method for the request. Default is 'GET'.</param>
        /// <returns>The decoded response object.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected override object RestServer(IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            AddRestParameters(parameters);

            Uri uri = this.GetApiUrl(parameters["method"].ToString());
            return this.OAuthRequest(uri, parameters, httpMethod);
        }

        /// <summary>
        /// Make a graph api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <param name="parameters">JsonObject of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected override object Graph(string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            var uri = GetGraphRequestUri(path);

            return this.OAuthRequest(uri, parameters, httpMethod);
        }

        /// <summary>
        /// Make a OAuth Request
        /// </summary>
        /// <param name="uri">The url to make the request.</param>
        /// <param name="parameters">The parameters of the request.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <returns>The decoded response object.</returns>
        protected override object OAuthRequest(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Uri requestUrl;
            string contentType;
            byte[] postData = BuildRequestData(uri, parameters, httpMethod, out requestUrl, out contentType);

            return WithMirrorRetry<object>(() => { return MakeRequest(httpMethod, requestUrl, postData, contentType); });
        }
#endif

        /// <summary>
        /// Invoke the old restserver.php endpoint.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="parameters">The parameters of the method call.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <returns>The decoded response object.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected override void RestServerAsync(FacebookAsyncCallback callback, object state, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            AddRestParameters(parameters);

            Uri uri = this.GetApiUrl(parameters["method"].ToString());

            this.OAuthRequestAsync(callback, state, uri, parameters, httpMethod);
        }

        /// <summary>
        /// Make a graph api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <param name="parameters">JsonObject of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected override void GraphAsync(FacebookAsyncCallback callback, object state, string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            var uri = GetGraphRequestUri(path);

            this.OAuthRequestAsync(callback, state, uri, parameters, httpMethod);
        }

        /// <summary>
        /// Make a OAuth Request
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="uri">The url to make the request.</param>
        /// <param name="parameters">The parameters of the request.</param>
        /// <param name="httpMethod">The http method of the request.</param>
        /// <returns>The decoded response object.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void OAuthRequestAsync(FacebookAsyncCallback callback, object state, Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Uri requestUrl;
            string contentType;
            byte[] postData = BuildRequestData(uri, parameters, httpMethod, out requestUrl, out contentType);

            MakeRequestAsync(callback, state, httpMethod, requestUrl, postData, contentType);
        }

        /// <summary>
        /// Get a Login URL for use with redirects. By default, full page redirect is
        /// assumed. If you are using the generated URL with a window.open() call in
        /// JavaScript, you can pass in display=popup as part of the parameters.
        /// The parameters:
        ///     - next: the url to go to after a successful login
        ///     - cancel_url: the url to go to after the user cancels
        ///     - req_perms: comma separated list of requested extended perms
        ///     - display: can be "page" (default, full page) or "popup"
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public override Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            var currentUrl = this.CurrentUrl.ToString();

            var defaultParams = new Dictionary<string, object>();
            defaultParams["api_key"] = this.AppId;
            defaultParams["cancel_url"] = currentUrl;
            defaultParams["display"] = "page";
            defaultParams["fbconnect"] = 1;
            defaultParams["next"] = currentUrl;
            defaultParams["return_session"] = 1;
            defaultParams["session_version"] = 3;
            defaultParams["v"] = "1.0";

            return this.GetUrl(
                "www",
                "login.php",
                defaultParams.Merge(parameters));
        }

#if CLIENTPROFILE || SILVERLIGHT

        /// <summary>
        /// Gets an OAuth Login URL for use with redirects.
        /// This method is only for use with mobile or desktop
        /// clients.
        /// </summary>
        /// <returns>The URL for the login flow.</returns>
        public Uri GetOAuthLoginUrl()
        {
            return GetOAuthLoginUrl(null);
        }

        /// <summary>
        /// Gets an OAuth Login URL for use with redirects.
        /// This method is only for use with mobile or desktop
        /// clients.
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public Uri GetOAuthLoginUrl(IDictionary<string, object> parameters)
        {
            var currentUrl = this.CurrentUrl.ToString();

            var defaultParams = new Dictionary<string, object>();
            defaultParams["client_id"] = this.AppId;
            defaultParams["display"] = "popup";
            //defaultParams["type"] = "user_agent";
            defaultParams["redirect_uri"] = currentUrl;

            return this.GetUrl(
                "graph",
                "oauth/authorize",
                defaultParams.Merge(parameters));
        }
#endif

        /// <summary>
        /// Get a Logout URL suitable for use with redirects.
        /// The parameters:
        ///     - next: the url to go to after a successful logout
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public override Uri GetLogoutUrl(IDictionary<string, object> parameters)
        {
            var defaultParams = new Dictionary<string, object>();
            defaultParams["api_key"] = this.AppId;
            defaultParams["no_session"] = this.CurrentUrl.ToString();
            if (this.Session != null)
            {
                // If might be better to throw an exception if the
                // session is null because you dont need to logout,
                // but this way makes it easier to build logout links.
                defaultParams["session_key"] = this.Session.SessionKey;
            }

            return this.GetUrl(
                "www",
                "logout.php",
                defaultParams.Merge(parameters));
        }

        /// <summary>
        /// Get a Logout URL suitable for use with redirects.
        /// The parameters:
        ///     - next: the url to go to after a successful logout
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public override Uri GetLoginStatusUrl(IDictionary<string, object> parameters)
        {
            string currentUrl = this.CurrentUrl.ToString();

            var defaultParams = new Dictionary<string, object>();
            defaultParams["api_key"] = this.AppId;
            defaultParams["no_session"] = currentUrl;
            defaultParams["no_user"] = currentUrl;
            defaultParams["ok_session"] = currentUrl;
            defaultParams["session_version"] = 3;

            return this.GetUrl(
                "www",
                "extern/login_status.php",
                defaultParams.Merge(parameters));
        }

        /// <summary>
        /// Adds the standard REST requset parameters.
        /// </summary>
        /// <param name="parameters"></param>
        private void AddRestParameters(IDictionary<string, object> parameters)
        {
            parameters["api_key"] = this.AppId;
            parameters["format"] = "json-strings";
        }

        /// <summary>
        /// Gets the graph request url in the proper format.
        /// </summary>
        /// <param name="path">The request url path.</param>
        /// <returns>The fully qualified uri for the request.</returns>
        private Uri GetGraphRequestUri(string path)
        {
            if (!String.IsNullOrEmpty(path) && path.StartsWith("/", StringComparison.Ordinal))
            {
                path = path.Substring(1, path.Length - 1);
            }

            var uri = this.GetUrl("graph", path);
            return uri;
        }

        /// <summary>
        /// Builds the request post data and request uri based on the given parameters.
        /// </summary>
        /// <param name="uri">The request uri.</param>
        /// <param name="parameters">The request parameters.</param>
        /// <param name="httpMethod">The http method.</param>
        /// <param name="requestUrl">The outputed request uri.</param>
        /// <param name="contentType">The request content type.</param>
        /// <returns>The request post data.</returns>
        private byte[] BuildRequestData(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, out Uri requestUrl, out string contentType)
        {
            Contract.Requires(uri != null);
            Contract.Requires(parameters != null);

            if (!parameters.ContainsKey("access_token") && !String.IsNullOrEmpty(this.AccessToken))
            {
                parameters["access_token"] = this.AccessToken;
            }

            var requestUrlBuilder = new UriBuilder(uri);

            // Set the default content type
            contentType = "application/x-www-form-urlencoded";
            byte[] postData = null;
            string queryString = string.Empty;

            if (httpMethod == HttpMethod.Get)
            {
                queryString = parameters.ToJsonQueryString();
            }
            else
            {
                queryString = string.Concat("access_token=", parameters["access_token"]);
                parameters.Remove("access_token");

                var containsMediaObject = parameters.Where(p => p.Value is FacebookMediaObject).Count() > 0;
                if (containsMediaObject)
                {
                    string boundary = DateTime.Now.Ticks.ToString("x", CultureInfo.InvariantCulture);
                    postData = BuildMediaObjectPostData(parameters, boundary);
                    contentType = String.Concat("multipart/form-data; boundary=", boundary);
                }
                else
                {
                    postData = Encoding.UTF8.GetBytes(parameters.ToJsonQueryString());
                }
            }

            requestUrlBuilder.Query = queryString;
            requestUrl = requestUrlBuilder.Uri;

            return postData;
        }

        /// <summary>
        /// Builds the request post data if the request contains a media object
        /// such as an image or video to upload.
        /// </summary>
        /// <param name="parameters">The request parameters.</param>
        /// <param name="boundary">The multipart form request boundary.</param>
        /// <returns>The request post data.</returns>
        private static byte[] BuildMediaObjectPostData(IDictionary<string, object> parameters, string boundary)
        {
            FacebookMediaObject mediaObject = null;

            // Build up the post message header
            var sb = new StringBuilder();
            foreach (var kvp in parameters)
            {
                if (kvp.Value is FacebookMediaObject)
                {
                    // Check to make sure the file upload hasn't already been set.
                    if (mediaObject != null)
                    {
                        throw ExceptionFactory.CannotIncludeMultipleMediaObjects;
                    }
                    mediaObject = kvp.Value as FacebookMediaObject;
                }
                else
                {
                    sb.Append(_prefix).Append(boundary).Append(_newLine);
                    sb.Append("Content-Disposition: form-data; name=\"").Append(kvp.Key).Append("\"");
                    sb.Append(_newLine);
                    sb.Append(_newLine);
                    sb.Append(kvp.Value);
                    sb.Append(_newLine);
                }
            }

            Debug.Assert(mediaObject != null, "The mediaObject is null.");

            if (mediaObject.ContentType == null || mediaObject.GetValue() == null || mediaObject.FileName == null)
            {
                throw ExceptionFactory.MediaObjectMustHavePropertiesSet;
            }
            sb.Append(_prefix).Append(boundary).Append(_newLine);
            sb.Append("Content-Disposition: form-data; filename=\"").Append(mediaObject.FileName).Append("\"").Append(_newLine);
            sb.Append("Content-Type: ").Append(mediaObject.ContentType).Append(_newLine).Append(_newLine);

            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] fileData = mediaObject.GetValue();
            byte[] boundaryBytes = Encoding.UTF8.GetBytes(String.Concat(_newLine, _prefix, boundary, _prefix, _newLine));

            // Combine all bytes to post 
            var postData = new byte[postHeaderBytes.Length + fileData.Length + boundaryBytes.Length];
            Buffer.BlockCopy(postHeaderBytes, 0, postData, 0, postHeaderBytes.Length);
            Buffer.BlockCopy(fileData, 0, postData, postHeaderBytes.Length, fileData.Length);
            Buffer.BlockCopy(boundaryBytes, 0, postData, postHeaderBytes.Length + fileData.Length, boundaryBytes.Length);

            return postData;
        }

#if !SILVERLIGHT
        /// <summary>
        /// Make the API Request
        /// </summary>
        /// <param name="httpMethod">The http method to use. GET, POST, DELETE.</param>
        /// <param name="requestUrl">The uri of the request.</param>
        /// <param name="postData">The request data.</param>
        /// <param name="contentType">The request content type.</param>
        /// <returns>The decoded response object.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static object MakeRequest(HttpMethod httpMethod, Uri requestUrl, byte[] postData, string contentType)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = StringUtils.ConvertToString(httpMethod); // Set the http method GET, POST, etc.

            if (postData != null)
            {
                request.ContentLength = postData.Length;
                request.ContentType = contentType;
                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(postData, 0, postData.Length);
                    //dataStream.Close();
                }

            }

            object result = null;
            FacebookApiException exception = null;
            try
            {
                var responseData = "";
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    responseData = streamReader.ReadToEnd();
                }
                response.Close();

                result = JsonSerializer.DeserializeObject(responseData);
                exception = ExceptionFactory.GetRestException(result);
                if (exception != null)
                {
                    throw exception; // Thow the FacebookApiException
                }

            }
            catch (FacebookApiException)
            {
                // This is a rest api error thrown above, just rethrow
                throw;
            }
            catch (WebException ex)
            {
                // Graph API Errors or general web exceptions
                exception = ExceptionFactory.GetGraphException(ex);
                if (exception != null)
                {
                    throw exception;
                }
                throw;
            }
            return result;
        }

#endif
        /// <summary>
        /// Make the API Request
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="httpMethod">The http method to use. GET, POST, DELETE.</param>
        /// <param name="requestUrl">The uri of the request.</param>
        /// <param name="postData">The request data.</param>
        /// <param name="contentType">The request content type.</param>
        /// <exception cref="Facebook.FacebookApiException" />
        private static void MakeRequestAsync(FacebookAsyncCallback callback, object state, HttpMethod httpMethod, Uri requestUrl, byte[] postData, string contentType)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = StringUtils.ConvertToString(httpMethod); // Set the http method GET, POST, etc.
            if (httpMethod == HttpMethod.Post)
            {
                request.ContentType = contentType;
                request.BeginGetRequestStream((ar) => { RequestCallback(ar, postData, callback, state); }, request);
            }
            else
            {
                request.BeginGetResponse((ar) => { ResponseCallback(ar, callback, state); }, request);
            }
        }

        private static void RequestCallback(IAsyncResult asyncResult, byte[] postData, FacebookAsyncCallback callback, object state)
        {
            var request = (HttpWebRequest)asyncResult.AsyncState;
            using (Stream stream = request.EndGetRequestStream(asyncResult))
            {
                stream.Write(postData, 0, postData.Length);
            }
            request.BeginGetResponse((ar) => { ResponseCallback(ar, callback, state); }, request);
        }

        private static void ResponseCallback(IAsyncResult asyncResult, FacebookAsyncCallback callback, object state)
        {
            object result = null;
            FacebookApiException exception = null;
            try
            {
                var request = (HttpWebRequest)asyncResult.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(asyncResult);

                using (Stream responseStream = response.GetResponseStream())
                {
                    result = JsonSerializer.DeserializeObject(responseStream);
                }
            }
            catch (FacebookApiException) // Rest API Errors
            {
                throw;
            }
            catch (WebException ex) // Graph API Errors or general web exceptions
            {
                exception = ExceptionFactory.GetGraphException(ex);
                if (exception != null)
                {
                    throw exception; // Thow the FacebookApiException
                }
                throw;
            }
            finally
            {
                // Check to make sure there hasn't been an exception.
                // If there has, we want to pass null to the callback.
                object data = null;
                if (exception == null)
                {
                    data = result;
                }
#if SILVERLIGHT
                callback(new FacebookAsyncResult(data, state, null, asyncResult.CompletedSynchronously, asyncResult.IsCompleted, exception));
#else
                callback(new FacebookAsyncResult(data, state, asyncResult.AsyncWaitHandle, asyncResult.CompletedSynchronously, asyncResult.IsCompleted, exception));
#endif
            }
        }

#if !SILVERLIGHT
        /// <summary>
        /// Creates a facebook session from a signed request.
        /// </summary>
        /// <param name="signedRequest">The signed request.</param>
        /// <returns>The facebook session.</returns>
        private FacebookSession CreateSessionFromSignedRequest(FacebookSignedRequest signedRequest)
        {
            if (signedRequest == null || String.IsNullOrEmpty(signedRequest.AccessToken))
            {
                return null;
            }

            FacebookSession tempSession = new FacebookSession
            {
                UserId = signedRequest.UserId,
                AccessToken = signedRequest.AccessToken,
                Expires = signedRequest.Expires,
            };

            tempSession.Signature = this.GenerateSignature(tempSession);

            return tempSession;
        }

        /// <summary>
        /// Parses a signed request string.
        /// </summary>
        /// <param name="signedRequestValue">The encoded signed request value.</param>
        /// <returns>The valid signed request.</returns>
        protected FacebookSignedRequest ParseSignedRequest(string signedRequestValue)
        {
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(!signedRequestValue.Contains("."), "Invalid signed request.");

            string[] parts = signedRequestValue.Split('.');
            var sig = Base64UrlDecode(parts[0]);
            var payload = parts[1];

            using (var cryto = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(this.ApiSecret)))
            {
                var hash = Convert.ToBase64String(cryto.ComputeHash(Encoding.UTF8.GetBytes(payload)));
                var hashDecoded = Base64UrlDecode(hash);
                if (hashDecoded != sig)
                {
                    return null;
                }
            }

            var payloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(Base64UrlDecode(payload)));
            var data = (IDictionary<string, object>)JsonSerializer.DeserializeObject(payloadJson);
            var signedRequest = new FacebookSignedRequest();
            foreach (var keyValue in data)
            {
                signedRequest.Dictionary.Add(keyValue.Key, keyValue.Value.ToString());
            }
            return signedRequest;
        }

        private static string Base64UrlDecode(string encodedValue)
        {
            encodedValue = encodedValue.Replace('+', '-').Replace('/', '_').Trim();
            int pad = encodedValue.Length % 4;
            if (pad > 0)
            {
                pad = 4 - pad;
            }
            encodedValue = encodedValue.PadRight(encodedValue.Length + pad, '=');
            return encodedValue;
        }
#endif

        /// <summary>
        /// Parses a session value retrieved from a querystring.
        /// </summary>
        /// <param name="sessionValue">The session value.</param>
        /// <returns>The active session.</returns>
        protected static FacebookSession ParseFromQueryString(string sessionValue)
        {
            Contract.Requires(!String.IsNullOrEmpty(sessionValue));

            var session = new FacebookSession();
            // Parse the querystring format
            sessionValue = Uri.UnescapeDataString(sessionValue);
            if (!string.IsNullOrEmpty(sessionValue))
            {
                var result = JsonSerializer.DeserializeObject(sessionValue);
                if (result != null)
                {

                    var resultDict = (IDictionary<string, object>)result;
                    resultDict.ToDictionary(d => d.Key, d => d.Value != null ? d.Value.ToString() : string.Empty);
                    foreach (var key in resultDict.Keys)
                    {
                        session.Dictionary.Add(key, resultDict[key].ToString());
                    }
                }
            }
            return session;
        }

        protected static FacebookSession ParseFromCookie(string sessionValue)
        {
            Contract.Requires(!String.IsNullOrEmpty(sessionValue));
            Contract.Requires(!sessionValue.Contains(","), "Session value must not contain a comma.");

            var session = new FacebookSession();
            // Parse the cookie
            var parts = sessionValue.Replace("\"", string.Empty).Split('&');
            foreach (var part in parts)
            {
                if (!string.IsNullOrEmpty(part) && part.Contains("="))
                {
                    var nameValue = part.Split('=');
                    if (nameValue.Length == 2)
                    {
                        var s = Uri.UnescapeDataString(nameValue[1]);
                        session.Dictionary.Add(nameValue[0], s);
                    }
                }
            }
            return session;
        }

        /// <summary>
        /// This method invokes the supplied delegate with retry logic wrapped around it.  No values are returned.  If the delegate raises 
        /// recoverable Facebook server or client errors, then the supplied delegate is reinvoked after a certain amount of delay
        /// (which grows exponentially for each retry) until the retry limit is exceeded, at which point the exception
        /// is rethrown.  Other exceptions are not caught and will be visible to callers.
        /// </summary>
        /// <param name="body">The delegate to invoke within the retry code.</param>
        protected void WithMirrorRetry(Action body)
        {
            Contract.Requires(body != null);

            int retryCount = 0;

            while (true)
            {
                try
                {
                    body();
                    return;
                }
                catch (FacebookApiException ex)
                {
                    if (!RetryErrorTypes.Contains(ex.ErrorType))
                        throw;
                    else
                        if (retryCount >= _maxRetries) throw;
                }
                catch (WebException)
                {
                    if (retryCount >= _maxRetries) throw;
                }
                //Sleep for the retry delay before we retry again
                System.Threading.Thread.Sleep(_retryDelay);
                retryCount += 1;
            }
        }

        /// <summary>
        /// Similar to the above method, but a return value is allowed from the delegate.
        /// </summary>
        /// <typeparam name="TReturn">The type of object being returned</typeparam>
        /// <param name="body">The delegate to invoke within the retry logic which will produce the value to return</param>
        /// <returns>The value the delegate returns</returns>
        protected TReturn WithMirrorRetry<TReturn>(Func<TReturn> body)
        {
            Contract.Requires(body != null);

            int retryCount = 0;

            while (true)
            {
                try
                {
                    return body();
                }
                catch (FacebookApiException ex)
                {
                    if (!RetryErrorTypes.Contains(ex.ErrorType))
                        throw;
                    else
                        if (retryCount >= _maxRetries) throw;
                }
                catch (WebException)
                {
                    if (retryCount >= _maxRetries) throw;
                }
                //Sleep for the retry delay before we retry again
                System.Threading.Thread.Sleep(_retryDelay);
                retryCount += 1;
            }
        }

        /// <summary>
        /// Applies the Facebook settings to the 
        /// properties of this object.
        /// </summary>
        /// <param name="settings">The Facebook settings.</param>
        private void ApplySettings(IFacebookSettings settings)
        {
            Contract.Requires(settings != null);

            this.AppId = settings.AppId;
            this.ApiSecret = settings.ApiSecret;
            this.BaseDomain = settings.BaseDomain;
            this.CookieSupport = settings.CookieSupport;
            this._retryDelay = settings.RetryDelay == -1 ? this._retryDelay : settings.RetryDelay;
            this._maxRetries = settings.MaxRetries == -1 ? this._maxRetries : settings.MaxRetries;
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(_maxRetries >= 0);
            Contract.Invariant(_retryDelay >= 0);
            Contract.Invariant(_retryErrorTypes != null);
        }

    }
}
