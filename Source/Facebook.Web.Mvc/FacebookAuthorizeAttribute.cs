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

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(LoginUrl ?? "/");
        }
    }
}
