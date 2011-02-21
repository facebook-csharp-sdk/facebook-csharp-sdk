using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Facebook.Web.Mvc
{
    public class FacebookAuthorizeAttribute : FacebookAuthorizeAttributeBase
    {

        public string LoginUrl { get; set; }

        /// <summary>
        /// Handles the unauthorized request.
        /// </summary>
        /// <param name="facebookApp">The current Facebook App instance.</param>
        /// <param name="filterContext">The filter context.</param>
        protected override void HandleUnauthorizedRequest(FacebookApp facebookApp, System.Web.Mvc.AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(LoginUrl ?? "/");
        }
    }
}
