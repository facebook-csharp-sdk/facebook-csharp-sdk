// --------------------------------
// <copyright file="CanvasLinkExtensions.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Configuration;
using System.Web;

namespace Facebook.Web.Mvc
{
    public static class CanvasLinkExtensions
    {

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, htmlAttributes);
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes["target"] = "_top";
            return ToCanvasLink(htmlHelper.ViewContext.HttpContext, htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes));
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes["target"] = "_top";
            return ToCanvasLink(htmlHelper.ViewContext.HttpContext, htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, object routeValues)
        {
            return CanvasRouteLink(htmlHelper, linkText, new RouteValueDictionary(routeValues));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary routeValues)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeName, (object)null /* routeValues */ );
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeName, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, object routeValues, object htmlAttributes)
        {
            return CanvasRouteLink(htmlHelper, linkText, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return CanvasRouteLink(htmlHelper, linkText, null /* routeName */, routeValues, htmlAttributes);
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues, object htmlAttributes)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes["target"] = "_top";
            return ToCanvasLink(htmlHelper.ViewContext.HttpContext, htmlHelper.RouteLink(linkText, routeName, routeValues, htmlAttributes));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes["target"] = "_top";
            return ToCanvasLink(htmlHelper.ViewContext.HttpContext, htmlHelper.RouteLink(linkText, routeName, protocol, hostName, fragment, routeValues, htmlAttributes));

        }
        private static MvcHtmlString ToCanvasLink(HttpContextBase httpContext, MvcHtmlString html)
        {
            FacebookUrlBuilder canvasUrl = new FacebookUrlBuilder(httpContext.Request);
            string htmlString = html.ToHtmlString();
            string replacer = "href=\"";
            if (htmlString.Contains("href=\"/"))
            {
                replacer = "href=\"/";
            }
            htmlString = htmlString.Replace(replacer, string.Concat("href=\"", canvasUrl.FacebookAppRootUrl, "/"));
            return MvcHtmlString.Create(htmlString);
        }

    }
}
