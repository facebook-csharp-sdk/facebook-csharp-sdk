namespace Facebook
{
    /// <summary>
    /// Represents a Facebook Application.
    /// </summary>
    public interface IFacebookApplication
    {
        /// <summary>
        /// Gets the application id.
        /// </summary>
        string AppId { get; }

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

        /// <summary>
        /// Gets the url to return the user after they cancel authorization.
        /// </summary>
        string CancelUrlPath { get; }
    }
}