using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace Facebook.Web.Mvc
{
    public static class CanvasUrlExtensions
    {
        public static string CanvasAction(this UrlHelper urlHelper, string actionName)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, null, (RouteValueDictionary)null /* routeValues */);
        }

        public static string CanvasAction(this UrlHelper urlHelper, string actionName, object routeValues)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, null /* controllerName */, new RouteValueDictionary(routeValues));
        }

        public static string CanvasAction(this UrlHelper urlHelper, string actionName, RouteValueDictionary routeValues)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, null /* controllerName */, routeValues);
        }

        public static string CanvasAction(this UrlHelper urlHelper, string actionName, string controllerName)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, controllerName, (RouteValueDictionary)null /* routeValues */);
        }

        public static string CanvasAction(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        public static string CanvasAction(this UrlHelper urlHelper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, controllerName, routeValues);
        }

        public static string CanvasAction(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues, string protocol)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, controllerName, protocol, null /* hostName */, null /* fragment */, new RouteValueDictionary(routeValues), urlHelper.RouteCollection, urlHelper.RequestContext, true /* includeImplicitMvcValues */);
        }

        public static string CanvasAction(this UrlHelper urlHelper, string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, controllerName, protocol, hostName, null /* fragment */, routeValues, urlHelper.RouteCollection, urlHelper.RequestContext, true /* includeImplicitMvcValues */);
        }

        private static string GenerateCanvasUrl(UrlHelper urlHelper, string routeName, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            return GenerateCanvasUrl(urlHelper, routeName, actionName, controllerName, routeValues, urlHelper.RouteCollection, urlHelper.RequestContext, true /* includeImplicitMvcValues */);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string GenerateCanvasUrl(UrlHelper urlHelper, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, RouteCollection routeCollection, RequestContext requestContext, bool includeImplicitMvcValues)
        {
            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, includeImplicitMvcValues);
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(urlHelper.RequestContext.HttpContext.Request);
            return urlBuilder.BuildCanvasPageUrl(url).ToString();
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string GenerateCanvasUrl(UrlHelper urlHelper, string routeName, string actionName, string controllerName, RouteValueDictionary routeValues, RouteCollection routeCollection, RequestContext requestContext, bool includeImplicitMvcValues)
        {
            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, routeValues, routeCollection, requestContext, includeImplicitMvcValues);
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(urlHelper.RequestContext.HttpContext.Request);
            return urlBuilder.BuildCanvasPageUrl(url).ToString();
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, object routeValues)
        {
            return CanvasRouteUrl(urlHelper, null /* routeName */, routeValues);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, RouteValueDictionary routeValues)
        {
            return CanvasRouteUrl(urlHelper, null /* routeName */, routeValues);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, string routeName)
        {
            return CanvasRouteUrl(urlHelper, routeName, (object)null /* routeValues */);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, string routeName, object routeValues)
        {
            return CanvasRouteUrl(urlHelper, routeName, routeValues, null /* protocol */);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, string routeName, RouteValueDictionary routeValues)
        {
            return CanvasRouteUrl(urlHelper, routeName, routeValues, null /* protocol */, null /* hostName */);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, string routeName, object routeValues, string protocol)
        {
            return GenerateCanvasUrl(urlHelper, routeName, null /* actionName */, null /* controllerName */, protocol, null /* hostName */, null /* fragment */, new RouteValueDictionary(routeValues), urlHelper.RouteCollection, urlHelper.RequestContext, false /* includeImplicitMvcValues */);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, string routeName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            return GenerateCanvasUrl(urlHelper, routeName, null /* actionName */, null /* controllerName */, protocol, hostName, null /* fragment */, routeValues, urlHelper.RouteCollection, urlHelper.RequestContext, false /* includeImplicitMvcValues */);
        }
    }
}
