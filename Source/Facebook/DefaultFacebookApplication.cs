// --------------------------------
// <copyright file="DefaultFacebookApplication.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    /// <summary>
    /// Represents the default Facebook application.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class DefaultFacebookApplication : IFacebookApplication
    {
        /// <summary>
        /// Gets or sets the application id.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the application secret.
        /// </summary>
        public string AppSecret { get; set; }

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

        /// <summary>
        /// Gets or sets the secure canvas url.
        /// </summary>
        public string SecureCanvasUrl { get; set; }

        /// <summary>
        /// Gets or sets the url to return the user after they cancel authorization.
        /// </summary>
        public string CancelUrlPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use Facebook beta.
        /// </summary>
        public bool UseFacebookBeta { get; set; }

        /// <summary>
        /// Gets a value indicating whether the scheme is secure.
        /// </summary>
        public bool IsSecureConnection { get; set; }
    }
}
