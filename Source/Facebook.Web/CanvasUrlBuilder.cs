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
using System.Collections.Specialized;
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
        public Uri CanvasPage
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);
                return this.canvasSettings.CanvasPageUrl;
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
        public Uri GetLoginUrl(FacebookAppBase facebookApp, string permissions, string returnUrlPath, string cancelUrlPath)
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
                returnUrl = BuildCanvasPageUrl(returnUrlPath);
            }
            else
            {
                returnUrl = BuildCanvasPageUrl(this.CurrentCanvasPathAndQuery);
            }
            parameters["next"] = returnUrl.ToString();


            // set the cancel url
            Uri cancelUrl;
            if (!string.IsNullOrEmpty(cancelUrlPath))
            {
                cancelUrl = BuildCanvasPageUrl(cancelUrlPath);
            }
            else if (this.canvasSettings.AuthorizeCancelUrl != null)
            {
                cancelUrl = this.canvasSettings.AuthorizeCancelUrl;
            }
            else
            {
                cancelUrl = BuildCanvasPageUrl(this.CurrentCanvasPathAndQuery);
            }
            parameters["cancel_url"] = cancelUrl.ToString();


            return facebookApp.GetLoginUrl(parameters);
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

            // remove signed request from the query string, but keep the rest of the query string there
            var pathArray = pathAndQuery.Split("?".ToCharArray());
            if (pathArray.Length > 1)
            {
                var queryStrings = pathArray[1];
                var col = HttpUtility.ParseQueryString("?" + queryStrings);
                if (!string.IsNullOrEmpty(col["signed_request"]))
                {
                    col.Remove("signed_request");
                }

                // if more than one facebook will forward one query string param
                string query = "";
                if (col.AllKeys.Length > 1)
                {
                    query = "?";
                    foreach (var c in col.AllKeys)
                    {
                        query += c + "=" + col[c] + "&";
                    }
                    query = query.TrimEnd("&".ToCharArray());
                    
                }
                pathAndQuery = pathArray[0] + query;
            }


            var url = string.Concat(CanvasPage, pathAndQuery);
            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }


            return new Uri(HttpUtility.HtmlEncode(url));
        }

    }
}