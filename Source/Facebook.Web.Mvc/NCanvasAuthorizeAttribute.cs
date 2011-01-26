/*namespace Facebook.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web;

    public class NCanvasAuthorizeAttribute : NFacebookAuthorizeAttribute
    {
        public string LoginDisplayMode { get; set; }

        public string CancelUrlPath { get; set; }

        public string ReturnUrlPath { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext, IFacebookAppSettings settings)
        {
            if (string.IsNullOrEmpty(this.Permissions))
            {
                return;
            }

            var appName = filterContext.HttpContext.Items["facebooksdk-currentappname"];

            if (settings == null)
            {
                if (appName == null)
                {
                    throw new ApplicationException("Facebook Application Settings not found. Did you miss [FacebookApp] attribute?");
                }
                else
                {
                    throw new ApplicationException(string.Format("Facebook Application Settings for '{0}' not found.", appName));
                }
            }

            if (string.IsNullOrEmpty(settings.AppId))
            {
                throw new ApplicationException(string.Format("Facebook Application ID has not been specified for '{0}.", appName));
            }

            if (string.IsNullOrEmpty(settings.AppSecret))
            {
                throw new ApplicationException(string.Format("Facebook Application Secret has not been specified for '{0}.", appName));
            }

            var authorizer = new Authorizer(settings.AppId, settings.AppSecret, filterContext.HttpContext) { Perms = this.Permissions };

            if (!authorizer.IsAuthorized())
            {
                this.HandleUnauthorizedRequest(filterContext, settings);
            }
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext, IFacebookAppSettings settings)
        {
            Contract.Requires(filterContext != null);
            Contract.Requires(settings != null);

            var loginUri = this.GetLoginUrl(settings, filterContext.HttpContext, null);
            filterContext.Result = new CanvasRedirectResult(loginUri.ToString());
        }

        internal virtual protected Uri GetLoginUrl(IFacebookAppSettings settings, HttpContextBase httpContext, IDictionary<string, object> parameters)
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

            return authorizer.GetLoginUrl(null);
        }
    }
}*/