﻿// --------------------------------
// <copyright file="CanvasSoftAuthorizeAttribute.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web;
    using System;

    /// <summary>
    /// This filter will send an unauthorized user to the 
    /// specified view rather than redirecting them directly
    /// to the facebook login page. This allows for a landing
    /// page to explain to the user why the permissions requested
    /// are needed.
    /// </summary>
    public class CanvasSoftAuthorizeAttribute : CanvasAuthorizeAttribute
    {
        /// <summary>
        /// The default view.
        /// </summary>
        private const string DefaultView = "FacebookAuthorize";

        /// <summary>
        /// The view name.
        /// </summary>
        private string view;

        /// <summary>
        /// The master.
        /// </summary>
        private string master;

        /// <summary>
        /// Gets or sets the name of the View.
        /// </summary>
        public string View
        {
            get { return !string.IsNullOrEmpty(this.view) ? this.view : DefaultView; }
            set { this.view = value; }
        }

        /// <summary>
        /// Gets or sets the Master.
        /// </summary>
        public string Master
        {
            get { return this.master ?? string.Empty; }
            set { this.master = value; }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext, IFacebookApplication settings)
        {
            Contract.Requires(filterContext != null);
            Contract.Requires(filterContext.HttpContext != null);
            Contract.Requires(filterContext.HttpContext.Request != null);
            Contract.Requires(filterContext.RouteData != null);
            Contract.Requires(filterContext.Controller != null);
            Contract.Requires(settings != null);

            var model = new FacebookAuthorizeInfo(
                this.GetLoginUrl(settings, filterContext.HttpContext, null),
                this.Perms,
                filterContext.HttpContext.Request.QueryString.AllKeys.Contains("error_reason"),
                filterContext.RouteData.Values);

            var viewResult = new ViewResult
                                       {
                                           MasterName = this.Master,
                                           ViewName = this.View,
                                           ViewData = new ViewDataDictionary<FacebookAuthorizeInfo>(model),
                                           TempData = filterContext.Controller.TempData
                                       };

            filterContext.Result = viewResult;
        }

        protected internal override System.Uri GetLoginUrl(IFacebookApplication settings, HttpContextBase httpContext, IDictionary<string, object> parameters)
        {
            var authorizer = new FacebookCanvasAuthorizer(settings, httpContext)
            {
                ReturnUrlPath = this.ReturnUrlPath,
                CancelUrlPath = this.CancelUrlPath,
                LoginDisplayMode = this.LoginDisplayMode
            };

            if (!String.IsNullOrEmpty(this.Permissions))
            {
                authorizer.Permissions = this.Permissions.Replace(" ", String.Empty).Split(',');
            }

            if (string.IsNullOrEmpty(this.CancelUrlPath))
            {
                // set it to this same url instead of going to facebook.com
                var canvasUrlBuilder = new CanvasUrlBuilder(settings, httpContext.Request);
                var currentPathAndQuery = canvasUrlBuilder.CurrentCanvasPathAndQuery;

                if (currentPathAndQuery.Contains("?"))
                {
                    var parts = currentPathAndQuery.Split('?');
                    if (parts.Length == 2 && !string.IsNullOrEmpty(parts[1]))
                    {
                        var queryStrings = FacebookUtils.ParseUrlQueryString(parts[1]);

                        // remove oauth 2 error querystrings.
                        // error_reason=user_denied&error_denied=access_denied&error_description=The+user+denied+your+request.
                        if (queryStrings.ContainsKey("error_reason"))
                        {
                            queryStrings.Remove("error_reason");
                        }

                        if (queryStrings.ContainsKey("error_denied"))
                        {
                            queryStrings.Remove("error_denied");
                        }

                        if (queryStrings.ContainsKey("error_description"))
                        {
                            queryStrings.Remove("error_description");
                        }

                        currentPathAndQuery = parts[0] + "?" + FacebookUtils.ToJsonQueryString(queryStrings);
                    }
                }

                authorizer.CancelUrlPath = currentPathAndQuery;
            }

            return authorizer.GetLoginUrl(null);
        }
    }
}