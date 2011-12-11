// --------------------------------
// <copyright file="IFacebookApplication.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

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
        /// Gets the secure canvas url.
        /// </summary>
        string SecureCanvasUrl { get; }

        /// <summary>
        /// Gets the url to return the user after they cancel authorization.
        /// </summary>
        string CancelUrlPath { get; }

        /// <summary>
        /// Gets a value indicating whether it is beta.
        /// </summary>
        bool UseFacebookBeta { get; }

        /// <summary>
        /// Gets a value indicating whether the scheme is secure.
        /// </summary>
        bool IsSecureConnection { get; }
    }
}