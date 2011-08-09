// --------------------------------
// <copyright file="FacebookAuthorizeAttributeBase.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
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
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public virtual string Perms
        {
            get { return Permissions; }
            set { Permissions = value; }
        }

        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        public virtual string Permissions { get; set; }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            OnAuthorization(filterContext, FacebookApplication.Current);
        }

        /// <summary>
        /// On Authorization.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="facebookApplication">The Facebook application settings.</param>
        public abstract void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication facebookApplication);

        /// <summary>
        /// Splits string to string array using comma.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>Array of strings.</returns>
        internal static string[] ToArrayString(string str)
        {
            return string.IsNullOrEmpty(str) ? null : str.Split(',');
        }

        /*
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // redirect to facebook login
            var oauth = new FacebookOAuthClientAuthorizer
            {
                ClientId = AppId,
                ClientSecret = AppSecret,
                // set the redirect_uri
            };

            var parameters = new Dictionary<string, object>();
            parameters["display"] = LoginDisplayMode;

            if (!string.IsNullOrEmpty(Perms))
            {
                parameters["scope"] = Perms;
            }

            if (!string.IsNullOrEmpty(State))
            {
                parameters["state"] = State;
            }

            var loginUrl = oauth.GetLoginUri(parameters);
            filterContext.HttpContext.Response.Redirect(loginUrl.ToString());
        } */
    }
}