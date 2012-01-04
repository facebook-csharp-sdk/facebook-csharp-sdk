// --------------------------------
// <copyright file="FacebookOAuthClient.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using FluentHttp;

    /// <summary>
    /// Represents the Facebook OAuth Client.
    /// </summary>
    public class FacebookOAuthClient
    {
        /// <summary>
        /// The Facebook AppId.
        /// </summary>
        private string _appId;

        /// <summary>
        /// The Facebook AppSecret.
        /// </summary>
        private string _appSecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthClient"/> class.
        /// </summary>
        public FacebookOAuthClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthClient"/> class.
        /// </summary>
        /// <param name="facebookApplication">
        /// The Facebook application.
        /// </param>
        public FacebookOAuthClient(IFacebookApplication facebookApplication)
        {
            if (facebookApplication != null)
            {
                _appId = facebookApplication.AppId;
                _appSecret = facebookApplication.AppSecret;
            }
        }

        /// <summary>
        /// Event handler for application access token completion.
        /// </summary>
        public event EventHandler<FacebookApiEventArgs> GetApplicationAccessTokenCompleted;

        /// <summary>
        /// Event handler for application access token completion.
        /// </summary>
        public event EventHandler<FacebookApiEventArgs> ExchangeCodeForAccessTokenCompleted;

        /// <summary>
        /// Gets or sets the app id.
        /// </summary>
        public virtual string AppId { get { return _appId; } set { _appId = value; } }

        /// <summary>
        /// Gets or sets the app secret.
        /// </summary>
        public virtual string AppSecret { get { return _appSecret; } set { _appSecret = value; } }

        /// <summary>
        /// Gets or sets the redirect uri.
        /// </summary>
        public virtual Uri RedirectUri { get; set; }

        /// <summary>
        /// Gets the aliases to Facebook domains.
        /// </summary>
        protected virtual Dictionary<string, Uri> DomainMaps
        {
            get
            {
                // return IsBeta ? FacebookUtils.DomainMapsBeta : FacebookUtils.DomainMaps;
                return FacebookUtils.DomainMaps;
            }
        }

        #region Login url helpers

        /// <summary>
        /// Gets the login url.
        /// </summary>
        /// <returns>
        /// Returns the Facebook login url.
        /// </returns>
        public virtual Uri GetLoginUrl()
        {
            return GetLoginUrl(null);
        }

        /// <summary>
        /// Gets the login url.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the Facebook login url.
        /// </returns>
        /// <remarks>
        /// http://developers.facebook.com/docs/reference/dialogs/oauth
        /// Parameters that can be used:
        ///     client_id     : Your application's identifier. This is called client_id instead of app_id for this particular method to be compliant with the OAuth 2.0 specification. Required, but automatically specified by most SDKs.
        ///     redirect_uri  : The URL to redirect to after the user clicks a button on the dialog. Required, but automatically specified by most SDKs.
        ///     scope         : Optional. A comma-delimited list of permissions.
        ///     state         : Optional. An opaque string used to maintain application state between the request and callback. When Facebook redirects the user back to your redirect_uri, this value will be included unchanged in the response.
        ///     response_type : Optional, default is token. The requested response: an access token (token), an authorization code (code), or both (code token).
        ///     display       : The display mode in which to render the dialog. The default is page on the www subdomain and wap on the m subdomain. This is automatically specified by most SDKs. (For WP7 builds it is set to touch.)
        /// </remarks>
        public virtual Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            var defaultParameters = new Dictionary<string, object>();
            defaultParameters["client_id"] = AppId;
            defaultParameters["redirect_uri"] = RedirectUri ?? new Uri("http://www.facebook.com/connect/login_success.html");
#if WINDOWS_PHONE
            defaultParameters["display"] = "touch";
#endif
            var mergedParameters = FacebookUtils.Merge(defaultParameters, parameters);

            // check if client_id and redirect_uri is not null or empty.
            if (mergedParameters["client_id"] == null || string.IsNullOrEmpty(mergedParameters["client_id"].ToString()))
            {
                throw new ArgumentException("client_id required.");
            }

            if (mergedParameters["redirect_uri"] == null || string.IsNullOrEmpty(mergedParameters["redirect_uri"].ToString()))
            {
                throw new ArgumentException("redirect_uri required.");
            }

            // seems like if we don't do this and rather pass the original uri object,
            // it seems to have http://localhost:80/csharpsamples instead of
            // http://localhost/csharpsamples
            // notice the port number, that shouldn't be there.
            // this seems to happen for iis hosted apps.
            mergedParameters["redirect_uri"] = mergedParameters["redirect_uri"].ToString();

            var url = "http://www.facebook.com/dialog/oauth/?" + FacebookUtils.ToJsonQueryString(mergedParameters);

            // In order to be compliant with the OAuth spec Facebook have made changes to their auth APIs.
            // As part of this update, they will be deprecating 'code_and_token' and need developers 
            // to use 'code%20token'. Everything is identical, just replace '_and_' with encoded
            // <space> '%20'.
            // url = url.Replace("response_type=code+token", "response_type=code%20token");

            return new Uri(url);
        }

        /// <summary>
        /// Gets the login url.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="redirectUri">
        /// The redirect Uri.
        /// </param>
        /// <returns>
        /// The url to navigate.
        /// </returns>
        public static Uri GetLoginUrl(string appId, Uri redirectUri)
        {
            return GetLoginUrl(appId, redirectUri, null, null);
        }

        /// <summary>
        /// Gets the login url.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="redirectUri">
        /// The redirect Uri.
        /// </param>
        /// <param name="extendedPermissions">
        /// The extended permissions (scope).
        /// </param>
        /// <returns>
        /// The url to navigate.
        /// </returns>
        public static Uri GetLoginUrl(string appId, Uri redirectUri, string[] extendedPermissions)
        {
            return GetLoginUrl(appId, redirectUri, extendedPermissions, null);
        }

        /// <summary>
        /// Gets the login url.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="redirectUri">
        /// The redirect Uri.
        /// </param>
        /// <param name="extendedPermissions">
        /// The extended permissions (scope).
        /// </param>
        /// <param name="loginParameters">
        /// The login parameters.
        /// </param>
        /// <returns>
        /// The url to navigate.
        /// </returns>
        public static Uri GetLoginUrl(string appId, Uri redirectUri, string[] extendedPermissions, IDictionary<string, object> loginParameters)
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException("appId");

            var oauth = new FacebookOAuthClient { AppId = appId, RedirectUri = redirectUri };

            var defaultLoginParameters = new Dictionary<string, object>
                                             {
                                                 { "response_type", "code" }, // make it "code" by default for security reasons.
#if WINDOWS_PHONE
                                                 { "display", "touch" }
#else
                                                 { "display", "popup" }
#endif
                                             };

            if (extendedPermissions != null && extendedPermissions.Length > 0)
            {
                defaultLoginParameters["scope"] = string.Join(",", extendedPermissions);
            }

            var mergedLoginParameters = FacebookUtils.Merge(defaultLoginParameters, loginParameters);

            var loginUrl = oauth.GetLoginUrl(mergedLoginParameters);
            return loginUrl;
        }

        #endregion

        #region Application Access Token

