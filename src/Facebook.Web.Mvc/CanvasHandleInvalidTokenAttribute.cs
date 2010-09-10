using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CanvasHandleInvalidTokenAttribute : HandleInvalidTokenAttribute
    {
        protected override void RedirectToFacebookLogin(System.Web.Mvc.ExceptionContext filterContext)
        {
            FacebookApp app = new FacebookApp();
            var loginUrl = app.GetLoginUrl();
            filterContext.Result = new CanvasRedirectResult(loginUrl.ToString());
        }
    }
}
