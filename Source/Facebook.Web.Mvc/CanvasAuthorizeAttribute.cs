namespace Facebook.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Provides funcationality for restricting access to controllers or actions based on Facebook permissions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CanvasAuthorizeAttribute : FacebookAuthorizeAttribute
    {
        private readonly ICanvasSettings canvasSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="facebookSettings">
        /// The facebook settings.
        /// </param>
        public CanvasAuthorizeAttribute(IFacebookSettings facebookSettings, ICanvasSettings canvasSettings)
            : base(facebookSettings)
        {
            this.canvasSettings = canvasSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasAuthorizeAttribute"/> class.
        /// </summary>
        public CanvasAuthorizeAttribute()
            : this(Facebook.FacebookSettings.Current, CanvasSettings.Current)
        {
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new CanvasRedirectResult(this.GetLoginUrl(null, filterContext.HttpContext).ToString());
        }

        public Uri GetLoginUrl(IDictionary<string, object> parameters, HttpContextBase httpContext)
        {
            var canvasAuthorizer = new CanvasAuthorizer(this.FacebookSettings, this.canvasSettings, httpContext)
                                       {
                                           LoginDisplayMode = this.LoginDisplayMode,
                                           State = this.State,
                                           Perms = this.Perms,
                                           CancelUrlPath = this.CancelUrlPath
                                       };

            return canvasAuthorizer.GetLoginUrl(null);
        }
    }
}