// --------------------------------
// <copyright file="CanvasUrlBuilderExtensions.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// Extensions methods for Canvas Url Builder
    /// </summary>
    public static class CanvasUrlBuilderExtensions
    {
        private const string HttpContextKey = "facebook_canvasurlbuilder";

        /// <summary>
        /// Gets a canvas page url that can be used by the browser.
        /// </summary>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="relativeUrl">
        /// The relative url.
        /// </param>
        /// <returns>
        /// The canvas page url.
        /// </returns>
        public static string ResolveCanvasPageUrl(this Control control, string relativeUrl)
        {
            var resolvedUrl = control.ResolveUrl(relativeUrl);

            return ResolveCanvasPageUrl(resolvedUrl, new HttpContextWrapper(HttpContext.Current));
        }

        internal static string ResolveCanvasPageUrl(string resolvedUrl, HttpContextWrapper httpContext)
        {
            return GetCanvasUrlBuilder(httpContext).BuildCanvasPageUrl(resolvedUrl).AbsoluteUri;
        }

        /// <summary>
        /// Gets a canvas url that can be used by the browser.
        /// </summary>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="relativeUrl">
        /// The relative url.
        /// </param>
        /// <returns>
        /// The canvas url.
        /// </returns>
        public static string ResolveCanvasUrl(this Control control, string relativeUrl)
        {
            var resolvedUrl = control.ResolveUrl(relativeUrl);

            return ResolveCanvasUrl(resolvedUrl, new HttpContextWrapper(HttpContext.Current));
        }

        private static string ResolveCanvasUrl(string resolvedUrl, HttpContextWrapper httpContext)
        {
            return GetCanvasUrlBuilder(httpContext).BuildCanvasUrl(resolvedUrl).AbsoluteUri;
        }

        internal static CanvasUrlBuilder GetCanvasUrlBuilder(HttpContextWrapper httpContext)
        {
            var items = httpContext.Items;
            var httpRequest = httpContext.Request;
            CanvasUrlBuilder canvasUrlBuilder;

            if (items[HttpContextKey] == null)
            {
                canvasUrlBuilder = new CanvasUrlBuilder(FacebookApplication.Current, httpRequest);
                items[HttpContextKey] = canvasUrlBuilder;
            }
            else
            {
                canvasUrlBuilder = (CanvasUrlBuilder)items[HttpContextKey];
            }

            return canvasUrlBuilder;
        }
    }
}