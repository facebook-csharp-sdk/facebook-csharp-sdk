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
    using System.Diagnostics.Contracts;
    using System.Text;

    /// <summary>
    /// Represents the Facebook OAuth Helpers
    /// </summary>
    public class FacebookOAuthClient
    {
        /// <summary>
        /// The current web client.
        /// </summary>
        private IWebClient _webClient;

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
        /// The facebook application.
        /// </param>
        public FacebookOAuthClient(IFacebookApplication facebookApplication)
        {
            if (facebookApplication != null)
            {
                AppId = facebookApplication.AppId;
                AppSecret = facebookApplication.AppSecret;
            }
        }

        /// <summary>
        /// Gets or sets the app id.
        /// </summary>
        public virtual string AppId { get; set; }

        /// <summary>
        /// Gets or sets the app secret.
        /// </summary>
        public virtual string AppSecret { get; set; }

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
                Contract.Ensures(Contract.Result<Dictionary<string, Uri>>() != null);

                // return IsBeta ? FacebookUtils.DomainMapsBeta : FacebookUtils.DomainMaps;
                return FacebookUtils.DomainMaps;
            }
        }

        /// <summary>
        /// Gets or sets the web client.
        /// </summary>
        internal IWebClient WebClient
        {
            get { return _webClient ?? (_webClient = new WebClientWrapper()); }
            set { _webClient = value; }
        }

        #region Login/Logout url helpers

        /// <summary>
        /// Gets the login uri.
        /// </summary>
        /// <returns>
        /// Returns the facebook login uri.
        /// </returns>
        public virtual Uri GetLoginUrl()
        {
            return GetLoginUrl(null);
        }

        /// <summary>
        /// Gets the login uri.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the facebook login uri.
        /// </returns>
        /// <remarks>
        /// http://developers.facebook.com/docs/reference/dialogs/oauth
        /// Parameters that can be used:
        ///     client_id     : Your application's identifier. This is called client_id instead of app_id for this particular method to be compliant with the OAuth 2.0 specification. Required, but automatically specified by most SDKs.
        ///     redirect_uri  : The URL to redirect to after the user clicks a button on the dialog. Required, but automatically specified by most SDKs.
        ///     scope         : Optional. A comma-delimited list of permissions.
        ///     state         : Optional. An opaque string used to maintain application state between the request and callback. When Facebook redirects the user back to your redirect_uri, this value will be included unchanged in the response.
        ///     response_type : Optional, default is token. The requested response: an access token (token), an authorization code (code), or both (code_and_token).
        ///     display       : The display mode in which to render the dialog. The default is page on the www subdomain and wap on the m subdomain. This is automatically specified by most SDKs. (For WP7 builds it is set to touch.)
        /// </remarks>
        public virtual Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

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

            return new Uri(url);
        }

        /// <summary>
        /// Gets the logout url.
        /// </summary>
        /// <returns>
        /// Returns the logout url.
        /// </returns>
        public virtual Uri GetLogoutUrl()
        {
            return GetLogoutUrl(null);
        }

        /// <summary>
        /// Gets the logout url.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the logout url.
        /// </returns>
        public virtual Uri GetLogoutUrl(IDictionary<string, object> parameters)
        {
            // more information on this at http://stackoverflow.com/questions/2764436/facebook-oauth-logout
            var uriBuilder = new UriBuilder("http://m.facebook.com/logout.php");

            var defaultParams = new Dictionary<string, object>();
            defaultParams["confirm"] = 1;
            defaultParams["next"] = RedirectUri ?? new Uri("http://www.facebook.com");

            var mergedParameters = FacebookUtils.Merge(defaultParams, parameters);

            if (mergedParameters["next"] == null)
            {
                mergedParameters.Remove("next");
            }
            else
            {
                mergedParameters["next"] = mergedParameters["next"].ToString();
            }

            uriBuilder.Query = FacebookUtils.ToJsonQueryString(mergedParameters);

            return uriBuilder.Uri;
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
        /// <param name="logout">  
        /// Indicates whether to logout existing logged in user or not.  
        /// </param>  
        /// <param name="loginParameters">
        /// The login parameters.
        /// </param>
        /// <returns>
        /// The url to navigate.
        /// </returns>
        public static Uri GetLoginUrl(string appId, Uri redirectUri, string[] extendedPermissions, bool logout, IDictionary<string, object> loginParameters)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Ensures(Contract.Result<Uri>() != null);

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

            Uri navigateUrl;
            if (logout)
            {
                var logoutParameters = new Dictionary<string, object>
                                           {
                                               { "next", loginUrl }
                                           };

                navigateUrl = oauth.GetLogoutUrl(logoutParameters);
            }
            else
            {
                navigateUrl = loginUrl;
            }

            return navigateUrl;

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
            return GetLoginUrl(appId, redirectUri, extendedPermissions, false, loginParameters);
        }

        #endregion

        #region Application Access Token

        /// <summary>
        /// Event handler for application access token completion.
        /// </summary>
        public event EventHandler<FacebookApiEventArgs> GetApplicationAccessTokenCompleted;

#if !SILVERLIGHT

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
            {
                throw new ArgumentException("client_id required");
            }

            if (mergedParameters["client_secret"] == null || string.IsNullOrEmpty(mergedParameters["client_secret"].ToString()))
            {
                throw new ArgumentException("client_secret required");
            }

            if (mergedParameters["grant_type"] == null || string.IsNullOrEmpty(mergedParameters["grant_type"].ToString()))
            {
                throw new ArgumentException("grant_type required");
            }

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

        /// <summary>
        /// Event handler for application access token completion.
        /// </summary>
        public event EventHandler<FacebookApiEventArgs> ExchangeCodeForAccessTokenCompleted;

