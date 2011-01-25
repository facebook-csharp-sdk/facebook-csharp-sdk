namespace Facebook.Web.Mvc
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class NFacebookAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        internal const string HttpContextCurrentAppNameKey = "facebooksdk-currentappname";
        internal const string HttpContextCurrentAppSettingsKey = "facebooksdk-currentappsettings";

        public NFacebookAuthorizeAttribute()
        {
            this.Order = 1;
        }

        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        public string Permissions { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Contract.Requires(filterContext != null);
            Contract.Requires(filterContext.HttpContext != null);
            Contract.Requires(filterContext.HttpContext.Items != null);

            var settings = (IFacebookAppSettings)filterContext.HttpContext.Items[HttpContextCurrentAppSettingsKey];
            
            OnAuthorization(filterContext, settings);
        }

        public abstract void OnAuthorization(AuthorizationContext filterContext, IFacebookAppSettings settings);
    }
}