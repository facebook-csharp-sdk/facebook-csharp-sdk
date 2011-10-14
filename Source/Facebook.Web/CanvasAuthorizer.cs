// --------------------------------
// <copyright file="CanvasAuthorizer.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// Represents the authorizer for the Canvas Applications.
    /// </summary>
    public class CanvasAuthorizer : FacebookWebAuthorizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasAuthorizer"/> class. 
        /// </summary>
        public CanvasAuthorizer()
            : base(FacebookWebContext.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasAuthorizer"/> class. 
        /// </summary>
        /// <param name="context">The <see cref="FacebookWebContext"/></param>
        public CanvasAuthorizer(FacebookWebContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasAuthorizer"/> class. 
        /// </summary>
        /// <param name="settings">The Facebook application settings.</param>
        /// <param name="httpContext">The HttpContext.</param>
        public CanvasAuthorizer(IFacebookApplication settings, HttpContextBase httpContext)
            : base(settings, httpContext)
        {
        }

        /// <summary>
        /// Handle unauthorized requests.
        /// </summary>
        public override void HandleUnauthorizedRequest()
        {
            FacebookWebRequest.HttpContext.Response.ContentType = "text/html";
            FacebookWebRequest.HttpContext.Response.Write(CanvasUrlBuilder.GetCanvasRedirectHtml(GetLoginUrl(null)));
        }

        /// <summary>
        /// Gets the canvas login url.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the login url.
        /// </returns>
        public Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            var defaultParameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(LoginDisplayMode))
            {
                defaultParameters["display"] = LoginDisplayMode;
            }

            if (Permissions != null)
            {
                defaultParameters["scope"] = string.Join(",", Permissions);
            }

            var canvasUrlBuilder = new CanvasUrlBuilder(FacebookWebRequest.Settings, FacebookWebRequest.HttpContext.Request);
            return canvasUrlBuilder.GetLoginUrl(ReturnUrlPath, CancelUrlPath, State, FacebookUtils.Merge(defaultParameters, parameters));
        }

    }
}