#if !(SILVERLIGHT || WINRT)
        /// <summary>
        /// Gets the application access token.
        /// </summary>
        /// <returns>
        /// The application access token.
        /// </returns>
        public virtual object GetApplicationAccessToken()
        {
            return GetApplicationAccessToken(null);
        }

        /// <summary>
        /// Gets the application access token.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The application access token.
        /// </returns>
        public virtual object GetApplicationAccessToken(IDictionary<string, object> parameters)
        {
            string name, path;

            var pars = BuildApplicationAccessTokenParameters(parameters, out name, out path);

            return BuildApplicationAccessTokenResult(OAuthRequest(name, path, pars));
        }

#endif

        /// <summary>
        /// Gets the application access token asynchronously.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        public virtual void GetApplicationAccessTokenAsync(IDictionary<string, object> parameters, object userToken)
        {
            string name, path;

            var pars = BuildApplicationAccessTokenParameters(parameters, out name, out path);

            OAuthRequestAsync(
               name, path, pars, userToken,
               json => BuildApplicationAccessTokenResult(json).ToString(),
               (o, e) =>
               {
                   if (GetApplicationAccessTokenCompleted != null)
                   {
                       GetApplicationAccessTokenCompleted(this, e);
                   }
               });
        }

        /// <summary>
        /// Gets the application access token asynchronously.
        /// </summary>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        public virtual void GetApplicationAccessTokenAsync(object userToken)
        {
            GetApplicationAccessTokenAsync(null, userToken);
        }

        /// <summary>
        /// Gets the application access token asynchronously.
        /// </summary>
        public virtual void GetApplicationAccessTokenAsync()
        {
            GetApplicationAccessTokenAsync(null, null);
        }

        private IDictionary<string, object> BuildApplicationAccessTokenParameters(IDictionary<string, object> parameters, out string name, out string path)
        {
            name = FacebookUtils.DOMAIN_MAP_GRAPH;
            path = "/oauth/access_token";

            var defaultParameters = new Dictionary<string, object>
                                        {
                                            { "client_id", AppId },
                                            { "client_secret", AppSecret },
                                            { "grant_type", "client_credentials" }
                                        };

            var mergedParameters = FacebookUtils.Merge(defaultParameters, parameters);

            if (mergedParameters["client_id"] == null || string.IsNullOrEmpty(mergedParameters["client_id"].ToString()))
                throw new ArgumentException("client_id required");

            if (mergedParameters["client_secret"] == null || string.IsNullOrEmpty(mergedParameters["client_secret"].ToString()))
                throw new ArgumentException("client_secret required");

            if (mergedParameters["grant_type"] == null || string.IsNullOrEmpty(mergedParameters["grant_type"].ToString()))
                throw new ArgumentException("grant_type required");

            return mergedParameters;
        }

        private object BuildApplicationAccessTokenResult(string responseString)
        {
            var returnParameter = new JsonObject();
            FacebookUtils.ParseQueryParametersToDictionary("?" + responseString, returnParameter);

            return returnParameter;
        }

        #endregion

        #region ExchangeCodeForAccessToken

