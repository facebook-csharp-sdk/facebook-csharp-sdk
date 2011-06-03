// --------------------------------
// <copyright file="FacebookOAuthResult.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;

    /// <summary>
    /// Represents the authentication result of Facebook.
    /// </summary>
    public interface IFacebookOAuthResult
    {
        /// <summary>
        /// Gets the short error reason for failed authentication if an error occurred.
        /// </summary>
        string ErrorReason { get; }

        /// <summary>
        /// Gets the long error description for failed authentication if an error occurred.
        /// </summary>
        string ErrorDescription { get; }

        /// <summary>
        /// Gets the <see cref="DateTime"/> when the access token will expire.
        /// </summary>
        DateTime Expires { get; }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// Gets a value indicating whether access token or code was successfully retrieved.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Gets the code used to exchange with facebook to retrieve access token.
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Gets an opaque state used to maintain application state between the request and callback.
        /// </summary>
        string State { get; }
    }
}