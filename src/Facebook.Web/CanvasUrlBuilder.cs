// --------------------------------
// <copyright file="CanvasUrlHelper.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Facebook.Web
{
    public class CanvasUrlBuilder
    {
        private const string redirectPath = "facebookredirect.axd";
        private HttpRequestBase _request;

        public CanvasUrlBuilder(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            Contract.Requires(request.Url != null);
            Contract.Requires(request.Headers != null);

            this._request = request;
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(_request != null);
            Contract.Invariant(_request.Headers != null);
            Contract.Invariant(_request.Url != null);
        }

        public static string GetCanvasRedirectHtml(Uri url)
        {
            if (url == null) {
                throw new ArgumentNullException("url");
            }
            return GetCanvasRedirectHtml(url.ToString());
        }

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
        /// Facebook pulls the content for your application's 
        /// canvas pages from this base url.
        /// </summary>
        public Uri CanvasUrl
        {
            get
            {
                string url;
                if (CanvasSettings.Current.CanvasUrl != null)
                {
                    url = CanvasSettings.Current.CanvasUrl.ToString();
                }
                else if (_request.Headers.AllKeys.Contains("Host"))
                {
                    // This will attempt to get the url based on the host
                    // in case we are behind a load balancer (such as with azure)
                    url = string.Concat(_request.Url.Scheme, "://", _request.Headers["Host"]);

                }
                else
                {
                    url = string.Concat(_request.Url.Scheme, "://", _request.Url.Host, ":", _request.Url.Port);
                }
                return new Uri(url);
            }
        }

        /// <summary>
        /// The current url of your application being
        /// pulled by Facebook.
        /// </summary>
        public Uri CurrentCanvasUrl
        {
            get
            {
                var pathAndQuery = _request.Url.PathAndQuery;
                var currentCanvasUrl = string.Concat(CanvasUrl, pathAndQuery);
                return new Uri(currentCanvasUrl);
            }
        }

        /// <summary>
        /// The current Path and query of the application 
        /// being pulled by Facebook.
        /// </summary>
        public string CurrentCanvasPathAndQuery
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                var pathAndQuery = _request.Url.PathAndQuery;
                var appPath = _request.ApplicationPath;
                if (appPath != null && appPath != "/" && appPath.Length > 0)
                {
                    pathAndQuery = pathAndQuery.Replace(appPath, string.Empty);
                }

                return pathAndQuery ?? string.Empty;
            }
        }

        /// <summary>
        /// The base url of your application on Facebook.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Uri CanvasPageUrl
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                if (CanvasSettings.Current == null)
                {
                    throw new InvalidOperationException("Canvas settings not found in application configuration.");
                }
                return CanvasSettings.Current.CanvasPageUrl;
            }
        }

        /// <summary>
        /// The current url of the application on facebook.
        /// </summary>
        public Uri CanvasPageCurrentUrl
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

                var path = CanvasPageUrl.AbsolutePath.Replace("/", string.Empty);
                if (String.IsNullOrEmpty(path))
                {
                    throw new InvalidOperationException("Invalid path.");
                }
                return path;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "3#"), 
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public Uri GetLoginUrl(FacebookAppBase facebookApp, string permissions, string returnUrlPath, string cancelUrlPath)
        {
            if (facebookApp == null)
            {
                throw new ArgumentNullException("facebookApp");
            }
            Contract.Ensures(Contract.Result<Uri>() != null);

            return GetLoginUrl(facebookApp, permissions, returnUrlPath, cancelUrlPath, false);
        }

        /// <summary>
        /// Gets the login url for the current request.
        /// </summary>
        /// <param name="filterContext">The current AuthorizationContext.</param>
        /// <param name="cancelToSelf">Should the cancel url return to this same action. (Only do this on soft authorize, otherwise you will get an infinate loop.)</param>
        /// <returns>The cancel url.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "3#")] 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public Uri GetLoginUrl(FacebookAppBase facebookApp, string permissions, string returnUrlPath, string cancelUrlPath, bool cancelToSelf)
        {
            if (facebookApp == null)
            {
                throw new ArgumentNullException("facebookApp");
            }
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
            else if (CanvasSettings.Current.AuthorizeCancelUrl != null)
            {
                cancelUrl = CanvasSettings.Current.AuthorizeCancelUrl;
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

        public Uri BuildAuthCancelUrl()
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return BuildAuthCancelUrl(null);
        }

        public Uri BuildAuthCancelUrl(string pathAndQuery)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return BuildAuthReturnUrl(pathAndQuery, true);
        }

        public Uri BuildAuthReturnUrl()
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return BuildAuthReturnUrl(null);
        }

        public Uri BuildAuthReturnUrl(string pathAndQuery)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return BuildAuthReturnUrl(pathAndQuery, false);
        }

        public Uri BuildCanvasPageUrl(string pathAndQuery)
        {
            if (String.IsNullOrEmpty(pathAndQuery))
            {
                throw new ArgumentNullException("pathAndQuery");
            }
            Contract.Ensures(Contract.Result<Uri>() != null);

            if (!pathAndQuery.StartsWith("/", StringComparison.Ordinal))
            {
                pathAndQuery = String.Concat("/", pathAndQuery);
            }

            var url = string.Concat(CanvasPageUrl, pathAndQuery);
            return new Uri(url);
        }

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

            var appPath = _request.ApplicationPath;
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
