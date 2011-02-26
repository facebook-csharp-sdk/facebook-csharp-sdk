﻿// --------------------------------
// <copyright file="CanvasAuthorizeAttribute.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web;

    public class CanvasAuthorizeAttribute : FacebookAuthorizeAttributeBase
    {
        public string LoginDisplayMode { get; set; }

        public string CancelUrlPath { get; set; }

        public string ReturnUrlPath { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication settings)
        {
            var authorizer = new FacebookWebRequest(settings, filterContext.HttpContext);

            if (!authorizer.IsAuthorized(this.Permissions))
            {
                this.HandleUnauthorizedRequest(filterContext, FacebookContext.Current);
            }
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext, IFacebookApplication settings)
        {
            Contract.Requires(filterContext != null);
            Contract.Requires(settings != null);

            var loginUri = this.GetLoginUrl(settings, filterContext.HttpContext, null);
            filterContext.Result = new CanvasRedirectResult(loginUri.ToString());
        }

        internal virtual protected Uri GetLoginUrl(IFacebookApplication settings, HttpContextBase httpContext, IDictionary<string, object> parameters)
        {
            Contract.Requires(settings != null);
            Contract.Requires(httpContext != null);

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

            return authorizer.GetLoginUrl(parameters);
        }
    }
}