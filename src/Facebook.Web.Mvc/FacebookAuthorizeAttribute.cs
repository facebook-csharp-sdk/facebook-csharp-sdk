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

namespace Facebook.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class FacebookAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        private FacebookApp app;

        public FacebookApp App
        {
            get { return this.app; }
        }

        public string Perms { get; set; }
        public string CancelUrlPath { get; set; }
        public string ReturnUrlPath { get; set; }


        public FacebookAuthorizeAttribute()
        {
            app = new FacebookApp();
        }

        public FacebookAuthorizeAttribute(FacebookApp app)
        {
            this.app = app;
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
            bool authenticated = app.Session != null;
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
            if (app.Session != null)
            {
                filterContext.ActionParameters["FacebookId"] = app.Session.UserId;
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Gets the login url for the current request.
        /// </summary>
        /// <param name="filterContext">The current AuthorizationContext.</param>
        /// <param name="cancelToSelf">Should the cancel url return to this same action. (Only do this on soft authorize, otherwise you will get an infinate loop.)</param>
        /// <returns>The cancel url.</returns>
        protected virtual string GetLoginUrl(AuthorizationContext filterContext, bool cancelToSelf = false)
        {
            FacebookUrlBuilder facebookUrl = new FacebookUrlBuilder(filterContext.HttpContext.Request);
            dynamic parameters = new ExpandoObject();
            parameters.req_perms = Perms;
            parameters.canvas = 1;

            // set the return url
            Uri returnUrl;
            if (!string.IsNullOrEmpty(ReturnUrlPath))
            {
                returnUrl = facebookUrl.BuildAuthReturnUrl(ReturnUrlPath);
            }
            else
            {
                returnUrl = facebookUrl.BuildAuthReturnUrl();
            }
            parameters.next = returnUrl.ToString();


            // set the cancel url
            Uri cancelUrl;
            if (!string.IsNullOrEmpty(CancelUrlPath))
            {
                cancelUrl = facebookUrl.BuildAuthReturnUrl(CancelUrlPath);
            }
            else if (CanvasSettings.Current.AuthorizeCancelUrl != null)
            {
                cancelUrl = CanvasSettings.Current.AuthorizeCancelUrl;
            }
            else
            {
                if (cancelToSelf)
                {
                    cancelUrl = facebookUrl.BuildAuthCancelUrl();
                }
                else
                {
                    // Cancel url is facebook.com
                    cancelUrl = new Uri("http://www.facebook.com");
                }

            }
            parameters.cancel_url = cancelUrl.ToString();

            Uri uri = app.GetLoginUrl(parameters);
            return uri.ToString();
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var url = GetLoginUrl(filterContext);
            filterContext.Result = new RedirectResult(url);
        }

        /// <summary>
        /// Gets the current user's permission using FQL. A better method 
        /// would be to cache the permission in your local database and subscribe to 
        /// real time updates of user permissions: http://developers.facebook.com/docs/api/realtime
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        protected virtual string[] GetCurrentPerms(string perms)
        {
            if (app.Session == null)
            {
                throw new ArgumentException("A valid session must be present to get current permissions.");
            }
            if (string.IsNullOrEmpty(perms))
            {
                throw new ArgumentNullException("perms");
            }

            dynamic data = app.Fql(string.Format("SELECT {0} FROM permissions WHERE uid == {1}", perms, app.UserId));
            if (data.Count == 0)
            {
                return new string[0];
            }

            var permData = (IDictionary<string, object>)data[0];
            return (from perm in permData
                    where perm.Value.ToString() == "1"
                    select perm.Key).ToArray();
        }
    }
}