#if !(SILVERLIGHT || WINRT)

        /// <summary>
        /// Exchange code for access token.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual object ExchangeCodeForAccessToken(string code, IDictionary<string, object> parameters)
        {
            string name, path;

            var defaultParameters = new Dictionary<string, object> { { "code", code } };
            var mergedParameters = FacebookUtils.Merge(defaultParameters, parameters);

            mergedParameters = BuildExchangeCodeForAccessTokenParameters(mergedParameters, out name, out path);

            return BuildExchangeCodeForAccessTokenResult(OAuthRequest(name, path, mergedParameters));
        }

        /// <summary>
        /// Exchange code for access token.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual object ExchangeCodeForAccessToken(string code)
        {
            return ExchangeCodeForAccessToken(code, null);
        }

#endif

        /// <summary>
        /// Exchange code for access token asynchronously.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        public virtual void ExchangeCodeForAccessTokenAsync(string code, IDictionary<string, object> parameters, object userToken)
        {
            string name, path;

            var defaultParameters = new Dictionary<string, object> { { "code", code } };
            var mergedParameters = FacebookUtils.Merge(defaultParameters, parameters);

            mergedParameters = BuildExchangeCodeForAccessTokenParameters(mergedParameters, out name, out path);

            OAuthRequestAsync(
                name, path, mergedParameters, userToken,
                json => BuildExchangeCodeForAccessTokenResult(json).ToString(),
                (o, e) =>
                {
                    if (ExchangeCodeForAccessTokenCompleted != null)
                        ExchangeCodeForAccessTokenCompleted(this, e);
                });
        }

        /// <summary>
        /// Exchange code for access token asynchronously.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void ExchangeCodeForAccessTokenAsync(string code, IDictionary<string, object> parameters)
        {
            ExchangeCodeForAccessTokenAsync(code, parameters, null);
        }

        /// <summary>
        /// Exchange code for access token asynchronously.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        public virtual void ExchangeCodeForAccessTokenAsync(string code)
        {
            ExchangeCodeForAccessTokenAsync(code, null, null);
        }

        private IDictionary<string, object> BuildExchangeCodeForAccessTokenParameters(IDictionary<string, object> parameters, out string name, out string path)
        {
            name = FacebookUtils.DOMAIN_MAP_GRAPH;
            path = "oauth/access_token";

            var pars = new Dictionary<string, object>();
            pars["client_id"] = AppId;
            pars["client_secret"] = AppSecret;
            pars["redirect_uri"] = RedirectUri ?? new Uri("http://www.facebook.com/connect/login_success.html");
            pars["code"] = null;

            var mergedParameters = FacebookUtils.Merge(pars, parameters);

            if (mergedParameters["client_id"] == null || string.IsNullOrEmpty(mergedParameters["client_id"].ToString()))
                throw new Exception("ClientID required.");

            if (mergedParameters["client_secret"] == null || string.IsNullOrEmpty(mergedParameters["client_secret"].ToString()))
                throw new Exception("ClientSecret required");

            if (mergedParameters["redirect_uri"] != null &&
                !string.IsNullOrEmpty(mergedParameters["redirect_uri"].ToString()))
            {
                // seems like if we don't do this and rather pass the original uri object,
                // it seems to have http://localhost:80/csharpsamples instead of
                // http://localhost/csharpsamples
                // notice the port number, that shouldn't be there.
                // this seems to happen for iis hosted apps.
                mergedParameters["redirect_uri"] = mergedParameters["redirect_uri"].ToString();
            }

            if (mergedParameters["code"] == null || string.IsNullOrEmpty(mergedParameters["code"].ToString()))
                throw new Exception("code required");

            return mergedParameters;
        }

        private object BuildExchangeCodeForAccessTokenResult(string json)
        {
            var returnParameter = new JsonObject();
            FacebookUtils.ParseQueryParametersToDictionary("?" + json, returnParameter);

            // access_token=string&expires=long or access_token=string
            // Convert to JsonObject to support dynamic and be consistent with the rest of the library.
            var jsonObject = new JsonObject();

            if (returnParameter.ContainsKey("access_token"))
            {
                jsonObject["access_token"] = returnParameter["access_token"];
            }

            // check if expires exist coz for offline_access it is not present.
            if (returnParameter.ContainsKey("expires"))
            {
                jsonObject.Add("expires", Convert.ToInt64(returnParameter["expires"]));
            }

            return jsonObject;
        }

        #endregion

