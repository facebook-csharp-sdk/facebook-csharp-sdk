using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Facebook.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HandleInvalidTokenAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception.GetType() == typeof(FacebookOAuthException))
            {
                filterContext.ExceptionHandled = true;
                RedirectToFacebookLogin(filterContext);
            }
        }

        protected virtual void RedirectToFacebookLogin(ExceptionContext filterContext)
        {
            FacebookApp app = new FacebookApp();
            var loginUrl = app.GetLoginUrl();
            filterContext.Result = new RedirectResult(loginUrl.ToString());
        }
    }
}
