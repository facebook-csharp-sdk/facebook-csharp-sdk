using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using System.Web.Routing;

namespace Facebook.Web.Mvc
{
    /// <summary>
    /// Provides extension methods for buiiding canvas urls.
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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);
            var appSettings = (IFacebookAppSettings)requestContext.HttpContext.Items[NFacebookAuthorizeAttribute.HttpContextCurrentAppSettingsKey];

            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, includeImplicitMvcValues);
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(appSettings, requestContext.HttpContext.Request);
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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

            var appSettings = (IFacebookAppSettings)requestContext.HttpContext.Items[NFacebookAuthorizeAttribute.HttpContextCurrentAppSettingsKey];

            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, routeValues, routeCollection, requestContext, includeImplicitMvcValues);
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(appSettings, urlHelper.RequestContext.HttpContext.Request);
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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

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
            Contract.Requires(urlHelper != null && urlHelper.RequestContext != null && urlHelper.RequestContext.HttpContext != null && urlHelper.RequestContext.HttpContext.Request != null &&
                urlHelper.RequestContext.HttpContext.Request.Url != null && urlHelper.RequestContext.HttpContext.Request.Headers != null && urlHelper.RouteCollection != null);

            return GenerateCanvasUrl(urlHelper, routeName, null /* actionName */, null /* controllerName */, protocol, hostName, null /* fragment */, routeValues, urlHelper.RouteCollection, urlHelper.RequestContext, false /* includeImplicitMvcValues */);
        }
    }
}