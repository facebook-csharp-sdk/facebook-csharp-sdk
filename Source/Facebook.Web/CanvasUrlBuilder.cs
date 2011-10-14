// --------------------------------
// <copyright file="CanvasUrlBuilder.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Facebook Canvas Url builder.
    /// </summary>
    public class CanvasUrlBuilder
    {
        /// <summary>
        /// Redirect path.
        /// </summary>
        private const string RedirectPath = "facebookredirect.axd";

        /// <summary>
        /// Facebook Application settings.
        /// </summary>
        private readonly IFacebookApplication _settings;

        /// <summary>
        /// The http request.
        /// </summary>
        private readonly HttpRequestBase _httpRequest;

        /// <summary>
        /// Indicates whether the url is beta.
        /// </summary>
        private bool _useFacebookBeta;

        /// <summary>
        /// Indicates whether the url is secure.
        /// </summary>
        private bool _isSecureConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasUrlBuilder"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        public CanvasUrlBuilder(IFacebookApplication settings, HttpRequestBase httpRequest)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (httpRequest == null)
                throw new ArgumentNullException("httpRequest");
            if (httpRequest.Url == null)
                throw new ArgumentNullException("httpRequest.Url");

            _settings = settings;
            _httpRequest = httpRequest;

            // cache it for performance improvements
            _useFacebookBeta = IsBeta(_httpRequest.UrlReferrer);
            _isSecureConnection = IsSecureUrl(_httpRequest.Url);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasUrlBuilder"/> class.
        /// </summary>
        public CanvasUrlBuilder()
            : this(FacebookApplication.Current, new HttpRequestWrapper(HttpContext.Current.Request))
        {
            if (FacebookApplication.Current == null)
                throw new Exception("FacebookApplication.Current is null.");
            if (HttpContext.Current == null)
                throw new Exception("HttpContext.Current is null.");
        }

        /// <summary>
        /// Gets the aliases to Facebook domains.
        /// </summary>
        protected virtual Dictionary<string, Uri> DomainMaps
        {
            get
            {
                return IsSecureConnection
                           ? (UseFacebookBeta ? FacebookUtils.DomainMapsBetaSecure : FacebookUtils.DomainMapsSecure)
                           : (UseFacebookBeta ? FacebookUtils.DomainMapsBeta : FacebookUtils.DomainMaps);
            }
        }

        /// <summary>
        /// Gets the base url of your application on Facebook.
        /// </summary>
        public Uri CanvasPage
        {
            get
            {
                return new Uri(DomainMaps[FacebookUtils.DOMAIN_MAP_APPS] + CanvasPageApplicationPath.Substring(1));
            }
        }

        /// <summary>
        /// Gets the Facebook Application Path.
        /// </summary>
        public string CanvasPageApplicationPath
        {
            get
            {
                return FacebookWebUtils.RemoveTrailingSlash(new Uri(_settings.CanvasPage)).AbsolutePath;
            }
        }

        /// <summary>
        /// Gets the URL where Facebook pulls the content for your application's canvas pages.
        /// </summary>
        public Uri CanvasUrl
        {
            get
            {
                string url;
                if (_settings.CanvasUrl != null)
                {
                    url = _settings.CanvasUrl;
                }
                else if (_httpRequest.Headers.AllKeys.Contains("Host"))
                {
                    // This will attempt to get the url based on the host
                    // in case we are behind a load balancer (such as with azure)
                    url = string.Concat(_httpRequest.Url.Scheme, "://", _httpRequest.Headers["Host"]);
                }
                else
                {
                    url = string.Concat(_httpRequest.Url.Scheme, "://", _httpRequest.Url.Host, ":", _httpRequest.Url.Port);
                }

                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("CanvasUrl is null or empty");
                }

                return new Uri(FacebookWebUtils.RemoveTrailingSlash(url));
            }
        }

        /// <summary>
        /// Gets the secure URL where Facebook pulls the content for your application's cavas pages.
        /// </summary>
        public Uri SecureCanvasUrl
        {
            get
            {
                string url;
                if (_settings.SecureCanvasUrl != null)
                {
                    url = _settings.SecureCanvasUrl;
                }
                else if (_httpRequest.Headers.AllKeys.Contains("Host"))
                {
                    // This will attempt to get the url based on the host
                    // in case we are behind a load balancer (such as with azure)
                    url = string.Concat(_httpRequest.Url.Scheme, "://", _httpRequest.Headers["Host"]);
                }
                else
                {
                    url = string.Concat(_httpRequest.Url.Scheme, "://", _httpRequest.Url.Host, ":", _httpRequest.Url.Port);
                }

                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("SecureCanvasUrl is null or empty");
                }

                return new Uri(FacebookWebUtils.RemoveTrailingSlash(url));
            }
        }

        /// <summary>
        /// Gets the current URL of your application that Facebook
        /// is pulling..
        /// </summary>
        /// <value>The current canvas URL.</value>
        public Uri CurrentCanvasUrl
        {
            get
            {
                var uriBuilder = new UriBuilder(IsSecureConnection ? SecureCanvasUrl : CanvasUrl);
                var parts = _httpRequest.RawUrl.Split('?');
                uriBuilder.Path = parts[0];
                if (parts.Length > 1)
                {
                    uriBuilder.Query = parts[1];
                }

                return FacebookWebUtils.RemoveTrailingSlash(uriBuilder.Uri);
            }
        }

        /// <summary>
        /// Gets the current Path and query of the application 
        /// being pulled by Facebook.
        /// </summary>
        public string CurrentCanvasPathAndQuery
        {
            get
            {
                var pathAndQuery = _httpRequest.Url.PathAndQuery;
                var i = pathAndQuery.IndexOf("/");
                if (i > 0)
                {
                    pathAndQuery = pathAndQuery.Substring(i);
                }
                return pathAndQuery;
            }
        }

        /// <summary>
        /// Gets the current url of the application on facebook.
        /// </summary>
        public Uri CurrentCanvasPage
        {
            get
            {
                return BuildCanvasPageUrl(CurrentCanvasPathAndQuery);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the connection is secure.
        /// </summary>
        public bool IsSecureConnection
        {
            get { return _isSecureConnection; }
            set { _isSecureConnection = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use Facebook beta.
        /// </summary>
        public bool UseFacebookBeta
        {
            get { return _useFacebookBeta; }
            set { _useFacebookBeta = value; }
        }

        /// <summary>
        /// Builds a Facebook canvas page return URL.
        /// </summary>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <returns>
        /// The canvas page url.
        /// </returns>
        public Uri BuildCanvasPageUrl(string pathAndQuery)
        {
            if (string.IsNullOrEmpty(pathAndQuery))
                throw new ArgumentNullException("pathAndQuery");

            if (!pathAndQuery.StartsWith("/", StringComparison.Ordinal))
            {
                pathAndQuery = String.Concat("/", pathAndQuery);
            }

            var canvasUrl = IsSecureConnection ? SecureCanvasUrl : CanvasUrl;
            if (canvasUrl.PathAndQuery != "/" && pathAndQuery.StartsWith(canvasUrl.PathAndQuery))
            {
                pathAndQuery = pathAndQuery.Substring(canvasUrl.PathAndQuery.Length);
            }

            var url = string.Concat(CanvasPage, pathAndQuery);

            return new Uri(FacebookWebUtils.RemoveTrailingSlash(url));
        }

        /// <summary>
        /// Builds a Facebook canvas return URL.
        /// </summary>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <returns>
        /// The canvas url.
        /// </returns>
        public Uri BuildCanvasUrl(string pathAndQuery)
        {
            if (string.IsNullOrEmpty(pathAndQuery))
                throw new ArgumentNullException("pathAndQuery");

            pathAndQuery = FacebookWebUtils.RemoveStartingSlash(pathAndQuery);

            var canvasUrl = IsSecureConnection ? SecureCanvasUrl : CanvasUrl;
            if (canvasUrl.PathAndQuery != "/" && pathAndQuery.StartsWith(canvasUrl.PathAndQuery))
            {
                pathAndQuery = pathAndQuery.Substring(canvasUrl.PathAndQuery.Length);
            }

            var url = string.Concat(canvasUrl, pathAndQuery);

            return new Uri(FacebookWebUtils.RemoveTrailingSlash(url));
        }

        /// <summary>
        /// Gets the canvas login url
        /// </summary>
        /// <param name="returnUrlPath">
        /// The return Url Path.
        /// </param>
        /// <param name="cancelUrlPath">
        /// The cancel Url Path.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="loginParameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the login url.
        /// </returns>
        public Uri GetLoginUrl(string returnUrlPath, string cancelUrlPath, string state, IDictionary<string, object> loginParameters)
        {
            var oauth = new FacebookOAuthClient
            {
                AppId = _settings.AppId
            };

            var oauthJsonState = PrepareCanvasLoginUrlOAuthState(returnUrlPath, cancelUrlPath, state, loginParameters);

            var oauthState = FacebookWebUtils.Base64UrlEncode(Encoding.UTF8.GetBytes(oauthJsonState.ToString()));
            var mergedLoginParameters = FacebookUtils.Merge(loginParameters, null);
            mergedLoginParameters["state"] = oauthState;

            var appPath = _httpRequest.ApplicationPath;
            if (appPath != "/")
            {
                appPath = string.Concat(appPath, "/");
            }

            string redirectRoot = RedirectPath;

            var uriBuilder = new UriBuilder(CurrentCanvasUrl)
            {
                Path = string.Concat(appPath, redirectRoot),
                Query = string.Empty
            };

            oauth.RedirectUri = uriBuilder.Uri;

            var loginUrl = oauth.GetLoginUrl(mergedLoginParameters);
            return loginUrl;
        }

        internal IDictionary<string, object> PrepareCanvasLoginUrlOAuthState(string returnUrlPath, string cancelUrlPath, string state, IDictionary<string, object> loginParameters)
        {
            var oauthJsonState = new JsonObject();

            // make it one letter character so more info can fit in.
            // r -> return_url_path
            // c -> cancel_url_path
            // s -> user_state

            var mergedParameters = FacebookUtils.Merge(null, loginParameters);

            if (mergedParameters.ContainsKey("state"))
            {
                // override the user state if present in the parameters.
                state = mergedParameters["state"] == null ? null : mergedParameters["state"].ToString();
            }

            if (!string.IsNullOrEmpty(state))
            {
                oauthJsonState["s"] = state;
            }

            if (string.IsNullOrEmpty(returnUrlPath))
            {
                oauthJsonState["r"] = CurrentCanvasPage.ToString();
            }
            else
            {
                if (IsRelativeUri(returnUrlPath))
                {
                    oauthJsonState["r"] = BuildCanvasPageUrl(returnUrlPath).ToString();
                }
                else
                {
                    oauthJsonState["r"] = returnUrlPath;
                }
            }

            if (string.IsNullOrEmpty(cancelUrlPath))
            {
                // if cancel url path is empty, get settings from facebook application.
                cancelUrlPath = _settings.CancelUrlPath;
            }

            if (!string.IsNullOrEmpty(cancelUrlPath))
            {
                if (IsRelativeUri(cancelUrlPath))
                {
                    oauthJsonState["c"] = BuildCanvasPageUrl(cancelUrlPath).ToString();
                }
                else
                {
                    oauthJsonState["c"] = cancelUrlPath;
                }
            }

            return oauthJsonState;
        }

        /// <summary>
        /// Gets the canvas redirect HTML.
        /// </summary>
        /// <param name="url">The redirect url.</param>
        /// <returns>
        /// Returns redirect html.
        /// </returns>
        public static string GetCanvasRedirectHtml(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            return string.Concat("<html><head><script type=\"text/javascript\">\ntop.location = \"", url, "\";\n", "</script></head><body></body></html>");
        }

        /// <summary>
        /// Checks if the specified input string is a valid relative uri.
        /// </summary>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// Returns true if the input string is a valid uri.
        /// </returns>
        internal static bool IsRelativeUri(string str)
        {
            if (!string.IsNullOrEmpty(str) && Uri.IsWellFormedUriString(str, UriKind.Relative))
            {
                Uri tempValue;
                return Uri.TryCreate(str, UriKind.Relative, out tempValue);
            }

            return false;
        }

        /// <summary>
        /// Checks if the url is a secure url.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// Returns true if the url is secure.
        /// </returns>
        internal static bool IsSecureUrl(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            return url.Scheme == "https";
        }

        /// <summary>
        /// Checks if the url is beta.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <returns>
        /// Returns true if the url is beta.
        /// </returns>
        internal bool IsBeta(Uri uri)
        {
            if (uri == null)
            {
                return _settings.UseFacebookBeta;
            }

            return uri.Host == "apps.beta.facebook.com";
        }
    }
}