#if !SILVERLIGHT

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
                    {
                        ExchangeCodeForAccessTokenCompleted(this, e);
                    }
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
            {
                throw new Exception("ClientID required.");
            }

            if (mergedParameters["client_secret"] == null || string.IsNullOrEmpty(mergedParameters["client_secret"].ToString()))
            {
                throw new Exception("ClientSecret required");
            }

            if (mergedParameters["redirect_uri"] == null || string.IsNullOrEmpty(mergedParameters["redirect_uri"].ToString()))
            {
                throw new Exception("RedirectUri required");
            }

            if (mergedParameters["code"] == null || string.IsNullOrEmpty(mergedParameters["code"].ToString()))
            {
                throw new Exception("code required");
            }

            // seems like if we don't do this and rather pass the original uri object,
            // it seems to have http://localhost:80/csharpsamples instead of
            // http://localhost/csharpsamples
            // notice the port number, that shouldn't be there.
            // this seems to happen for iis hosted apps.
            mergedParameters["redirect_uri"] = mergedParameters["redirect_uri"].ToString();

            return mergedParameters;
        }

        private object BuildExchangeCodeForAccessTokenResult(string json)
        {
            var returnParameter = new JsonObject();
            FacebookUtils.ParseQueryParametersToDictionary("?" + json, returnParameter);

            // access_token=string&expires=long or access_token=string
            // Convert to JsonObject to support dynamic and be consistent with the rest of the library.
            var jsonObject = new JsonObject();
            jsonObject["access_token"] = returnParameter["access_token"];

            // check if expires exist coz for offline_access it is not present.
            if (returnParameter.ContainsKey("expires"))
            {
                jsonObject.Add("expires", Convert.ToInt64(returnParameter["expires"]));
            }

            return jsonObject;
        }

        #endregion

        #region Helper methods

#if !SILVERLIGHT
        internal protected virtual string OAuthRequest(string name, string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!String.IsNullOrEmpty(name));

            var mergedParameters = FacebookUtils.Merge(null, parameters);

            path = FacebookUtils.ParseQueryParametersToDictionary(path, mergedParameters);

            if (!string.IsNullOrEmpty(path) && path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(1, path.Length - 1);
            }

            var requestUrl = GetUrl(name, path, parameters);

            var webClient = WebClient;

            string responseString = null;
            try
            {
                var resultData = webClient.DownloadData(requestUrl);
                if (resultData != null)
                {
                    responseString = Encoding.UTF8.GetString(resultData);
                }
            }
            catch (WebExceptionWrapper ex)
            {
                responseString = FacebookUtils.GetResponseString(ex);
            }

            // todo: make sure the url is graph url then only check for graph exception
            var graphException = ExceptionFactory.GetGraphException(responseString);

            if (graphException != null)
            {
                throw graphException;
            }

            return responseString;
        }
#endif
        internal protected virtual void OAuthRequestAsync(string name, string path, IDictionary<string, object> parameters, object userToken, Func<string, string> processResponseString, Action<object, FacebookApiEventArgs> onDownloadComplete)
        {
            Contract.Requires(!String.IsNullOrEmpty(name));
            Contract.Requires(onDownloadComplete != null);

            var mergedParameters = FacebookUtils.Merge(null, parameters);

            path = FacebookUtils.ParseQueryParametersToDictionary(path, mergedParameters);

            if (!string.IsNullOrEmpty(path) && path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(1, path.Length - 1);
            }

            var requestUrl = GetUrl(name, path, parameters);

            var webClient = WebClient;

            var tempState = new WebClientStateContainer
            {
                UserState = userToken,
                Method = HttpMethod.Get,
                RequestUri = requestUrl
            };

            webClient.DownloadDataCompleted = (o, e) => DownloadDataCompleted(o, e, processResponseString, onDownloadComplete);

            webClient.DownloadDataAsync(requestUrl, tempState);
        }

        internal void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgsWrapper e, Func<string, string> processResponseString, Action<object, FacebookApiEventArgs> onDownloadComplete)
        {
            Contract.Requires(onDownloadComplete != null);

            // var userState = (WebClientStateContainer)e.UserState;

            string json = null;

            if (e.Error == null && e.Result != null)
            {
                json = Encoding.UTF8.GetString(e.Result, 0, e.Result.Length);

                if (processResponseString != null)
                {
                    // make the result json
                    json = processResponseString(json);
                }
            }

            HttpMethod method;
            var args = GetApiEventArgs(e, json, out method);

            /*
             * use onDownloadComplete instead, so don't need to do string compare
            
             
            if (userState.RequestUri.AbsolutePath.StartsWith("/oauth/access_token") && GetApplicationAccessTokenCompleted != null)
            {
                GetApplicationAccessTokenCompleted(this, args);
            }

             */

            onDownloadComplete(this, args);
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
            Contract.Requires(!String.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<Uri>() != default(Uri));

            return FacebookUtils.GetUrl(DomainMaps, name, path, parameters);
        }

        private FacebookApiEventArgs GetApiEventArgs(AsyncCompletedEventArgs e, string json, out HttpMethod httpMethod)
        {
            var state = (WebClientStateContainer)e.UserState;
            httpMethod = state.Method;

            var cancelled = e.Cancelled;
            var userState = state.UserState;
            var error = e.Error;

            // Check for Graph Exception
            var webException = error as WebExceptionWrapper;
            if (webException != null)
            {
                error = ExceptionFactory.GetGraphException(webException);
            }

            if (error == null)
            {
                error = ExceptionFactory.CheckForRestException(DomainMaps, state.RequestUri, json) ?? error;
            }

            var args = new FacebookApiEventArgs(error, cancelled, userState, json);
            return args;
        }

        #endregion
    }
}