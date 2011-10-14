// --------------------------------
// <copyright file="CanvasAuthorizeAttribute.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web;

    /// <summary>
    /// Represents the canvas authorize attribute.
    /// </summary>
    public class CanvasAuthorizeAttribute : FacebookAuthorizeAttributeBase
    {
        /// <summary>
        /// Gets or sets the login display mode.
        /// </summary>
        public virtual string LoginDisplayMode { get; set; }

        /// <summary>
        /// Gets or sets the cancel url path.
        /// </summary>
        public virtual string CancelUrlPath { get; set; }

        /// <summary>
        /// Gets or sets the return url path.
        /// </summary>
        public virtual string ReturnUrlPath { get; set; }

        /// <summary>
        /// Authorization.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="settings">The Facebook application settings.</param>
        /// <exception cref="ArgumentException">Throws if Permissions contains space.</exception>
        public override void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication settings)
        {
            var authorizer = new FacebookWebContext(settings, filterContext.HttpContext);

            if (!string.IsNullOrEmpty(Permissions) && Permissions.IndexOf(" ") != -1)
            {
                throw new ArgumentException("Permissions cannot contain whitespace.");
            }

            if (!authorizer.IsAuthorized(ToArrayString(Permissions)))
            {
                this.HandleUnauthorizedRequest(filterContext, FacebookApplication.Current);
            }
        }

        /// <summary>
        /// Handles unauthorized request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="settings">The Facebook application settings.</param>
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext, IFacebookApplication settings)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            if (settings == null)
                throw new ArgumentNullException("settings");

            var loginUri = this.GetLoginUrl(settings, filterContext.HttpContext, null);
            filterContext.Result = new CanvasRedirectResult(loginUri.ToString());
        }

        /// <summary>
        /// Gets the login url.
        /// </summary>
        /// <param name="settings">The Facebook application settings.</param>
        /// <param name="httpContext">The http context.</param>
        /// <param name="parameters">The login parameters.</param>
        /// <returns>The login url.</returns>
        internal virtual protected Uri GetLoginUrl(IFacebookApplication settings, HttpContextBase httpContext, IDictionary<string, object> parameters)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            var authorizer = new CanvasAuthorizer(settings, httpContext)
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