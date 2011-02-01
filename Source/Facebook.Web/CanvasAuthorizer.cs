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
        /// The facebook settings.
        /// </summary>
        private readonly IFacebookApplication facebookApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasAuthorizer"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        public CanvasAuthorizer(IFacebookApplication settings, HttpContextBase httpContext)
            : base(settings, httpContext)
        {
            Contract.Requires(settings != null);
            Contract.Requires(!string.IsNullOrEmpty(settings.AppId));
            Contract.Requires(!string.IsNullOrEmpty(settings.AppSecret));
            Contract.Requires(httpContext != null);
            Contract.Requires(httpContext.Request != null);
            Contract.Requires(httpContext.Request.Params != null);
            Contract.Requires(httpContext.Response != null);

            this.facebookApplication = settings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasAuthorizer"/> class.
        /// </summary>
        public CanvasAuthorizer()
            : this(FacebookContext.Current, new HttpContextWrapper(System.Web.HttpContext.Current))
        {
        }

        /// <summary>
        /// Gets the Facebook application settings.
        /// </summary>
        public IFacebookApplication Settings
        {
            get
            {
                Contract.Ensures(Contract.Result<IFacebookApplication>() != null);
                return this.facebookApplication;
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

        /// <summary>
        /// Gets the canvas login url.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the login url.
        /// </returns>
        public Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            var defaultParameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(this.LoginDisplayMode))
            {
                defaultParameters["display"] = this.LoginDisplayMode;
            }

            if (!string.IsNullOrEmpty(this.Perms))
            {
                defaultParameters["scope"] = this.Perms;
            }

            var canvasUrlBuilder = new CanvasUrlBuilder(this.Settings, this.HttpRequest);
            return canvasUrlBuilder.GetLoginUrl(this.ReturnUrlPath, this.CancelUrlPath, this.State, FacebookUtils.Merge(defaultParameters, parameters));
        }

        [ContractInvariantMethod]
        private void InvarientObject()
        {
            Contract.Invariant(this.facebookApplication != null);
        }
    }
}