namespace Facebook.Web.Mvc
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class NFacebookAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        public string Permissions { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Contract.Requires(filterContext != null);
            Contract.Requires(filterContext.HttpContext != null);
            Contract.Requires(filterContext.HttpContext.Items != null);

            OnAuthorization(filterContext, FacebookContext.Current);
        }

        public abstract void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication settings);
    }
}