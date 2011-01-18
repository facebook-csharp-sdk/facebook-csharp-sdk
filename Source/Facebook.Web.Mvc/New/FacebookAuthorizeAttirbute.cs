
namespace Facebook.Web.Mvc.New
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Facebook.Web.New;

    /// <summary>
    /// Represents the base class for restricting access to controllers or actions based on Facebook permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class FacebookAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// The Facebook Settings (includes appid and appsecret).
        /// </summary>
        private readonly IFacebookSettings facebookSettings;

        /// <summary>
        /// The facebook session.
        /// </summary>
        private FacebookSession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="facebookSettings">
        /// The facebook settings.
        /// </param>
        public FacebookAuthorizeAttribute(IFacebookSettings facebookSettings)
        {
            this.facebookSettings = facebookSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthorizeAttribute"/> class.
        /// </summary>
        public FacebookAuthorizeAttribute()
            : this(Facebook.FacebookSettings.Current)
        {
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
        /// Gets the Facebook Settings (includes appid and appsecret).
        /// </summary>
        public IFacebookSettings FacebookSettings
        {
            get
            {
                return this.facebookSettings;
            }
        }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            var authorizer = new Authorizer(this.facebookSettings, filterContext.HttpContext);
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
                ClientId = this.FacebookSettings.AppId,
                ClientSecret = this.FacebookSettings.AppSecret,
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