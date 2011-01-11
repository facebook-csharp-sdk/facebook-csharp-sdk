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
        /// Handles the unauthorized request.
        /// </summary>
        /// <param name="facebookApp">The current Facebook app instance.</param>
        /// <param name="filterContext">The filter context.</param>
        protected override void HandleUnauthorizedRequest(FacebookApp facebookApp, AuthorizationContext filterContext)
        {
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(filterContext.HttpContext.Request);
            var url = urlBuilder.GetLoginUrl(facebookApp, Perms, ReturnUrlPath, CancelUrlPath, false);
            filterContext.Result = new CanvasRedirectResult(url.ToString());
        }

    }
}
