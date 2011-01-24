namespace Facebook
{
    /// <summary>
    /// Represents the Facebook application settings.
    /// </summary>
    public interface IFacebookAppSettings
    {
        /// <summary>
        /// Gets the application id.
        /// </summary>
        string AppId { get; }

        /// <summary>
        /// Gets the application api key.
        /// </summary>
        string ApiKey { get; }

        /// <summary>
        /// Gets the application secret.
        /// </summary>
        string AppSecret { get; }

        /// <summary>
        /// Gets the site url.
        /// </summary>
        string SiteUrl { get; }

        /// <summary>
        /// Gets the canvas page.
        /// </summary>
        string CanvasPage { get; }

        /// <summary>
        /// Gets the canvas url.
        /// </summary>
        string CanvasUrl { get; }
    }
}