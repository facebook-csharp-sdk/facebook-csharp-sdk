namespace Facebook.Web.Mvc
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class CanvasInputExtensions
    {
        public static MvcHtmlString FacebookSignedRequest(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Hidden("signed_request", htmlHelper.ViewContext.RequestContext.HttpContext.Request["signed_request"]);
        }

    }
}
