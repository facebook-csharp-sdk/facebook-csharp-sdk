// --------------------------------
// <copyright file="CanvasAuthorizeAttribute.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facebook.Web.Mvc
{
    /// <summary>
    /// Represents the base class for restricting access to controllers or actions based on Facebook permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class FacebookAuthorizeAttributeBase : ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        /// <value>The perms.</value>
        public string Perms { get; set; }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // The user is inside the iframe, now we need to check to make
            // sure they are authenticed.
            var facebookApp = new FacebookApp();
            var isAuthorized = AuthorizeCore(facebookApp, filterContext.HttpContext);
            if (!isAuthorized)
            {
                HandleUnauthorizedRequest(facebookApp, filterContext);
            }
        }

        /// <summary>
        /// Authorizes the core.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        protected virtual bool AuthorizeCore(FacebookApp facebookApp, HttpContextBase httpContext)
        {
            Contract.Requires(httpContext != null);
            Contract.EndContractBlock();

            bool authenticated = facebookApp.Session != null;
            if (authenticated && !string.IsNullOrEmpty(Perms))
            {
                var requiredPerms = Perms.Split(',');
                var currentPerms = GetCurrentPerms(facebookApp, Perms);
                foreach (var perm in requiredPerms)
                {
                    if (!currentPerms.Contains(perm))
                    {
                        return false;
                    }
                }
            }
            return authenticated;
        }

        /// <summary>
        /// Handles the unauthorized request.
        /// </summary>
        /// <param name="facebookApp">The current Facebook App instance.</param>
        /// <param name="filterContext">The filter context.</param>
        protected abstract void HandleUnauthorizedRequest(FacebookApp facebookApp, AuthorizationContext filterContext);

        /// <summary>
        /// Gets the current user's permission using FQL.
        /// </summary>
        /// <remarks>
        /// A better method would be to cache the permission in your local database and subscribe to 
        /// real time updates of user permissions: http://developers.facebook.com/docs/api/realtime</remarks>
        /// <param name="perms">The permission to check.</param>
        /// <returns></returns>
        protected virtual string[] GetCurrentPerms(FacebookApp facebookApp, string perms)
        {
            Contract.Requires(!String.IsNullOrEmpty(perms));
            Contract.Ensures(Contract.Result<string[]>() != null);

            var authUtil = new Authorizer(facebookApp);
            var requiredPerms = Perms.Replace(" ", String.Empty).Split(',');
            return authUtil.HasPermissions(requiredPerms);
        }
    }
}
