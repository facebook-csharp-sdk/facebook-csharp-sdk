namespace Facebook.Web
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Represents the Facebook authorizer class.
    /// </summary>
    public class Authorizer
    {
        /// <summary>
        /// The http context.
        /// </summary>
        private readonly HttpContextBase httpContext;

        /// <summary>
        /// The facebook session.
        /// </summary>
        private FacebookSession session;

        public string AppId { get; set; }
        public string AppSecret { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Authorizer"/> class.
        /// </summary>
        /// <param name="facebookSettings">
        /// The facebook settings.
        /// </param>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        public Authorizer(string appId, string appSecret, HttpContextBase httpContext)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(!string.IsNullOrEmpty(appSecret));
            Contract.Requires(httpContext != null);
            Contract.Requires(httpContext.Request != null);
            Contract.Requires(httpContext.Request.Params != null);
            Contract.Requires(httpContext.Response != null);

            this.AppId = appId;
            this.AppSecret = appSecret;
            this.httpContext = httpContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Authorizer"/> class.
        /// </summary>
        /// <param name="facebookSettings">
        /// The facebook settings.
        /// </param>
        public Authorizer(string appId, string appSecret)
            : this(appId, appSecret, new HttpContextWrapper(System.Web.HttpContext.Current))
        {
        }

        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        public string Perms { get; set; }

        /// <summary>
        /// Gets or sets the return url path.
        /// </summary>
        public string ReturnUrlPath { get; set; }

        /// <summary>
        /// Gets or sets the cancel url path.
        /// </summary>
        public string CancelUrlPath { get; set; }

        /// <summary>
        /// Gets or sets the login display mode.
        /// </summary>
        public string LoginDisplayMode { get; set; }

        /// <summary>
        /// Gets or sets an opaque state used to maintain application state between the request and callback.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        public virtual FacebookSession Session
        {
            get
            {
                return this.session ??
                       (this.session = FacebookWebUtils.GetSession(this.AppId, this.AppSecret, this.HttpRequest));
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

            return FacebookWebUtils.HasPermissions(this.AppId, this.AppSecret, userId, permissions);
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
        /// Checks if the user is authenticated and the application has all the specified permissions.
        /// </summary>
        /// <returns>
        /// Return true if the user is authenticated and the application has all the specified permissions.
        /// </returns>
        public virtual bool IsAuthorized()
        {
            bool isAuthenticated = this.Session != null;

            if (isAuthenticated && !string.IsNullOrEmpty(this.Perms))
            {
                var requiredPerms = this.Perms.Replace(" ", string.Empty).Split(',');
                var currentPerms = this.HasPermissions(requiredPerms);
                foreach (var perm in requiredPerms)
                {
                    if (!currentPerms.Contains(perm))
                    {
                        return false;
                    }
                }
            }

            return isAuthenticated;
        }

        /// <summary>
        /// Authorizes the user if the user is not logged in or the application does not have all the sepcified permissions.
        /// </summary>
        /// <returns>
        /// Return true if the user is authenticated and the application has all the specified permissions.
        /// </returns>
        public bool Authorize()
        {
            var isAuthorized = this.IsAuthorized();

            if (!isAuthorized)
            {
                this.HandleUnauthorizedRequest();
            }

            return isAuthorized;
        }

        /// <summary>
        /// Handle unauthorized requests.
        /// </summary>
        public virtual void HandleUnauthorizedRequest()
        {
            this.HttpResponse.Redirect(this.CancelUrlPath);
        }

        /// <summary>
        /// The code contracts invarient object method.
        /// </summary>
        [ContractInvariantMethod]
        private void InvarientObject()
        {
            Contract.Invariant(!string.IsNullOrEmpty(this.AppId));
            Contract.Invariant(!string.IsNullOrEmpty(this.AppSecret));
            Contract.Invariant(this.httpContext != null);
            Contract.Invariant(this.httpContext.Request.Params != null);
            Contract.Invariant(this.HttpContext.Response != null);
            Contract.Invariant(this.HttpContext.Request != null);
            Contract.Invariant(this.HttpContext.Request.Params != null);
        }
    }
}