namespace Facebook.Web.Mvc
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class NFacebookAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public NFacebookAuthorizeAttribute()
        {
            this.Order = 1;
        }

        public string Permissions { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Contract.Requires(filterContext != null);
            Contract.Requires(filterContext.Controller != null);
            Contract.Requires(filterContext.Controller.ViewData != null);

            var settings = (IFacebookAppSettings)filterContext.Controller.ViewData["facebooksdk-currentappsettings"];

            OnAuthorization(filterContext, settings);
        }

        public abstract void OnAuthorization(AuthorizationContext filterContext, IFacebookAppSettings settings);
    }
}