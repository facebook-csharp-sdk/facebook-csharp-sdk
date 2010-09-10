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
using System.Diagnostics.Contracts;

namespace Facebook.Web.Mvc
{
    public static class CanvasLinkExtensions
    {

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, htmlAttributes);
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrEmpty(linkText))
            {
                throw new ArgumentNullException("linkText");
            }
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null/* routeName */, actionName, controllerName, routeValues, htmlAttributes));
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasActionLink(htmlHelper, linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrEmpty(linkText))
            {
                throw new ArgumentNullException("linkText");
            }
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null /* routeName */, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, object routeValues)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasRouteLink(htmlHelper, linkText, new RouteValueDictionary(routeValues));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary routeValues)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasRouteLink(htmlHelper, linkText, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasRouteLink(htmlHelper, linkText, routeName, (object)null /* routeValues */ );
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasRouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasRouteLink(htmlHelper, linkText, routeName, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, object routeValues, object htmlAttributes)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasRouteLink(htmlHelper, linkText, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasRouteLink(htmlHelper, linkText, null /* routeName */, routeValues, htmlAttributes);
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues, object htmlAttributes)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasRouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrEmpty(linkText))
            {
                throw new ArgumentNullException("linkText");
            }
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return MvcHtmlString.Create(GenerateRouteLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, routeName, routeValues, htmlAttributes));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            Contract.Requires(!String.IsNullOrEmpty(linkText));
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return CanvasRouteLink(htmlHelper, linkText, routeName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (String.IsNullOrEmpty(linkText))
            {
                throw new ArgumentNullException("linkText");
            }
            Contract.Requires(htmlHelper != null);
            Contract.Requires(htmlHelper.ViewContext != null);

            return MvcHtmlString.Create(GenerateRouteLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, routeName, protocol, hostName, fragment, routeValues, htmlAttributes));
        }

        private static string GenerateLink(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return GenerateLink(requestContext, routeCollection, linkText, routeName, actionName, controllerName, null/* protocol */, null/* hostName */, null/* fragment */, routeValues, htmlAttributes);
        }

        private static string GenerateLink(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return GenerateLinkInternal(requestContext, routeCollection, linkText, routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes, true /* includeImplicitMvcValues */);
        }

        private static string GenerateRouteLink(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return GenerateRouteLink(requestContext, routeCollection, linkText, routeName, null/* protocol */, null/* hostName */, null/* fragment */, routeValues, htmlAttributes);
        }

        private static string GenerateRouteLink(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return GenerateLinkInternal(requestContext, routeCollection, linkText, routeName, null /* actionName */, null /* controllerName */, protocol, hostName, fragment, routeValues, htmlAttributes, false /* includeImplicitMvcValues */);
        }

        private static string GenerateLinkInternal(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool includeImplicitMvcValues)
        {
            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
            htmlAttributes["target"] = "_top";
            var canvasSettings = CanvasSettings.Current;
            if (canvasSettings == null || canvasSettings.CanvasPageUrl == null)
            {
                throw new ConfigurationErrorsException("The canvas settings were not found or are invalid in the application configuration file.");
            }
            var canvasPageUrl = canvasSettings.CanvasPageUrl.ToString();
            string url = String.Concat(canvasPageUrl, UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, includeImplicitMvcValues));
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = (!String.IsNullOrEmpty(linkText)) ? HttpUtility.HtmlEncode(linkText) : String.Empty
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

    }
}
