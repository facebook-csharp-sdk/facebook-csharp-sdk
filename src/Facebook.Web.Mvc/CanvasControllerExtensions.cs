using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics.CodeAnalysis;

namespace Facebook.Web.Mvc
{
    public static class CanvasControllerExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Instance method for consistency with other helpers.")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#",
            Justification = "Response.Redirect() takes its URI as a string parameter.")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "controller", 
            Justification="Extension method")]
        public static RedirectResult CanvasRedirect(this Controller controller, string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            return new CanvasRedirectResult(url);
        }

        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName)
        {
            return CanvasRedirectToAction(controller, actionName, (RouteValueDictionary)null);
        }

        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName, object routeValues)
        {
            return CanvasRedirectToAction(controller, actionName, new RouteValueDictionary(routeValues));
        }

        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName, RouteValueDictionary routeValues)
        {
            return CanvasRedirectToAction(controller, actionName, null /* controllerName */, routeValues);
        }

        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName, string controllerName)
        {
            return CanvasRedirectToAction(controller, actionName, controllerName, (RouteValueDictionary)null);
        }

        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName, string controllerName, object routeValues)
        {
            return CanvasRedirectToAction(controller, actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "controller")]
        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            routeValues = routeValues ?? new RouteValueDictionary();
            if (actionName != null)
            {
                routeValues["action"] = actionName;
            }
            if (controllerName != null)
            {
                routeValues["controller"] = controllerName;
            }
            return new CanvasRedirectToRouteResult(routeValues);
        }

        public static RedirectToRouteResult CanvasRedirectToRoute(this Controller controller, object routeValues)
        {
            return CanvasRedirectToRoute(controller, new RouteValueDictionary(routeValues));
        }

        public static RedirectToRouteResult CanvasRedirectToRoute(this Controller controller, RouteValueDictionary routeValues)
        {
            return CanvasRedirectToRoute(controller, null /* routeName */, routeValues);
        }

        public static RedirectToRouteResult CanvasRedirectToRoute(this Controller controller, string routeName)
        {
            return CanvasRedirectToRoute(controller, routeName, (RouteValueDictionary)null);
        }

        public static RedirectToRouteResult CanvasRedirectToRoute(this Controller controller, string routeName, object routeValues)
        {
            return CanvasRedirectToRoute(controller, routeName, new RouteValueDictionary(routeValues));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "controller",
            Justification = "Extension method")]
        public static RedirectToRouteResult CanvasRedirectToRoute(this Controller controller, string routeName, RouteValueDictionary routeValues)
        {
            return new CanvasRedirectToRouteResult(routeName, routeValues ?? new RouteValueDictionary());
        }

    }
}
