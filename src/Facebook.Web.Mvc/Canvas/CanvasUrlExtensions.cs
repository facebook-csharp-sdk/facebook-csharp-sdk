// --------------------------------
// <copyright file="CanvasUrlExtensions.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebookgraphtoolkit.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Facebook.Web.Mvc.Canvas {
    public static class CanvasUrlExtensions {

        public static string CanvasAction(this UrlHelper url, string actionName) {
            return ToCanvasUrl(url.Action(actionName));
        }
        public static string CanvasAction(this UrlHelper url, string actionName, object routeValues) {
            return ToCanvasUrl(url.Action(actionName, routeValues));
        }
        public static string CanvasAction(this UrlHelper url, string actionName, RouteValueDictionary routeValues) {
            return ToCanvasUrl(url.Action(actionName, routeValues));
        }
        public static string CanvasAction(this UrlHelper url, string actionName, string controllerName) {
            return ToCanvasUrl(url.Action(actionName, controllerName));
        }
        public static string CanvasAction(this UrlHelper url, string actionName, string controllerName, object routeValues) {
            return ToCanvasUrl(url.Action(actionName, controllerName, routeValues));
        }
        public static string CanvasAction(this UrlHelper url, string actionName, string controllerName, RouteValueDictionary routeValues) {
            return ToCanvasUrl(url.Action(actionName, controllerName, routeValues));
        }
        public static string CanvasAction(this UrlHelper url, string actionName, string controllerName, object routeValues, string protocol) {
            return ToCanvasUrl(url.Action(actionName, controllerName, routeValues, protocol));
        }
        public static string CanvasRouteUrl(this UrlHelper url, RouteValueDictionary routeValues) {
            return ToCanvasUrl(url.RouteUrl(routeValues));
        }
        public static string CanvasRouteUrl(this UrlHelper url, string routeName, object routeValues) {
            return ToCanvasUrl(url.RouteUrl(routeValues));
        }
        public static string CanvasRouteUrl(this UrlHelper url, string routeName, RouteValueDictionary routeValues) {
            return ToCanvasUrl(url.RouteUrl(routeValues));
        }
        public static string CanvasRouteUrl(this UrlHelper url, string routeName, object routeValues, string protocol) {
            return ToCanvasUrl(url.RouteUrl(routeValues));
        }
        public static string CanvasRouteUrl(this UrlHelper url, string routeName, RouteValueDictionary routeValues, string protocol, string hostName) {
            return ToCanvasUrl(url.RouteUrl(routeValues));
        }
        public static string CanvasContent(this UrlHelper url, string contentPath) {
            return ToCanvasUrl(url.Content(contentPath));
        }
        private static string ToCanvasUrl(string path) {
            FacebookApp app = new FacebookApp();
            return app.GetCanvasPageUrl(path);
        }

    }
}
