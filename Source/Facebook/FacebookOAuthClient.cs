// --------------------------------
// <copyright file="FacebookOAuthClient.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Net;

    /// <summary>
    /// Represents the Facebook OAuth Helpers
    /// </summary>
    public class FacebookOAuthClient
    {
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
                this.ClientId = facebookApplication.AppId;
                this.ClientSecret = facebookApplication.AppSecret;
            }
        }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the redirect uri.
        /// </summary>
        public Uri RedirectUri { get; set; }

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
        public Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            var defaultParameters = new Dictionary<string, object>();
            defaultParameters["client_id"] = this.ClientId;
            defaultParameters["redirect_uri"] = this.RedirectUri ?? new Uri("http://www.facebook.com/connect/login_success.html");
#if WINDOWS_PHONE
            defaultParameters["display"] = "touch";
#endif
            var mergedParameters = FacebookUtils.Merge(defaultParameters, parameters);

            // check if client_id and redirect_uri is not null or empty.
            if (mergedParameters["client_id"] == null || string.IsNullOrEmpty(mergedParameters["client_id"].ToString()))
            {
                throw new InvalidOperationException("client_id required.");
            }

            if (mergedParameters["redirect_uri"] == null || string.IsNullOrEmpty(mergedParameters["redirect_uri"].ToString()))
            {
                throw new InvalidOperationException("redirect_uri required.");
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
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the logout url.
        /// </returns>
        public Uri GetLogoutUrl(IDictionary<string, object> parameters)
        {
            // more information on this at http://stackoverflow.com/questions/2764436/facebook-oauth-logout
            var uriBuilder = new UriBuilder("http://m.facebook.com/logout.php");

            var defaultParams = new Dictionary<string, object>();
            defaultParams["confirm"] = 1;
            defaultParams["next"] = this.RedirectUri ?? new Uri("http://www.facebook.com");

            var mergedParameters = FacebookUtils.Merge(defaultParams, parameters);

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

            var oauth = new FacebookOAuthClient { ClientId = appId, RedirectUri = redirectUri };

            var defaultLoginParameters = new Dictionary<string, object>
                                             {
                                                 { "response_type", "code" }, // make it "code" by default for security reasons.
                                                 { "display", "popup" }
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

#if !SILVERLIGHT
        // Silverlight should have only async calls

        /// <summary>
        /// Gets the access token by exchanging the code.
        /// </summary>
        /// <param name="code">
        /// The code to exchange.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the access token or expires if exists.
        /// </returns>
        public object ExchangeCodeForAccessToken(string code, IDictionary<string, object> parameters)
        {
            Contract.Requires(!string.IsNullOrEmpty(code));

            var uri = BuildExchangeCodeForAccessTokenUrl(code, parameters);

            string json;
            using (var webClient = new WebClient())
            {
                try
                {
                    json = webClient.DownloadString(uri);
                }
                catch (WebException ex)
                {
                    // Graph API Errors or general web exceptions
                    var exception = ExceptionFactory.GetGraphException(ex);
                    if (exception != null)
                    {
                        throw exception;
                    }

                    throw;
                }
            }

            return BuildExchangeCodeResult(json);
        }
#endif

        /// <summary>
        /// Gets the access token by exchanging the code.
        /// </summary>
        /// <param name="code">
        /// The code to exchange.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public void ExchangeCodeForAccessTokenAsync(string code, IDictionary<string, object> parameters)
        {
            Contract.Requires(!string.IsNullOrEmpty(code));

            var requestUri = this.BuildExchangeCodeForAccessTokenUrl(code, parameters);

            using (var webClient = new WebClient())
            {
                webClient.DownloadStringCompleted +=
                    (o, e) => this.OnExchangeCodeForAccessTokenCompleted(e);
                webClient.DownloadStringAsync(requestUri, null);
            }
        }

        private void OnExchangeCodeForAccessTokenCompleted(DownloadStringCompletedEventArgs e)
        {
            var args = this.GetApiEventArgs(e, e.Error == null ? this.BuildExchangeCodeResult(e.Result).ToString() : null);

            this.OnExchangeCodeForAccessTokenCompleted(args);
        }

        public event EventHandler<FacebookApiEventArgs> ExchangeCodeForAccessTokenCompleted;

        protected void OnExchangeCodeForAccessTokenCompleted(FacebookApiEventArgs e)
        {
            if (this.ExchangeCodeForAccessTokenCompleted != null)
            {
                this.ExchangeCodeForAccessTokenCompleted(this, e);
            }
        }

#if !SILVERLIGHT

        /// <summary>
        /// Gets the application access token.
        /// </summary>
        /// <returns>
        /// The application access token.
        /// </returns>
        public object GetApplicationAccessToken()
        {
            var requestUri = BuildGetApplicationAccessTokenUrl();

            string responseData;
            using (var webClient = new WebClient())
            {
                try
                {
                    responseData = webClient.DownloadString(requestUri);
                }
                catch (WebException ex)
                {
                    // Graph API Errors or general web exceptions
                    var exception = ExceptionFactory.GetGraphException(ex);
                    if (exception != null)
                    {
                        throw exception;
                    }

                    throw;
                }
            }

            var returnParameter = new JsonObject();
            FacebookClient.ParseQueryParametersToDictionary("?" + responseData, returnParameter);

            return returnParameter;
        }

#endif

        /// <summary>
        /// Get the application access token asynchronously.
        /// </summary>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <example>
        /// <code>
        ///  var oauth = new FacebookOAuthClient { ClientId = "{appid}", ClientSecret = "{appsecret}" };
        ///  oauth.GetApplicationAccessTokenAsync(
        ///      ar =>
        ///      {
        ///          dynamic result = ar.Result;
        ///          Console.WriteLine(result.access_token);
        ///      }, null);
        /// </code>
        /// </example>
        public void GetApplicationAccessTokenAsync()
        {
            var requestUri = BuildGetApplicationAccessTokenUrl();

            using (var webClient = new WebClient())
            {
                webClient.DownloadStringCompleted +=
                    (o, e) => this.OnGetApplicationAccessTokenCompleted(e);

                webClient.DownloadStringAsync(requestUri, null);
            }
        }

        private void OnGetApplicationAccessTokenCompleted(DownloadStringCompletedEventArgs e)
        {
            FacebookApiEventArgs args;
            if (e.Error == null)
            {
                var json = new JsonObject();
                FacebookClient.ParseQueryParametersToDictionary("?" + e.Result, json);
                args = this.GetApiEventArgs(e, json.ToString());
            }
            else
            {
                args = this.GetApiEventArgs(e, null);
            }
            this.OnGetApplicationAccessTokenCompleted(args);
        }

        public event EventHandler<FacebookApiEventArgs> GetApplicationAccessTokenCompleted;

        protected void OnGetApplicationAccessTokenCompleted(FacebookApiEventArgs args)
        {
            if (GetApplicationAccessTokenCompleted != null)
            {
                GetApplicationAccessTokenCompleted(this, args);
            }
        }

        private Uri BuildGetApplicationAccessTokenUrl()
        {
            if (string.IsNullOrEmpty(this.ClientId))
            {
                throw new Exception("ClientID required.");
            }

            if (string.IsNullOrEmpty(this.ClientSecret))
            {
                throw new Exception("ClientSecret required");
            }

            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = this.ClientId;
            parameters["client_secret"] = this.ClientSecret;
            parameters["grant_type"] = "client_credentials";

            var queryString = FacebookUtils.ToJsonQueryString(parameters);

            var uriBuilder = new UriBuilder("https://graph.facebook.com/oauth/access_token") { Query = queryString };
            return uriBuilder.Uri;
        }

        private Uri BuildExchangeCodeForAccessTokenUrl(string code, IDictionary<string, object> parameters)
        {
            var pars = new Dictionary<string, object>();
            pars["client_id"] = this.ClientId;
            pars["client_secret"] = this.ClientSecret;
            pars["redirect_uri"] = this.RedirectUri ?? new Uri("http://www.facebook.com/connect/login_success.html");
            pars["code"] = code;

            var mergedParameters = FacebookUtils.Merge(pars, parameters);

            if (pars["client_id"] == null || string.IsNullOrEmpty(pars["client_id"].ToString()))
            {
                throw new Exception("ClientID required.");
            }

            if (pars["client_secret"] == null || string.IsNullOrEmpty(pars["client_secret"].ToString()))
            {
                throw new Exception("ClientSecret required");
            }

            if (pars["redirect_uri"] == null || string.IsNullOrEmpty(pars["redirect_uri"].ToString()))
            {
                throw new Exception("RedirectUri required");
            }

            // seems like if we don't do this and rather pass the original uri object,
            // it seems to have http://localhost:80/csharpsamples instead of
            // http://localhost/csharpsamples
            // notice the port number, that shouldn't be there.
            // this seems to happen for iis hosted apps.
            mergedParameters["redirect_uri"] = mergedParameters["redirect_uri"].ToString();

            var queryString = FacebookUtils.ToJsonQueryString(mergedParameters);

            var uriBuilder = new UriBuilder("https://graph.facebook.com/oauth/access_token") { Query = queryString.ToString() };
            return uriBuilder.Uri;
        }

        private object BuildExchangeCodeResult(string json)
        {
            var returnParameter = new JsonObject();
            FacebookClient.ParseQueryParametersToDictionary("?" + json, returnParameter);

            // access_token=string&expires=long or access_token=string
            // Convert to JsonObject to support dynamic and be consistent with the rest of the library.
            var jsonObject = new JsonObject();
            jsonObject["access_token"] = (string)returnParameter["access_token"];

            // check if expires exist coz for offline_access it is not present.
            if (returnParameter.ContainsKey("expires"))
            {
                jsonObject.Add("expires", Convert.ToInt64(returnParameter["expires"]));
            }

            return jsonObject;
        }

        private FacebookApiEventArgs GetApiEventArgs(AsyncCompletedEventArgs e, string json)
        {
            var cancelled = e.Cancelled;
            var userState = e.UserState;
            var error = e.Error;

            // Check for Graph Exception
            var webException = error as WebException;

            if (webException != null)
            {
                error = ExceptionFactory.GetGraphException(webException);
            }

            return new FacebookApiEventArgs(error, cancelled, userState, json);
        }
    }
}