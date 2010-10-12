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
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Configuration;
using System.Web.Security;
using System.Dynamic;
using System.Web.Routing;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Facebook.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class FacebookAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        private FacebookApp _facebookApp;

        public FacebookApp FacebookApp
        {
            get { return this._facebookApp; }
        }

        public string Perms { get; set; }
        public string CancelUrlPath { get; set; }
        public string ReturnUrlPath { get; set; }


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

        protected FacebookAuthorizeAttribute(FacebookApp facebookApp)
        {
            if (facebookApp == null)
            {
                throw new ArgumentNullException("facebookApp");
            }
            this._facebookApp = facebookApp;
        }

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

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            Contract.Requires(filterContext != null);

            var url = GetLoginUrl(filterContext);
            filterContext.Result = new RedirectResult(url.ToString());
        }

        /// <summary>
        /// Gets the current user's permission using FQL. A better method 
        /// would be to cache the permission in your local database and subscribe to 
        /// real time updates of user permissions: http://developers.facebook.com/docs/api/realtime
        /// </summary>
        /// <param name="perms">The permission to check.</param>
        /// <returns></returns>
        protected virtual string[] GetCurrentPerms(string perms)
        {
            Contract.Requires(!String.IsNullOrEmpty(perms));
            Contract.Ensures(Contract.Result<string[]>() != null);

            var result = new string[0];
            if (_facebookApp.UserId != 0)
            {
                var query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM permissions WHERE uid == {1}", perms, _facebookApp.UserId);
                var parameters = new Dictionary<string, object>();
                parameters["query"] = query;
                parameters["method"] = "fql.query";
                parameters["access_token"] = string.Concat(_facebookApp.AppId, "|", _facebookApp.ApiSecret);
                var data = (JsonArray)_facebookApp.Api(parameters);
                if (data != null && data.Count > 0)
                {
                    var permData = data[0] as IDictionary<string, object>;
                    if (permData != null)
                    {
                        result = (from perm in permData
                                  where perm.Value.ToString() == "1"
                                  select perm.Key).ToArray();
                    }
                }
            }
            return result;
        }
    }
}