#if !(SILVERLIGHT || WINRT)

        internal protected virtual string OAuthRequest(string name, string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            parameters = parameters ?? new Dictionary<string, object>();

            path = FacebookUtils.ParseQueryParametersToDictionary(path, parameters);

            if (!string.IsNullOrEmpty(path) && path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                path = path.Substring(1, path.Length - 1);

            var requestUrl = GetUrl(name, path, parameters);

            var httpWebRequest = CreateHttpWebRequest(requestUrl);
            httpWebRequest.Method = "GET";

            var httpHelper = new HttpHelper(httpWebRequest);
            Stream responseStream;

            try
            {
                responseStream = httpHelper.OpenRead();
            }
            catch (WebExceptionWrapper ex)
            {
                if (ex.GetResponse() == null)
                    throw;

                responseStream = httpHelper.OpenRead();
            }

            Exception exception;
            string responseString = ProcessResponse(httpHelper, responseStream, out exception);

            if (exception == null)
                return responseString;

            throw exception;
        }

#endif

        internal protected virtual void OAuthRequestAsync(string name, string path, IDictionary<string, object> parameters, object userToken, Func<string, string> processResponseString, Action<object, FacebookApiEventArgs> onDownloadComplete)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (onDownloadComplete == null)
                throw new ArgumentNullException("onDownloadComplete");

            parameters = parameters ?? new Dictionary<string, object>();

            path = FacebookUtils.ParseQueryParametersToDictionary(path, parameters);

            if (!string.IsNullOrEmpty(path) && path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                path = path.Substring(1, path.Length - 1);

            var requestUrl = GetUrl(name, path, parameters);

            var httpWebRequest = CreateHttpWebRequest(requestUrl);
            httpWebRequest.Method = "GET";

#if TPL
            if (HttpWebRequestWrapperCreated != null)
                HttpWebRequestWrapperCreated(this, new HttpWebRequestCreatedEventArgs(userToken, httpWebRequest));
#endif

            var httpHelper = new HttpHelper(httpWebRequest);

            httpHelper.OpenReadCompleted +=
                (o, e) =>
                {
                    FacebookApiEventArgs args = null;
                    if (e.Error == null)
                    {
                        Exception ex;
                        string responseString = ProcessResponse(httpHelper, e.Result, out ex);
                        args = new FacebookApiEventArgs(ex, e.Cancelled, userToken, processResponseString(responseString), false, false);
                    }
                    else
                    {
                        if (e.Error is WebExceptionWrapper)
                        {
                            var webEx = (WebExceptionWrapper)e.Error;
                            if (webEx.GetResponse() == null)
                            {
                                args = new FacebookApiEventArgs(webEx, false, userToken, null, false, false);
                            }
                            else
                            {
                                httpHelper.OpenReadAsync();
                                return;
                            }
                        }
                    }

                    onDownloadComplete(this, args);
                };

            httpHelper.OpenReadAsync();
        }

#if TPL

        /// <summary>
        /// Event handler when http web request wrapper is created for async api only.
        /// (used internally by TPL for cancellation support)
        /// </summary>
        private event EventHandler<HttpWebRequestCreatedEventArgs> HttpWebRequestWrapperCreated;

        /// <summary>
        /// Exchange code for access token asynchronously.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> ExchangeCodeForAccessTokenTaskAsync(string code, IDictionary<string, object> parameters, System.Threading.CancellationToken cancellationToken)
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<object>();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.TrySetCanceled();
            }
            else
            {
                HttpWebRequestWrapper httpWebRequest = null;
                EventHandler<HttpWebRequestCreatedEventArgs> httpWebRequestCreatedHandler = null;
                httpWebRequestCreatedHandler += (o, e) =>
                                                    {
                                                        if (e.UserState != tcs)
                                                            return;
                                                        httpWebRequest = e.HttpWebRequest;
                                                    };

                var ctr = cancellationToken.Register(() => { if (httpWebRequest != null) httpWebRequest.Abort(); });

                EventHandler<FacebookApiEventArgs> handler = null;
                handler = (sender, e) => FacebookUtils.TransferCompletionToTask(tcs, e, e.GetResultData,
                                                                                () =>
                                                                                {
                                                                                    if (ctr != null) ctr.Dispose();
                                                                                    ExchangeCodeForAccessTokenCompleted -= handler;
                                                                                    HttpWebRequestWrapperCreated -= httpWebRequestCreatedHandler;
                                                                                });
                ExchangeCodeForAccessTokenCompleted += handler;
                HttpWebRequestWrapperCreated += httpWebRequestCreatedHandler;

                try
                {
                    ExchangeCodeForAccessTokenAsync(code, parameters, tcs);
                    if (cancellationToken.IsCancellationRequested && httpWebRequest != null) httpWebRequest.Abort();
                }
                catch
                {
                    ExchangeCodeForAccessTokenCompleted -= handler;
                    HttpWebRequestWrapperCreated -= httpWebRequestCreatedHandler;
                    throw;
                }
            }

            return tcs.Task;
        }

        /// <summary>
        /// Exchange code for access token asynchronously.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> ExchangeCodeForAccessTokenTaskAsync(string code, IDictionary<string, object> parameters)
        {
            return ExchangeCodeForAccessTokenTaskAsync(code, parameters, System.Threading.CancellationToken.None);
        }

        /// <summary>
        /// Exchange code for access token asynchronously.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> ExchangeCodeForAccessTokenTaskAsync(string code)
        {
            return ExchangeCodeForAccessTokenTaskAsync(code, null);
        }

        /// <summary>
        /// Gets the application access token asynchronously.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The application access token.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> GetApplicationAccessTokenTaskAsync(IDictionary<string, object> parameters, System.Threading.CancellationToken cancellationToken)
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<object>();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.TrySetCanceled();
            }
            else
            {
                HttpWebRequestWrapper httpWebRequest = null;
                EventHandler<HttpWebRequestCreatedEventArgs> httpWebRequestCreatedHandler = null;
                httpWebRequestCreatedHandler += (o, e) =>
                {
                    if (e.UserState != tcs)
                        return;
                    httpWebRequest = e.HttpWebRequest;
                };

                var ctr = cancellationToken.Register(() => { if (httpWebRequest != null) httpWebRequest.Abort(); });

                EventHandler<FacebookApiEventArgs> handler = null;
                handler = (sender, e) => FacebookUtils.TransferCompletionToTask(tcs, e, e.GetResultData,
                                                                                () =>
                                                                                {
                                                                                    if (ctr != null) ctr.Dispose();
                                                                                    GetApplicationAccessTokenCompleted -= handler;
                                                                                    HttpWebRequestWrapperCreated -= httpWebRequestCreatedHandler;
                                                                                });
                GetApplicationAccessTokenCompleted += handler;
                HttpWebRequestWrapperCreated += httpWebRequestCreatedHandler;

                try
                {
                    GetApplicationAccessTokenAsync(parameters, tcs);
                    if (cancellationToken.IsCancellationRequested && httpWebRequest != null) httpWebRequest.Abort();
                }
                catch
                {
                    GetApplicationAccessTokenCompleted -= handler;
                    HttpWebRequestWrapperCreated -= httpWebRequestCreatedHandler;
                    throw;
                }
            }

            return tcs.Task;
        }

        /// <summary>
        /// Gets the application access token asynchronously.
        /// </summary>
        /// <returns>
        /// The application access token.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> GetApplicationAccessTokenTaskAsync()
        {
            return GetApplicationAccessTokenTaskAsync(null, System.Threading.CancellationToken.None);
        }

