namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web;

    /// <summary>
    /// Facebook canvas authorizer.
    /// </summary>
    public class CanvasAuthorizer : Authorizer
    {
        /// <summary>
        /// Facebook canvas settings
        /// </summary>
        private readonly ICanvasSettings canvasSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasAuthorizer"/> class.
        /// </summary>
        /// <param name="facebookSettings">
        /// The facebook settings.
        /// </param>
        /// <param name="canvasSettings">
        /// The canvas settings.
        /// </param>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        public CanvasAuthorizer(string appId, string appSecret, ICanvasSettings canvasSettings, HttpContextBase httpContext)
            : base(appId, appSecret, httpContext)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(!string.IsNullOrEmpty(appSecret));
            Contract.Requires(httpContext != null);
            Contract.Requires(httpContext.Request != null);
            Contract.Requires(httpContext.Request.Params != null);
            Contract.Requires(httpContext.Response != null);
            Contract.Requires(canvasSettings != null);

            this.canvasSettings = canvasSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasAuthorizer"/> class.
        /// </summary>
        public CanvasAuthorizer(string appId, string appSecret)
            : this(appId, appSecret, Web.CanvasSettings.Current, new HttpContextWrapper(System.Web.HttpContext.Current))
        {
        }

        /// <summary>
        /// Gets the Facebook canvas settings.
        /// </summary>
        public ICanvasSettings CanvasSettings
        {
            get
            {
                Contract.Ensures(Contract.Result<ICanvasSettings>() != null);
                return this.canvasSettings;
            }
        }

        /// <summary>
        /// Handle unauthorized requests.
        /// </summary>
        public override void HandleUnauthorizedRequest()
        {
            this.HttpResponse.ContentType = "text/html";
            this.HttpResponse.Write(CanvasUrlBuilder.GetCanvasRedirectHtml(this.GetLoginUrl(null)));
        }

        public Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            var defaultParameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(this.LoginDisplayMode))
            {
                defaultParameters["display"] = this.LoginDisplayMode;
            }

            if (!string.IsNullOrEmpty(this.Perms))
            {
                defaultParameters["scope"] = this.Perms;
            }

            var urlBuilder = new CanvasUrlBuilder(this.canvasSettings, this.HttpRequest);
            return urlBuilder.GetLoginUrl(this.AppId, this.AppSecret, this.ReturnUrlPath, this.CancelUrlPath, this.State, defaultParameters);
        }

        [ContractInvariantMethod]
        private void InvarientObject()
        {
            Contract.Invariant(this.canvasSettings != null);
        }
    }
}