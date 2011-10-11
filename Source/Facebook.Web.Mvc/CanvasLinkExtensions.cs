// --------------------------------
// <copyright file="CanvasLinkExtensions.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Provides extensions for building canvas links.
    /// </summary>
    public static class CanvasLinkExtensions
    {
        /// <summary>
        /// Builds a canvas action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(), new RouteValueDictionary());
        }

        /// <summary>
        /// Builds a canvas action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        /// <summary>
        /// Builds a canvas action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Builds a canvas action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, new RouteValueDictionary());
        }

        /// <summary>
        /// Builds a canvas action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Builds a canvas action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        /// <summary>
        /// Builds a canvas action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Builds a canvas action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null/* routeName */, actionName, controllerName, routeValues, htmlAttributes));
        }

        /// <summary>
        /// Builds a canvas action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="fragment">The fragment.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return CanvasActionLink(htmlHelper, linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Builds a canvas action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="fragment">The fragment.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null /* routeName */, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes));
        }

        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, object routeValues)
        {
            return CanvasRouteLink(htmlHelper, linkText, new RouteValueDictionary(routeValues));
        }


        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary routeValues)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeValues, new RouteValueDictionary());
        }

        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeName, (object)null /* routeValues */ );
        }

        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeName, routeValues, new RouteValueDictionary());
        }

        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, object routeValues, object htmlAttributes)
        {
            return CanvasRouteLink(htmlHelper, linkText, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return CanvasRouteLink(htmlHelper, linkText, null /* routeName */, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues, object htmlAttributes)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return MvcHtmlString.Create(GenerateRouteLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, routeName, routeValues, htmlAttributes));
        }

        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="fragment">The fragment.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return CanvasRouteLink(htmlHelper, linkText, routeName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Builds a canvas route link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="fragment">The fragment.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CanvasRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return MvcHtmlString.Create(GenerateRouteLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, routeName, protocol, hostName, fragment, routeValues, htmlAttributes));
        }

        internal static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            RouteValueDictionary result = new RouteValueDictionary();

            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    result.Add(property.Name.Replace('_', '-'), property.GetValue(htmlAttributes));
                }
            }

            return result;
        }

        /// <summary>
        /// Generates the link.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="routeCollection">The route collection.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        private static string GenerateLink(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return GenerateLink(requestContext, routeCollection, linkText, routeName, actionName, controllerName, null/* protocol */, null/* hostName */, null/* fragment */, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Generates the link.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="routeCollection">The route collection.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="fragment">The fragment.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        private static string GenerateLink(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return GenerateLinkInternal(requestContext, routeCollection, linkText, routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes, true /* includeImplicitMvcValues */);
        }

        /// <summary>
        /// Generates the route link.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="routeCollection">The route collection.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        private static string GenerateRouteLink(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return GenerateRouteLink(requestContext, routeCollection, linkText, routeName, null/* protocol */, null/* hostName */, null/* fragment */, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Generates the route link.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="routeCollection">The route collection.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="fragment">The fragment.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        private static string GenerateRouteLink(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return GenerateLinkInternal(requestContext, routeCollection, linkText, routeName, null /* actionName */, null /* controllerName */, protocol, hostName, fragment, routeValues, htmlAttributes, false /* includeImplicitMvcValues */);
        }

        /// <summary>
        /// Generates the link.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="routeCollection">The route collection.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="fragment">The fragment.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="includeImplicitMvcValues">if set to <c>true</c> [include implicit MVC values].</param>
        /// <returns></returns>
        private static string GenerateLinkInternal(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool includeImplicitMvcValues)
        {
            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
            htmlAttributes["target"] = "_top";
            string webUrl = UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, includeImplicitMvcValues);
            var applicationPath = requestContext.HttpContext.Request.ApplicationPath;

            if (!string.IsNullOrEmpty(applicationPath) && applicationPath != "/" && webUrl.StartsWith(applicationPath))
            {
                webUrl = webUrl.Substring(applicationPath.Length);
            }

            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(FacebookApplication.Current, requestContext.HttpContext.Request);
            string url = urlBuilder.BuildCanvasPageUrl(webUrl).ToString();
            var tagBuilder = new TagBuilder("a")
            {
                InnerHtml = (!String.IsNullOrEmpty(linkText)) ? HttpUtility.HtmlEncode(linkText) : String.Empty
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }
    }
}
