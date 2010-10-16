// --------------------------------
// <copyright file="CanvasAuthorizeAttribute.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
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
    public class CanvasAuthorizeAttribute : FacebookAuthorizeAttribute
    {
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
