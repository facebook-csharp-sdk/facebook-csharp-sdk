// --------------------------------
// <copyright file="CanvasUrlHelper.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace Facebook.Web
{
    /// <summary>
    /// Provides a tool for building and retreiving Facebook canvas uniform resource identifiers (URIs).
    /// </summary>
    public class CanvasUrlBuilder
    {
        private const string redirectPath = "facebookredirect.axd";
        private HttpRequestBase request;
        private ICanvasSettings canvasSettings;


        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasUrlBuilder"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public CanvasUrlBuilder(HttpRequestBase request)
            : this(request, CanvasSettings.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasUrlBuilder"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="canvasSettings">The canvas settings.</param>
        public CanvasUrlBuilder(HttpRequestBase request, ICanvasSettings canvasSettings)
        {
            Contract.Requires(request != null);
            Contract.Requires(request.Url != null);
            Contract.Requires(request.Headers != null);
            Contract.Requires(canvasSettings != null);

            this.request = request;
            this.canvasSettings = canvasSettings;
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(request != null);
            Contract.Invariant(request.Headers != null);
            Contract.Invariant(request.Url != null);
            Contract.Invariant(canvasSettings != null);
        }

        /// <summary>
        /// Gets the canvas redirect HTML.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string GetCanvasRedirectHtml(Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            return GetCanvasRedirectHtml(url.ToString());
        }

        /// <summary>
        /// Gets the canvas redirect HTML.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
        public static string GetCanvasRedirectHtml(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            return "<html><head>" +
                   "<script type=\"text/javascript\">\n" +
                    "top.location = \"" + url + "\";\n" +
                    "</script>" +
                   "</head><body></body></html>";
        }

        /// <summary>
        /// Gets the URL where Facebook pulls the content 
        /// for your application's canvas pages.
        /// </summary>
        /// <value>The canvas URL.</value>
        public Uri CanvasUrl
        {
            get
            {
                string url;
                if (this.canvasSettings.CanvasUrl != null)
                {
                    url = this.canvasSettings.CanvasUrl.ToString();
                }
                else if (request.Headers.AllKeys.Contains("Host"))
                {
                    // This will attempt to get the url based on the host
                    // in case we are behind a load balancer (such as with azure)
                    url = string.Concat(request.Url.Scheme, "://", request.Headers["Host"]);
                }
                else
                {
                    url = string.Concat(request.Url.Scheme, "://", request.Url.Host, ":", request.Url.Port);
                }
                return new Uri(url);
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
                var parts = request.Url.PathAndQuery.Split('?');
                uriBuilder.Path = parts[0];
                if (parts.Length > 1)
                {
                    uriBuilder.Query = parts[1];
                }
                return uriBuilder.Uri;
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

                var pathAndQuery = request.Url.PathAndQuery;
                var appPath = this.CanvasUrl.AbsolutePath.Replace(this.CanvasPageApplicationPath, String.Empty);
                if (appPath != null && appPath != "/" && appPath.Length > 0)
                {
                    pathAndQuery = pathAndQuery.Replace(appPath, string.Empty);
                }

                return pathAndQuery ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the base url of your application on Facebook.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Uri CanvasPage
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);
                return this.canvasSettings.CanvasPage;
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
        /// Gets the Facebook login URL.
        /// </summary>
        /// <param name="facebookApp">The facebook app.</param>
        /// <param name="permissions">The permissions.</param>
        /// <param name="returnUrlPath">The return URL path.</param>
        /// <param name="cancelUrlPath">The cancel URL path.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "3#"),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public Uri GetLoginUrl(FacebookAppBase facebookApp, string permissions, string returnUrlPath, string cancelUrlPath)
        {
            Contract.Requires(facebookApp != null);
            Contract.Ensures(Contract.Result<Uri>() != null);

            return GetLoginUrl(facebookApp, permissions, returnUrlPath, cancelUrlPath, false);
        }

        /// <summary>
        /// Gets the login url for the current request.
        /// </summary>
        /// <param name="facebookApp">An instance of FacebookAppBase.</param>
        /// <param name="permissions">The comma seperated list of requested permissions.</param>
        /// <param name="returnUrlPath">The path to return the user after autheticating.</param>
        /// <param name="cancelUrlPath">The path to return the user if they do not authenticate.</param>
        /// <param name="cancelToSelf">Should the cancel url return to this same action. (Only do this on soft authorize, otherwise you will get an infinate loop.)</param>
        /// <returns>The cancel url.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "3#")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public Uri GetLoginUrl(FacebookAppBase facebookApp, string permissions, string returnUrlPath, string cancelUrlPath, bool cancelToSelf)
        {
            Contract.Requires(facebookApp != null);
            Contract.Ensures(Contract.Result<Uri>() != null);

            var parameters = new Dictionary<string, object>();
            parameters["req_perms"] = permissions;
            parameters["canvas"] = 1;

            // set the return url
            Uri returnUrl;
            if (!string.IsNullOrEmpty(returnUrlPath))
            {
                returnUrl = BuildAuthReturnUrl(returnUrlPath);
            }
            else
            {
                returnUrl = BuildAuthReturnUrl();
            }
            parameters["next"] = returnUrl.ToString();


            // set the cancel url
            Uri cancelUrl;
            if (!string.IsNullOrEmpty(cancelUrlPath))
            {
                cancelUrl = BuildAuthReturnUrl(cancelUrlPath);
            }
            else if (this.canvasSettings.AuthorizeCancelUrl != null)
            {
                cancelUrl = this.canvasSettings.AuthorizeCancelUrl;
            }
            else
            {
                if (cancelToSelf)
                {
                    cancelUrl = BuildAuthCancelUrl();
                }
                else
                {
                    // Cancel url is facebook.com
                    cancelUrl = new Uri("http://www.facebook.com");
                }

            }
            parameters["cancel_url"] = cancelUrl.ToString();


            return facebookApp.GetLoginUrl(parameters);
        }

        /// <summary>
        /// Builds a Facebook authorization cancel URL.
        /// </summary>
        /// <returns></returns>
        public Uri BuildAuthCancelUrl()
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return BuildAuthCancelUrl(null);
        }

        /// <summary>
        /// Builds a Facebook authorization cancel URL.
        /// </summary>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <returns></returns>
        public Uri BuildAuthCancelUrl(string pathAndQuery)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return BuildAuthReturnUrl(pathAndQuery, true);
        }

        /// <summary>
        /// Builds a Facebook authorization return URL.
        /// </summary>
        /// <returns></returns>
        public Uri BuildAuthReturnUrl()
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return BuildAuthReturnUrl(null);
        }

        /// <summary>
        /// Builds a Facebook authorization return URL.
        /// </summary>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <returns></returns>
        public Uri BuildAuthReturnUrl(string pathAndQuery)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return BuildAuthReturnUrl(pathAndQuery, false);
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
            return new Uri(url);
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

            var appPath = request.ApplicationPath;
            if (appPath != "/")
            {
                appPath = string.Concat(appPath, "/");
            }

            string redirectRoot = string.Concat(redirectPath, "/", cancel ? "cancel" : string.Empty);

            UriBuilder uriBuilder = new UriBuilder(CurrentCanvasUrl);
            uriBuilder.Path = string.Concat(appPath, redirectRoot, CanvasPageApplicationPath, path);
            uriBuilder.Query = null; // No Querystrings allowed in return urls
            return uriBuilder.Uri;
        }

    }
}
