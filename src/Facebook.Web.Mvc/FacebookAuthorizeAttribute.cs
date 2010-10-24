// --------------------------------
// <copyright file="CanvasAuthorizeAttribute.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
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
    public abstract class FacebookAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        private FacebookApp _facebookApp;

        /// <summary>
        /// Gets the facebook app.
        /// </summary>
        /// <value>The facebook app.</value>
        public FacebookApp FacebookApp
        {
            get { return this._facebookApp; }
        }

        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        /// <value>The perms.</value>
        public string Perms { get; set; }
        /// <summary>
        /// Gets or sets the cancel URL path.
        /// </summary>
        /// <value>The cancel URL path.</value>
        public string CancelUrlPath { get; set; }
        /// <summary>
        /// Gets or sets the return URL path.
        /// </summary>
        /// <value>The return URL path.</value>
        public string ReturnUrlPath { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthorizeAttribute"/> class.
        /// </summary>
        protected FacebookAuthorizeAttribute()
        {
            _facebookApp = new FacebookApp();
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(_facebookApp != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="facebookApp">The facebook app.</param>
        protected FacebookAuthorizeAttribute(FacebookApp facebookApp)
        {
            if (facebookApp == null)
            {
                throw new ArgumentNullException("facebookApp");
            }
            this._facebookApp = facebookApp;
        }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // The user is inside the iframe, now we need to check to make
            // sure they are authenticed.
            var isAuthorized = AuthorizeCore(filterContext.HttpContext);
            if (!isAuthorized)
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        /// <summary>
        /// Authorizes the core.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            Contract.Requires(httpContext != null);
            Contract.EndContractBlock();

            bool authenticated = _facebookApp.Session != null;
            if (authenticated && !string.IsNullOrEmpty(Perms))
            {
                var requiredPerms = Perms.Split(',');
                var currentPerms = GetCurrentPerms(Perms);
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
        /// Called by the MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.ActionParameters = filterContext.ActionParameters ?? new Dictionary<string, object>();

            if (_facebookApp.Session != null)
            {
                filterContext.ActionParameters["FacebookId"] = _facebookApp.Session.UserId;
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Gets the login url for the current request.
        /// </summary>
        /// <param name="filterContext">The current AuthorizationContext.</param>
        /// <returns>The cancel url.</returns>
        protected Uri GetLoginUrl(AuthorizationContext filterContext)
        {
            return GetLoginUrl(filterContext, false);
        }

        /// <summary>
        /// Gets the login url for the current request.
        /// </summary>
        /// <param name="filterContext">The current AuthorizationContext.</param>
        /// <param name="cancelToSelf">Should the cancel url return to this same action. (Only do this on soft authorize, otherwise you will get an infinate loop.)</param>
        /// <returns>The cancel url.</returns>
        protected virtual Uri GetLoginUrl(AuthorizationContext filterContext, bool cancelToSelf)
        {
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(filterContext.HttpContext.Request);
            return urlBuilder.GetLoginUrl(_facebookApp, Perms, ReturnUrlPath, CancelUrlPath, cancelToSelf);
        }

        /// <summary>
        /// Handles the unauthorized request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            Contract.Requires(filterContext != null);

            var url = GetLoginUrl(filterContext);
            filterContext.Result = new RedirectResult(url.ToString());
        }

        /// <summary>
        /// Gets the current user's permission using FQL.
        /// </summary>
        /// <remarks>
        /// A better method would be to cache the permission in your local database and subscribe to 
        /// real time updates of user permissions: http://developers.facebook.com/docs/api/realtime</remarks>
        /// <param name="perms">The permission to check.</param>
        /// <returns></returns>
        protected virtual string[] GetCurrentPerms(string perms)
        {
            Contract.Requires(!String.IsNullOrEmpty(perms));
            Contract.Ensures(Contract.Result<string[]>() != null);

            var authUtil = new Authorizer(this._facebookApp);
            var requiredPerms = Perms.Replace(" ", String.Empty).Split(',');
            return authUtil.HasPermissions(requiredPerms);
        }
    }
}
