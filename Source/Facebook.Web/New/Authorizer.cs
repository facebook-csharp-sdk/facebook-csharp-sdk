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
        /// The code contracts invarient object method.
        /// </summary>
        [ContractInvariantMethod]
        private void InvarientObject()
        {
            Contract.Invariant(this.facebookSettings != null);
            Contract.Invariant(this.httpContext != null);
            Contract.Invariant(this.HttpContext.Response != null);
            Contract.Invariant(this.HttpContext.Request != null);
        }
    }
}