﻿// --------------------------------
// <copyright file="FacebookAuthorizeAttributeBase.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Represents the base class for restricting access to controllers or actions based on Facebook permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class FacebookAuthorizeAttributeBase : ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        [System.Obsolete("Perms is marked for removal in future version. Use Permissions instead.")]
        public string Perms
        {
            get { return this.Permissions; }
            set { this.Permissions = value; }
        }

        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        public string Permissions { get; set; }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            this.OnAuthorization(filterContext, FacebookApplication.Current);
        }

        public abstract void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication facebookApplication);

        /*
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // redirect to facebook login
            var oauth = new FacebookOAuthClientAuthorizer
            {
                ClientId = this.AppId,
                ClientSecret = this.AppSecret,
                // set the redirect_uri
            };

            var parameters = new Dictionary<string, object>();
            parameters["display"] = this.LoginDisplayMode;

            if (!string.IsNullOrEmpty(this.Perms))
            {
                parameters["scope"] = this.Perms;
            }

            if (!string.IsNullOrEmpty(this.State))
            {
                parameters["state"] = this.State;
            }

            var loginUrl = oauth.GetLoginUri(parameters);
            filterContext.HttpContext.Response.Redirect(loginUrl.ToString());
        } */
    }
}