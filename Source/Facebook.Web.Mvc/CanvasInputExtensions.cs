// --------------------------------
// <copyright file="CanvasInputExtensions.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// Represents the canvas input extensions.
    /// </summary>
    public static class CanvasInputExtensions
    {
        /// <summary>
        /// Html helper for Facebook Signed Request.
        /// </summary>
        /// <param name="htmlHelper">The html helper.</param>
        /// <returns>Returns the Facebook Signed Request.</returns>
        public static MvcHtmlString FacebookSignedRequest(this HtmlHelper htmlHelper)
        {
            return htmlHelper.FacebookSignedRequest(htmlHelper.ViewContext.RequestContext.HttpContext.Request["signed_request"]);
        }

        /// <summary>
        /// Html helper for Facebook Signed Request.
        /// </summary>
        /// <param name="htmlHelper">The html helper.</param>
        /// <param name="signedRequestValue">The signed request value.</param>
        /// <returns>Returns the Facebook Signed Request.</returns>
        public static MvcHtmlString FacebookSignedRequest(this HtmlHelper htmlHelper, string signedRequestValue)
        {
            return htmlHelper.Hidden("signed_request", signedRequestValue);
        }

    }
}
