// --------------------------------
// <copyright file="CanvasControllerExtensions.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Canvas extension methods.
    /// </summary>
    public static class CanvasControllerExtensions
    {
        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Instance method for consistency with other helpers.")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#",
            Justification = "Response.Redirect() takes its URI as a string parameter.")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "controller",
            Justification = "Extension method")]
        public static RedirectResult CanvasRedirect(this Controller controller, string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            return new CanvasRedirectResult(url);
        }

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName)
        {
            return CanvasRedirectToAction(controller, actionName, (RouteValueDictionary)null);
        }

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName, object routeValues)
        {
            return CanvasRedirectToAction(controller, actionName, new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName, RouteValueDictionary routeValues)
        {
            return CanvasRedirectToAction(controller, actionName, null /* controllerName */, routeValues);
        }

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns></returns>
        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName, string controllerName)
        {
            return CanvasRedirectToAction(controller, actionName, controllerName, (RouteValueDictionary)null);
        }

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static RedirectToRouteResult CanvasRedirectToAction(this Controller controller, string actionName, string controllerName, object routeValues)
        {
            return CanvasRedirectToAction(controller, actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static RedirectToRouteResult CanvasRedirectToRoute(this Controller controller, object routeValues)
        {
            return CanvasRedirectToRoute(controller, new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static RedirectToRouteResult CanvasRedirectToRoute(this Controller controller, RouteValueDictionary routeValues)
        {
            return CanvasRedirectToRoute(controller, null /* routeName */, routeValues);
        }

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <returns></returns>
        public static RedirectToRouteResult CanvasRedirectToRoute(this Controller controller, string routeName)
        {
            return CanvasRedirectToRoute(controller, routeName, (RouteValueDictionary)null);
        }

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static RedirectToRouteResult CanvasRedirectToRoute(this Controller controller, string routeName, object routeValues)
        {
            return CanvasRedirectToRoute(controller, routeName, new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// Performs a canvas redirect.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "controller",
            Justification = "Extension method")]
        public static RedirectToRouteResult CanvasRedirectToRoute(this Controller controller, string routeName, RouteValueDictionary routeValues)
        {
            return new CanvasRedirectToRouteResult(routeName, routeValues ?? new RouteValueDictionary());
        }

    }
}
