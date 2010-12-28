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
        /// Initializes a new instance of the <see cref="FacebookAuthorizeAttributeBase"/> class.
        /// </summary>
        protected FacebookAuthorizeAttributeBase()
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
        /// Initializes a new instance of the <see cref="FacebookAuthorizeAttributeBase"/> class.
        /// </summary>
        /// <param name="facebookApp">The facebook app.</param>
        protected FacebookAuthorizeAttributeBase(FacebookApp facebookApp)
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
        /// Handles the unauthorized request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected abstract void HandleUnauthorizedRequest(AuthorizationContext filterContext);

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