#endif
        private string ProcessResponse(HttpHelper httpHelper, Stream responseStream, out Exception exception)
        {
            exception = null;

            try
            {
                string responseString;
                using (var stream = responseStream)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        responseString = reader.ReadToEnd();
                    }
                }

                if (httpHelper.InnerException == null)
                    return responseString;

                object jsonObject;
                exception = ExceptionFactory.GetGraphException(responseString, out jsonObject) ?? httpHelper.InnerException;

                return null;
            }
            catch (Exception ex)
            {
                exception = ex;
                return null;
            }
        }

        /// <summary>
        /// Build the URL for given domain alias, path and parameters.
        /// </summary>
        /// <param name="name">
        /// The name of the domain (from the domain maps).
        /// </param>
        /// <param name="path">
        /// Optional path (without a leading slash)
        /// </param>
        /// <param name="parameters">
        ///  Optional query parameters
        /// </param>
        /// <returns>
        /// The string of the url for the given parameters.
        /// </returns>
        internal protected virtual Uri GetUrl(string name, string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return FacebookUtils.GetUrl(DomainMaps, name, path, parameters);
        }

        /// <summary>
        /// Creates the http web request.
        /// </summary>
        /// <param name="url">The url of the http web request.</param>
        /// <returns>The http helper.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual HttpWebRequestWrapper CreateHttpWebRequest(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            return new HttpWebRequestWrapper((HttpWebRequest)WebRequest.Create(url));
        }

        #region Parse OAuth Result

        /// <summary>
        /// Parse the uri to <see cref="FacebookOAuthResult"/>.
        /// </summary>
        /// <param name="uriString">
        /// The url to parse.
        /// </param>
        /// <returns>
        /// Returns an instance of <see cref="FacebookOAuthResult"/>.
        /// </returns>
        public virtual FacebookOAuthResult ParseResult(string uriString)
        {
            return FacebookOAuthResult.Parse(uriString);
        }

        /// <summary>
        /// Parse the uri to <see cref="FacebookOAuthResult"/>.
        /// </summary>
        /// <param name="uri">
        /// The url to parse.
        /// </param>
        /// <returns>
        /// Returns an instance of <see cref="FacebookOAuthResult"/>.
        /// </returns>
        public virtual FacebookOAuthResult ParseResult(Uri uri)
        {
            return FacebookOAuthResult.Parse(uri);
        }

        /// <summary>
        /// Try parsing the uri to <see cref="FacebookOAuthResult"/>.
        /// </summary>
        /// <param name="uriString">
        /// The url to parse.
        /// </param>
        /// <param name="result">
        /// An instance of <see cref="FacebookOAuthResult"/>.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public virtual bool TryParseResult(string uriString, out FacebookOAuthResult result)
        {
            return FacebookOAuthResult.TryParse(uriString, out result);
        }

        /// <summary>
        /// Try parsing the uri to <see cref="FacebookOAuthResult"/>.
        /// </summary>
        /// <param name="uri">
        /// The url to parse.
        /// </param>
        /// <param name="result">
        /// An instance of <see cref="FacebookOAuthResult"/>.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public virtual bool TryParseResult(Uri uri, out FacebookOAuthResult result)
        {
            return FacebookOAuthResult.TryParse(uri, out result);
        }

        #endregion
    }
}