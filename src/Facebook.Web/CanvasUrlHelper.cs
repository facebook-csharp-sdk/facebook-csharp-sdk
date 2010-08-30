// --------------------------------
// <copyright file="CanvasUrlHelper.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebookgraphtoolkit.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Facebook.Web
{
    public static class CanvasUrlHelper
    {
        public static string GetCanvasPageUrl(this FacebookApp app)
        {
            if (FacebookSettings.Current.CanvasPageUrl == null)
            {
                throw new System.Configuration.ConfigurationErrorsException("The Facebook 'CanvasPageUrl' setting does not have a value.");
            }
            return FacebookSettings.Current.CanvasPageUrl.ToString();
        }

        public static string GetCanvasPageUrl(this FacebookApp app, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            if (path.StartsWith("/"))
            {
                path = path.Substring(1, path.Length - 1);
            }
            return string.Concat(GetCanvasPageUrl(app), path);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "app")]
        public static string GetCanvasRedirectScript(this FacebookApp app, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            return GetCanvasRedirectScript(uri.ToString());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "app")]
        public static string GetCanvasRedirectScript(this FacebookApp app, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            return GetCanvasRedirectScript(url);
        }

        public static string GetCanvasRedirectScript(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            return GetCanvasRedirectScript(uri.ToString());
        }

        public static string GetCanvasRedirectScript(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            return "<script type=\"text/javascript\">\n" +
                    "if (parent != self) \n" +
                    "top.location.href = \"" + url + "\";\n" +
                    "else self.location.href = \"" + url + "\";\n" +
                    "</script>";
        }

        public static string GetCanvasRedirectHtml(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            return GetCanvasRedirectHtml(uri.ToString());
        }

        public static string GetCanvasRedirectHtml(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            return "<html><head>" +
                    GetCanvasRedirectScript(url) +
                   "</head><body></body></html>";
        }
    }
}
