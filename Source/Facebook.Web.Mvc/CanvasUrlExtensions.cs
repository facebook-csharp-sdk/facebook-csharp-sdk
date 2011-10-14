// --------------------------------
// <copyright file="CanvasUrlExtensions.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Provides extension methods for building canvas urls.
    /// </summary>
    public static class CanvasUrlExtensions
    {
        /// <summary>
        /// Gets the canvas action url.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        public static string CanvasAction(this UrlHelper urlHelper, string actionName)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, null, (RouteValueDictionary)null /* routeValues */);
        }

        /// <summary>
        /// Gets the canvas action url.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static string CanvasAction(this UrlHelper urlHelper, string actionName, object routeValues)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, null /* controllerName */, new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// Gets the canvas action url.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static string CanvasAction(this UrlHelper urlHelper, string actionName, RouteValueDictionary routeValues)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, null /* controllerName */, routeValues);
        }

        /// <summary>
        /// Gets the canvas action url.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns></returns>
        public static string CanvasAction(this UrlHelper urlHelper, string actionName, string controllerName)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, controllerName, (RouteValueDictionary)null /* routeValues */);
        }

        /// <summary>
        /// Gets the canvas action url.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static string CanvasAction(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// Gets the canvas action url.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static string CanvasAction(this UrlHelper urlHelper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, controllerName, routeValues);
        }

        /// <summary>
        /// Gets the canvas action url.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="protocol">The protocol.</param>
        /// <returns></returns>
        public static string CanvasAction(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues, string protocol)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, controllerName, protocol, null /* hostName */, null /* fragment */, new RouteValueDictionary(routeValues), urlHelper.RouteCollection, urlHelper.RequestContext, true /* includeImplicitMvcValues */);
        }

        /// <summary>
        /// Gets the canvas action url.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <returns></returns>
        public static string CanvasAction(this UrlHelper urlHelper, string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            return GenerateCanvasUrl(urlHelper, null /* routeName */, actionName, controllerName, protocol, hostName, null /* fragment */, routeValues, urlHelper.RouteCollection, urlHelper.RequestContext, true /* includeImplicitMvcValues */);
        }

        /// <summary>
        /// Generates the canvas URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        private static string GenerateCanvasUrl(UrlHelper urlHelper, string routeName, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            return GenerateCanvasUrl(urlHelper, routeName, actionName, controllerName, routeValues, urlHelper.RouteCollection, urlHelper.RequestContext, true /* includeImplicitMvcValues */);
        }

        /// <summary>
        /// Generates the canvas URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="fragment">The fragment.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="routeCollection">The route collection.</param>
        /// <param name="requestContext">The request context.</param>
        /// <param name="includeImplicitMvcValues">if set to <c>true</c> [include implicit MVC values].</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string GenerateCanvasUrl(UrlHelper urlHelper, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, RouteCollection routeCollection, RequestContext requestContext, bool includeImplicitMvcValues)
        {
            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, includeImplicitMvcValues);
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(FacebookApplication.Current, requestContext.HttpContext.Request);
            return urlBuilder.BuildCanvasPageUrl(url).ToString();
        }

        /// <summary>
        /// Generates the canvas URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="routeCollection">The route collection.</param>
        /// <param name="requestContext">The request context.</param>
        /// <param name="includeImplicitMvcValues">if set to <c>true</c> [include implicit MVC values].</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string GenerateCanvasUrl(UrlHelper urlHelper, string routeName, string actionName, string controllerName, RouteValueDictionary routeValues, RouteCollection routeCollection, RequestContext requestContext, bool includeImplicitMvcValues)
        {
            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, routeValues, routeCollection, requestContext, includeImplicitMvcValues);
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(FacebookApplication.Current, urlHelper.RequestContext.HttpContext.Request);
            return urlBuilder.BuildCanvasPageUrl(url).ToString();
        }

        /// <summary>
        /// Gets the canvas route URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, object routeValues)
        {
            return CanvasRouteUrl(urlHelper, null /* routeName */, routeValues);
        }

        /// <summary>
        /// Gets the canvas route URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, RouteValueDictionary routeValues)
        {
            return CanvasRouteUrl(urlHelper, null /* routeName */, routeValues);
        }

        /// <summary>
        /// Gets the canvas route URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, string routeName)
        {
            return CanvasRouteUrl(urlHelper, routeName, (object)null /* routeValues */);
        }

        /// <summary>
        /// Gets the canvas route URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, string routeName, object routeValues)
        {
            return CanvasRouteUrl(urlHelper, routeName, routeValues, null /* protocol */);
        }

        /// <summary>
        /// Gets the canvas route URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, string routeName, RouteValueDictionary routeValues)
        {
            return CanvasRouteUrl(urlHelper, routeName, routeValues, null /* protocol */, null /* hostName */);
        }

        /// <summary>
        /// Gets the canvas route URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="protocol">The protocol.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, string routeName, object routeValues, string protocol)
        {
            return GenerateCanvasUrl(urlHelper, routeName, null /* actionName */, null /* controllerName */, protocol, null /* hostName */, null /* fragment */, new RouteValueDictionary(routeValues), urlHelper.RouteCollection, urlHelper.RequestContext, false /* includeImplicitMvcValues */);
        }

        /// <summary>
        /// Gets the canvas route URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public static string CanvasRouteUrl(this UrlHelper urlHelper, string routeName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            return GenerateCanvasUrl(urlHelper, routeName, null /* actionName */, null /* controllerName */, protocol, hostName, null /* fragment */, routeValues, urlHelper.RouteCollection, urlHelper.RequestContext, false /* includeImplicitMvcValues */);
        }
    }
}