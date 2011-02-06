using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Facebook.Web.Mvc
{
    public static class CanvasInputExtensions
    {

        public static MvcHtmlString FacebookSignedRequest(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Hidden("signed_request", htmlHelper.ViewContext.RequestContext.HttpContext.Request["signed_request"]);
        }

    }
}
