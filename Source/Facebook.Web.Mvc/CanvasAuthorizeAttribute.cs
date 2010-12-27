// --------------------------------
// <copyright file="CanvasAuthorizeAttribute.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Web.Mvc;

namespace Facebook.Web.Mvc
{
    /// <summary>
    /// Provides funcationality for restricting access to controllers or actions based on Facebook permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
    public class CanvasAuthorizeAttribute : FacebookAuthorizeAttributeBase
    {

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
        /// Initializes a new instance of the <see cref="CanvasAuthorizeAttribute"/> class.
        /// </summary>
        public CanvasAuthorizeAttribute() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="facebookApp">The facebook app.</param>
        public CanvasAuthorizeAttribute(FacebookApp facebookApp) :base(facebookApp) { }

        /// <summary>
        /// Gets the login url for the current request.
        /// </summary>
        /// <param name="filterContext">The current AuthorizationContext.</param>
        /// <returns>The cancel url.</returns>
        protected Uri GetLoginUrl(AuthorizationContext filterContext)
        {
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(filterContext.HttpContext.Request);
            return urlBuilder.GetLoginUrl(this.FacebookApp, Perms, ReturnUrlPath, CancelUrlPath);
        }

        /// <summary>
        /// Handles the unauthorized request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var url = GetLoginUrl(filterContext);
            filterContext.Result = new CanvasRedirectResult(url.ToString());
        }

    }
}
