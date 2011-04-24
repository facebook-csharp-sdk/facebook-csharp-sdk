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

    public static class CanvasInputExtensions
    {
        public static MvcHtmlString FacebookSignedRequest(this HtmlHelper htmlHelper)
        {
            return htmlHelper.FacebookSignedRequest(htmlHelper.ViewContext.RequestContext.HttpContext.Request["signed_request"]);
        }

        public static MvcHtmlString FacebookSignedRequest(this HtmlHelper htmlHelper, string signedRequestValue)
        {
            return htmlHelper.Hidden("signed_request", signedRequestValue);
        }

    }
}
