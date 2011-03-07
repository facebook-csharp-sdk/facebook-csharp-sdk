﻿// --------------------------------
// <copyright file="CanvasUrlBuilder.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
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
        /// <summary>
        /// Redirect path.
        /// </summary>
        private const string RedirectPath = "facebookredirect.axd";

        /// <summary>
        /// Facebook Application settings.
        /// </summary>
        private readonly IFacebookApplication settings;

        /// <summary>
        /// The http request.
        /// </summary>
        private readonly HttpRequestBase httpRequest;

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
            Contract.Requires(settings != null);
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Url != null);

            this.settings = settings;
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
                return FacebookUtils.RemoveTrailingSlash(new Uri(this.settings.CanvasPage));
            }
        }

        /// <summary>
        /// Gets the Facebook Application Path.
        /// </summary>
        public string CanvasPageApplicationPath
        {
            get
            {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

                return this.CanvasPage.AbsolutePath;
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
                if (this.settings.CanvasUrl != null)
                {
                    url = this.settings.CanvasUrl;
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
                Contract.Ensures(Contract.Result<Uri>() != null);

                return this.BuildCanvasPageUrl(this.CurrentCanvasPathAndQuery);
            }
        }

        /// <summary>
        /// Builds a Facebook canvas return URL.
        /// </summary>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <returns>
        /// The canvas page url.
        /// </returns>
        public Uri BuildCanvasPageUrl(string pathAndQuery)
        {
            Contract.Requires(!String.IsNullOrEmpty(pathAndQuery));
            Contract.Ensures(Contract.Result<Uri>() != null);

            if (!pathAndQuery.StartsWith("/", StringComparison.Ordinal))
            {
                pathAndQuery = String.Concat("/", pathAndQuery);
            }

            if (this.CanvasUrl.PathAndQuery != "/" && pathAndQuery.StartsWith(this.CanvasUrl.PathAndQuery))
            {
                pathAndQuery = pathAndQuery.Substring(this.CanvasUrl.PathAndQuery.Length);
            }

            var url = string.Concat(this.CanvasPage, pathAndQuery);
            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return new Uri(FacebookUtils.RemoveTrailingSlash(url));
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
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the login url.
        /// </returns>
        public Uri GetLoginUrl(string returnUrlPath, string cancelUrlPath, string state, IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            var oauth = new FacebookOAuthClient
                            {
                                AppId = this.settings.AppId,
                                AppSecret = this.settings.AppSecret
                            };

            if (parameters != null && parameters.ContainsKey("state"))
            {
                // parameters state override the state
                state = parameters["state"] == null ? null : parameters["state"].ToString();
            }

            var oauthJsonState = new JsonObject();

            // remove the http://apps.facebook.com/ length 25
            // make it one letter character so more info can fit in.
            // r -> return_url_path
            // c -> cancel_url_path
            // s -> user_state

            if (!string.IsNullOrEmpty(returnUrlPath))
            {
                // remove the starting /
                oauthJsonState["r"] = this.CanvasPageApplicationPath.Substring(1);

                // then return url path doesn't start with / add it
                if (!returnUrlPath.StartsWith("/"))
                {
                    oauthJsonState["r"] += "/";
                }

                oauthJsonState["r"] += returnUrlPath;
            }
            else
            {
                oauthJsonState["r"] = this.CurrentCanvasPage.ToString().Substring(25);
            }

            if (string.IsNullOrEmpty(cancelUrlPath))
            {
                // if cancel url path is empty, get settings from default facebook application.
                cancelUrlPath = FacebookContext.Current.CancelUrlPath;
            }

            if (!string.IsNullOrEmpty(cancelUrlPath))
            {
                if (IsRelativeUri(cancelUrlPath))
                {
                    // remove the first /
                    oauthJsonState["c"] = this.CanvasPageApplicationPath.Substring(1);

                    if (!cancelUrlPath.StartsWith("/"))
                    {
                        oauthJsonState["c"] += "/";
                    }
                }
                else
                {
                    oauthJsonState["c"] = string.Empty;
                }

                oauthJsonState["c"] += cancelUrlPath;
            }

            // user state
            if (!string.IsNullOrEmpty(state))
            {
                oauthJsonState["s"] = state;
            }

            var oauthState = FacebookUtils.Base64UrlEncode(Encoding.UTF8.GetBytes(oauthJsonState.ToString()));
            var mergedParameters = FacebookUtils.Merge(parameters, null);
            mergedParameters["state"] = oauthState;

            var appPath = this.httpRequest.ApplicationPath;
            if (appPath != "/")
            {
                appPath = string.Concat(appPath, "/");
            }

            string redirectRoot = RedirectPath;

            var uriBuilder = new UriBuilder(this.CurrentCanvasUrl)
                                 {
                                     Path = string.Concat(appPath, redirectRoot),
                                     Query = string.Empty
                                 };

            oauth.RedirectUri = uriBuilder.Uri;

            var loginUrl = oauth.GetLoginUrl(mergedParameters);
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

            return "<html><head><script type=\"text/javascript\">\ntop.location = \"" + url + "\";\n" + "</script></head><body></body></html>";
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

        [ContractInvariantMethod]
        private void InvariantObject()
        {
            Contract.Invariant(this.settings != null);
            Contract.Invariant(this.httpRequest != null);
            Contract.Invariant(this.httpRequest.Url != null);
        }
    }
}