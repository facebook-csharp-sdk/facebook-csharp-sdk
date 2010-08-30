using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Facebook.Web.Mvc.Canvas
{
    public class CanvasRedirectToRouteResult : RedirectToRouteResult
    {

        public CanvasRedirectToRouteResult(RouteValueDictionary routeValues) : base(routeValues) { }

        public CanvasRedirectToRouteResult(string routeName, RouteValueDictionary routeValues) : base(routeName, routeValues) { }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            string destinationPathAndQuery = UrlHelper.GenerateUrl(RouteName, null /* actionName */, null /* controllerName */, RouteValues, RouteTable.Routes, context.RequestContext, false /* includeImplicitMvcValues */);

            var canvasUrlBuilder = new FacebookUrlBuilder(context.HttpContext.Request);

            var canvasUrl = canvasUrlBuilder.BuildFacebookAppUrl(destinationPathAndQuery);

            var content = FacebookUrlBuilder.GetCanvasRedirectHtml(canvasUrl.ToString());

            context.Controller.TempData.Keep();

            context.HttpContext.Response.ContentType = "text/html";
            context.HttpContext.Response.Write(content);
        }

    }
}
