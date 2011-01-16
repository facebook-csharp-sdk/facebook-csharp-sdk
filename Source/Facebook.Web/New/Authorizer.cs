namespace Facebook.Web.New
{
    using System.Diagnostics.Contracts;
    using System.Web;

    /// <summary>
    /// Represents the Facebook authorizer class.
    /// </summary>
    public class Authorizer
    {
        /// <summary>
        /// The Facebook Settings (includes appid and appsecret).
        /// </summary>
        private readonly IFacebookSettings facebookSettings;

        /// <summary>
        /// The http context.
        /// </summary>
        private readonly HttpContextBase httpContext;

        /// <summary>
        /// The facebook session.
        /// </summary>
        private FacebookSession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="Authorizer"/> class.
        /// </summary>
        /// <param name="facebookSettings">
        /// The facebook settings.
        /// </param>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        public Authorizer(IFacebookSettings facebookSettings, HttpContextBase httpContext)
        {
            Contract.Requires(facebookSettings != null);
            Contract.Requires(!string.IsNullOrEmpty(facebookSettings.AppId));
            Contract.Requires(!string.IsNullOrEmpty(facebookSettings.AppSecret));
            Contract.Requires(httpContext != null);
            Contract.Requires(httpContext.Request != null);
            Contract.Requires(httpContext.Request.Params != null);
            Contract.Requires(httpContext.Response != null);

            this.facebookSettings = facebookSettings;
            this.httpContext = httpContext;
        }

        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        public string Perms { get; set; }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        public FacebookSession Session
        {
            get
            {
                return this.session ??
                       (this.session = FacebookWebUtils.GetSession(this.FacebookSettings.AppId, this.FacebookSettings.AppSecret, this.HttpRequest));
            }
        }

        /// <summary>
        /// Gets the Facebook Settings (includes appid and appsecret).
        /// </summary>
        public IFacebookSettings FacebookSettings
        {
            get
            {
                Contract.Ensures(Contract.Result<IFacebookSettings>() != null);
                return this.facebookSettings;
            }
        }

        /// <summary>
        /// Gets the http context.
        /// </summary>
        public HttpContextBase HttpContext
        {
            get
            {
                Contract.Ensures(Contract.Result<HttpContextBase>() != null);
                return this.httpContext;
            }
        }

        /// <summary>
        /// Gets the http request.
        /// </summary>
        protected HttpRequestBase HttpRequest
        {
            get
            {
                Contract.Ensures(Contract.Result<HttpRequestBase>() != null);
                return this.HttpContext.Request;
            }
        }

        /// <summary>
        /// Gets the http response.
        /// </summary>
        protected HttpResponseBase HttpResponse
        {
            get
            {
                Contract.Ensures(Contract.Result<HttpResponseBase>() != null);
                return this.HttpContext.Response;
            }
        }

        /// <summary>
        /// Check whether the user has the specified permissions.
        /// </summary>
        /// <param name="permissions">
        /// The permissions.
        /// </param>
        /// <returns>
        /// Returns the list of allowed permissions.
        /// </returns>
        public virtual string[] HasPermissions(string[] permissions)
        {
            Contract.Ensures(Contract.Result<string[]>() != null);

            long userId;

            if (this.Session == null || !long.TryParse(this.Session.UserId, out userId))
            {
                return new string[0];
            }

            return FacebookWebUtils.HasPermissions(this.FacebookSettings.AppId, this.FacebookSettings.AppSecret, userId, permissions);
        }

        /// <summary>
        /// Check whether the user has the specified permissions.
        /// </summary>
        /// <param name="permission">
        /// The permission.
        /// </param>
        /// <returns>
        /// Returns true if the user has permission otherwise false.
        /// </returns>
        public virtual bool HasPermission(string permission)
        {
            return this.HasPermissions(new[] { permission }).Length == 1;
        }

        /// <summary>
        /// The code contracts invarient object method.
        /// </summary>
        [ContractInvariantMethod]
        private void InvarientObject()
        {
            Contract.Invariant(this.facebookSettings != null);
            Contract.Invariant(!string.IsNullOrEmpty(this.FacebookSettings.AppId));
            Contract.Invariant(!string.IsNullOrEmpty(this.FacebookSettings.AppSecret));
            Contract.Invariant(this.httpContext != null);
            Contract.Invariant(this.httpContext.Request.Params != null);
            Contract.Invariant(this.HttpContext.Response != null);
            Contract.Invariant(this.HttpContext.Request != null);
            Contract.Invariant(this.HttpContext.Request.Params != null);
        }
    }
}