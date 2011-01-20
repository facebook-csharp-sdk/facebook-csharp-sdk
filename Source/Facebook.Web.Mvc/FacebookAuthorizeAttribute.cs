
namespace Facebook.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the base class for restricting access to controllers or actions based on Facebook permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class FacebookAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {

        /// <summary>
        /// The facebook session.
        /// </summary>
        private FacebookSession session;

        public string AppId { get; set; }
        public string AppSecret { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="facebookSettings">
        /// The facebook settings.
        /// </param>
        public FacebookAuthorizeAttribute(string appId, string appSecret)
        {
            Contract.Requires(!String.IsNullOrEmpty(appId));
            Contract.Requires(!String.IsNullOrEmpty(appSecret));

            this.AppId = appId;
            this.AppSecret = appSecret;
        }

        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        public string Perms { get; set; }

        /// <summary>
        /// Gets or sets the login display mode.
        /// </summary>
        public string LoginDisplayMode { get; set; }

        /// <summary>
        /// Gets or sets an opaque state used to maintain application state between the request and callback.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the cancel url path.
        /// </summary>
        public string CancelUrlPath { get; set; }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            var authorizer = new Authorizer(this.AppId, this.AppSecret, filterContext.HttpContext);
            if (!authorizer.IsAuthorized())
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }

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
        }
    }
}