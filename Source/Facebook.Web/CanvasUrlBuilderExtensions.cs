
namespace Facebook.Web
{
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// Extensions methods for Canvas Url Builder
    /// </summary>
    public static class CanvasUrlBuilderExtensions
    {
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

            return GetCanvasPageUrl(FacebookApplication.Current, new HttpRequestWrapper(HttpContext.Current.Request), resolvedUrl);
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

            return GetCanvasUrl(FacebookApplication.Current, new HttpRequestWrapper(HttpContext.Current.Request), resolvedUrl);
        }

        internal static string GetCanvasPageUrl(IFacebookApplication facebookApplication, HttpRequestBase request, string pathAndQuery)
        {
            var canvasUrlBuilder = new CanvasUrlBuilder(facebookApplication, request);

            return canvasUrlBuilder.BuildCanvasPageUrl(pathAndQuery).AbsoluteUri;
        }

        internal static string GetCanvasUrl(IFacebookApplication facebookApplication, HttpRequestWrapper request, string pathAndQuery)
        {
            var canvasUrlBuilder = new CanvasUrlBuilder(facebookApplication, request);

            return canvasUrlBuilder.BuildCanvasUrl(pathAndQuery).AbsoluteUri;
        }
    }
}