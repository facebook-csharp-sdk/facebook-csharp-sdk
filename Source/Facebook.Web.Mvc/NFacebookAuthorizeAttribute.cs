namespace Facebook.Web.Mvc
{
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NFacebookAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public NFacebookAuthorizeAttribute()
        {
            this.Order = 1;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var settings = filterContext.Controller.ViewData["facebooksdk-appsettings"];
        }
    }
}