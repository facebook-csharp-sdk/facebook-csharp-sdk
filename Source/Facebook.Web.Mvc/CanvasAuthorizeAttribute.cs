namespace Facebook.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web;

    public class CanvasAuthorizeAttribute : FacebookAuthorizeAttributeBase
    {
        public string LoginDisplayMode { get; set; }

        public string CancelUrlPath { get; set; }

        public string ReturnUrlPath { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication settings)
        {
            var authorizer = new Authorizer(settings, filterContext.HttpContext) { Perms = this.Permissions };

            if (!authorizer.IsAuthorized())
            {
                this.HandleUnauthorizedRequest(filterContext, FacebookContext.Current);
            }
        }
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext, IFacebookApplication settings)
        {
            Contract.Requires(filterContext != null);
            Contract.Requires(settings != null);

            var loginUri = this.GetLoginUrl(settings, filterContext.HttpContext, null);
            filterContext.Result = new CanvasRedirectResult(loginUri.ToString());
        }

        internal virtual protected Uri GetLoginUrl(IFacebookApplication settings, HttpContextBase httpContext, IDictionary<string, object> parameters)
        {
            Contract.Requires(settings != null);
            Contract.Requires(httpContext != null);

            var authorizer = new CanvasAuthorizer(settings, httpContext)
            {
                Perms = this.Permissions,
                ReturnUrlPath = this.ReturnUrlPath,
                CancelUrlPath = this.CancelUrlPath,
                LoginDisplayMode = this.LoginDisplayMode
            };

            return authorizer.GetLoginUrl(parameters);
        }
    }
}