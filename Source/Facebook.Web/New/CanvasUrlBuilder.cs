namespace Facebook.Web.New
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Facebook Canvas Url builder.
    /// </summary>
    public class CanvasUrlBuilder
    {
        private const string redirectPath = "facebookredirect.axd";

        /// <summary>
        /// The Facebook canvas settings
        /// </summary>
        private readonly ICanvasSettings canvasSettings;

        /// <summary>
        /// The http request.
        /// </summary>
        private readonly HttpRequestBase httpRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasUrlBuilder"/> class.
        /// </summary>
        /// <param name="canvasSettings">
        /// The canvas settings.
        /// </param>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        public CanvasUrlBuilder(ICanvasSettings canvasSettings, HttpRequestBase httpRequest)
        {
            Contract.Requires(canvasSettings != null);
            Contract.Requires(httpRequest != null);

            this.canvasSettings = canvasSettings;
            this.httpRequest = httpRequest;
        }

        /// <summary>
        /// Gets the base url of your application on Facebook.
        /// </summary>
        public Uri CanvasPage
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);
                return FacebookUtils.RemoveTrailingSlash(this.canvasSettings.CanvasPageUrl);
            }
        }

        /// <summary>
        /// The Facebook Application Path.
        /// </summary>
        public string CanvasPageApplicationPath
        {
            get
            {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

                return CanvasPage.AbsolutePath;
            }
        }

        /// <summary>
        /// Gets the URL where Facebook pull the content for your application's canvas pages.
        /// </summary>
        public Uri CanvasUrl
        {
            get
            {
                string url;
                if (this.canvasSettings.CanvasUrl != null)
                {
                    url = this.canvasSettings.CanvasUrl.ToString();
                }
                else if (this.httpRequest.Headers.AllKeys.Contains("Host"))
                {
                    // This will attempt to get the url based on the host
                    // in case we are behind a load balancer (such as with azure)
                    url = string.Concat(this.httpRequest.Url.Scheme, "://", this.httpRequest.Headers["Host"]);
                }
                else
                {
                    url = string.Concat(this.httpRequest.Url.Scheme, "://", this.httpRequest.Url.Host, ":", this.httpRequest.Url.Port);
                }

                return new Uri(FacebookUtils.RemoveTrailingSlash(url));
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
                var uriBuilder = new UriBuilder(this.CanvasUrl);
                var parts = this.httpRequest.RawUrl.Split('?');
                uriBuilder.Path = parts[0];
                if (parts.Length > 1)
                {
                    uriBuilder.Query = parts[1];
                }

                return FacebookUtils.RemoveTrailingSlash(uriBuilder.Uri);
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
                Contract.Ensures(Contract.Result<string>() != null);

                var pathAndQuery = this.httpRequest.Url.PathAndQuery;
                var appPath = this.CanvasUrl.AbsolutePath.Replace(this.CanvasPageApplicationPath, String.Empty);
                if (appPath != null && appPath != "/" && appPath.Length > 0)
                {
                    pathAndQuery = pathAndQuery.Replace(appPath, string.Empty);
                }

                return pathAndQuery ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the current url of the application on facebook.
        /// </summary>
        public Uri CurrentCanvasPage
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                return BuildCanvasPageUrl(CurrentCanvasPathAndQuery);
            }
        }

        /// <summary>
        /// Builds a Facebook canvas return URL.
        /// </summary>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <returns></returns>
        public Uri BuildCanvasPageUrl(string pathAndQuery)
        {
            Contract.Requires(!String.IsNullOrEmpty(pathAndQuery));
            Contract.Ensures(Contract.Result<Uri>() != null);

            if (!pathAndQuery.StartsWith("/", StringComparison.Ordinal))
            {
                pathAndQuery = String.Concat("/", pathAndQuery);
            }

            //if (this.CanvasUrl.PathAndQuery != "/" && pathAndQuery.StartsWith(this.CanvasUrl.PathAndQuery))
            //{
            //    pathAndQuery = pathAndQuery.Substring(this.CanvasUrl.PathAndQuery.Length);
            //}

            var url = string.Concat(CanvasPage, pathAndQuery);
            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return new Uri(FacebookUtils.RemoveTrailingSlash(url));
        }

        /// <summary>
        /// Gets the canvas login url
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the login url.
        /// </returns>
        public Uri GetLoginUrl(IFacebookSettings facebookSettings, string returnUrlPath, string state, IDictionary<string, object> parameters)
        {
            var oauth = new FacebookOAuthClientAuthorizer();
            if (facebookSettings != null)
            {
                oauth.ClientId = facebookSettings.AppId;
                oauth.ClientSecret = facebookSettings.AppSecret;
            }

            if (parameters != null && parameters.ContainsKey("state"))
            {
                // parameters state overried the state
                state = parameters["state"] == null ? null : parameters["state"].ToString();
            }

            var oauthJsonState = new JsonObject();

            // canvas path and query
            oauthJsonState["CurrentCanvasPathAndQuery"] = string.Concat(this.CanvasPageApplicationPath, this.CurrentCanvasPathAndQuery);

            // user state
            if (!string.IsNullOrEmpty(state))
            {
                oauthJsonState["state"] = state;
            }

            var oauthState = FacebookUtils.Base64UrlEncode(Encoding.UTF8.GetBytes(oauthJsonState.ToString()));
            var mergedParameters = FacebookUtils.Merge(parameters, null);
            mergedParameters["state"] = oauthState;

            var appPath = httpRequest.ApplicationPath;
            if (appPath != "/")
            {
                appPath = string.Concat(appPath, "/");
            }

            bool cancel = false;
            string redirectRoot = string.Concat(redirectPath, cancel ? "cancel" : string.Empty);

            var uriBuilder = new UriBuilder(this.CurrentCanvasUrl);
            uriBuilder.Path = string.Concat(appPath, redirectRoot);
            uriBuilder.Query = null; // no querystrings allowed.

            oauth.RedirectUri = uriBuilder.Uri;

            var loginUrl = oauth.GetLoginUri(mergedParameters);
            return loginUrl;
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
            Contract.Requires(url != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            return "<html><head><script type=\"text/javascript\">\ntop.location = \"" + url + "\";\n" +
                   "</script></head><body></body></html>";
        }

        /// <summary>
        /// Builds a Facebook canvas return URL.
        /// </summary>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <param name="cancel">if set to <c>true</c> [cancel].</param>
        /// <returns></returns>
        private Uri BuildAuthReturnUrl(string pathAndQuery, bool cancel)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            if (!string.IsNullOrEmpty(pathAndQuery) && pathAndQuery.StartsWith("/", StringComparison.Ordinal))
            {
                pathAndQuery = pathAndQuery.Substring(1);
            }

            if (pathAndQuery == null)
            {
                pathAndQuery = CurrentCanvasPathAndQuery;
            }

            string path;
            if (pathAndQuery.Contains('?'))
            {
                path = pathAndQuery.Split('?')[0];
            }
            else
            {
                path = pathAndQuery;
            }

            if (!path.StartsWith("/", StringComparison.Ordinal))
            {
                path = "/" + path;
            }

            var appPath = httpRequest.ApplicationPath;
            if (appPath != "/")
            {
                appPath = string.Concat(appPath, "/");
            }

            string redirectRoot = string.Concat(redirectPath, cancel ? "cancel" : string.Empty);

            var uriBuilder = new UriBuilder(this.CurrentCanvasUrl);
            uriBuilder.Path = string.Concat(appPath, redirectRoot, this.CanvasPageApplicationPath, path);
            uriBuilder.Query = null; // No Querystrings allowed in return urls

            return uriBuilder.Uri;
        }
    }
}