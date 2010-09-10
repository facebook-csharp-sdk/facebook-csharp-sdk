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

namespace Facebook.Web
{
    public class FacebookUrlBuilder
    {
        private const string redirectPath = "facebookredirect.axd";
        private HttpRequestBase _request;

        public FacebookUrlBuilder(HttpRequestBase request)
        {
            this._request = request;
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(_request != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
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

        public Uri HostRootUrl
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Uri HostCurrentUrl
        {
            get
            {
                Uri clientUrl = _request.Url;

                // Fix for Windows Azure
                if (_request.Headers.AllKeys.Contains("Host"))
                {
                    clientUrl = new Uri("http://" + _request.Headers["Host"]);
                }
                return clientUrl;
            }
        }

        public Uri FacebookAppRootUrl
        {
            get
            {
                if (CanvasSettings.Current.CanvasPageUrl == null)
                {
                    throw new ConfigurationErrorsException("You must set the canvas page url in the configuraiton settings.");
                }
                return CanvasSettings.Current.CanvasPageUrl;
            }
        }

        public Uri CanvasCurrentUrl
        {
            get
            {
                return BuildCanvasUrl(CanvasPathAndQuery);
            }
        }

        public string CanvasApplicationPath
        {
            get
            {
                if (CanvasSettings.Current.CanvasPageUrl == null)
                {
                    throw new ConfigurationErrorsException("You must set the canvas page url in the configuraiton settings.");
                }
                return CanvasSettings.Current.CanvasPageUrl.AbsolutePath.Replace("/", string.Empty);
            }
        }

        public string CanvasPathAndQuery
        {
            get
            {
                var pathAndQuery = _request.Url.PathAndQuery;
                var appPath = _request.ApplicationPath;
                if (appPath != "/")
                {
                    pathAndQuery = pathAndQuery.Replace(appPath, string.Empty);
                }
                if (!pathAndQuery.Contains("?") && !pathAndQuery.EndsWith("/"))
                {
                    pathAndQuery += "/";
                }
                return pathAndQuery;
            }
        }

        public Uri BuildAuthCancelUrl(string pathAndQuery = null)
        {
            return BuildAuthReturnUrl(pathAndQuery, true);
        }

        public Uri BuildAuthReturnUrl(string pathAndQuery = null)
        {
            return BuildAuthReturnUrl(pathAndQuery, false);
        }

        private Uri BuildAuthReturnUrl(string pathAndQuery, bool cancel)
        {
            if (!string.IsNullOrEmpty(pathAndQuery) && pathAndQuery.StartsWith("/"))
            {
                pathAndQuery = pathAndQuery.Substring(1);
            }

            if (pathAndQuery == null)
            {
                pathAndQuery = CanvasPathAndQuery;
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

            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            if (!path.EndsWith("/"))
            {
                path = path + "/";
            }

            var appPath = _request.ApplicationPath;
            if (appPath != "/")
            {
                appPath = string.Concat(appPath, "/");
            }

            string redirectRoot = string.Concat(redirectPath, "/", cancel ? "cancel/" : string.Empty);

            UriBuilder uriBuilder = new UriBuilder(HostCurrentUrl);
            uriBuilder.Path = string.Concat(appPath, redirectRoot, CanvasApplicationPath, path);
            uriBuilder.Query = null; // No Querystrings allowed in return urls
            return uriBuilder.Uri;
        }

        public Uri BuildCanvasUrl(string pathAndQuery)
        {
            if (pathAndQuery.StartsWith("/"))
            {
                pathAndQuery = pathAndQuery.Substring(1, pathAndQuery.Length - 1);
            }

            var url = string.Concat(FacebookAppRootUrl, pathAndQuery);
            if (!pathAndQuery.Contains("?") && !url.EndsWith("/"))
            {
                url += "/";
            }
            return new Uri(url);
        }

    }
}
