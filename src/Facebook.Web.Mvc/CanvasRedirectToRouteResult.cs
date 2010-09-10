using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics.Contracts;

namespace Facebook.Web.Mvc
{
    public class CanvasRedirectToRouteResult : RedirectToRouteResult
    {

        public CanvasRedirectToRouteResult(RouteValueDictionary routeValues) : base(routeValues) { }

        public CanvasRedirectToRouteResult(string routeName, RouteValueDictionary routeValues) : base(routeName, routeValues) { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public override void ExecuteResult(ControllerContext context)
        {
            string destinationPathAndQuery = UrlHelper.GenerateUrl(RouteName, null /* actionName */, null /* controllerName */, RouteValues, RouteTable.Routes, context.RequestContext, false /* includeImplicitMvcValues */);

            var canvasUrlBuilder = new CanvasUrlBuilder(context.HttpContext.Request);

            var canvasUrl = canvasUrlBuilder.BuildCanvasPageUrl(destinationPathAndQuery);

            var content = CanvasUrlBuilder.GetCanvasRedirectHtml(canvasUrl.ToString());

            context.Controller.TempData.Keep();

            context.HttpContext.Response.ContentType = "text/html";
            context.HttpContext.Response.Write(content);
        }

    }
}
