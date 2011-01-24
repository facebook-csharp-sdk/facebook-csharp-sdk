
namespace Facebook
{
    /// <summary>
    /// Represents a Facebook application settings.
    /// </summary>
    public class FacebookAppSettings : IFacebookAppSettings
    {
        /// <summary>
        /// Facebook application id.
        /// </summary>
        private readonly string appId;

        /// <summary>
        /// Facebook application secret.
        /// </summary>
        private readonly string appSecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAppSettings"/> class.
        /// </summary>
        /// <param name="appId">
        /// The application id.
        /// </param>
        /// <param name="appSecret">
        /// The application secret.
        /// </param>
        public FacebookAppSettings(string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        /// <summary>
        /// Gets the application id.
        /// </summary>
        public string AppId
        {
            get { return this.appId; }
        }

        /// <summary>
        /// Gets or sets the application api key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets the application secret.
        /// </summary>
        public string AppSecret
        {
            get { return this.appSecret; }
        }

        /// <summary>
        /// Gets or sets the site url.
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the canvas page.
        /// </summary>
        public string CanvasPage { get; set; }

        /// <summary>
        /// Gets or sets the canvas url.
        /// </summary>
        public string CanvasUrl { get; set; }
    }
}