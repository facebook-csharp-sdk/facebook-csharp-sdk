namespace Facebook
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the OAuth helpers.
    /// </summary>
    public interface IOAuthClientAuthorizer
    {
        /// <summary>
        /// Gets the client id.
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Gets the client secret.
        /// </summary>
        string ClientSecret { get; }

        /// <summary>
        /// Gets the redirect uri.
        /// </summary>
        Uri RedirectUri { get; }

        /// <summary>
        /// Gets the login uri for desktop applications.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the desktop login uri.
        /// </returns>
        Uri GetDesktopLoginUri(IDictionary<string, object> parameters);

        /// <summary>
        /// Gets the logout uri for desktop applications.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the desktop logout uri.
        /// </returns>
        Uri GetDesktopLogoutUri(IDictionary<string, object> parameters);

#if !SILVERLIGHT // silverlight should have only async calls

        // TODO: implement async version of ExchangeAccessTokenForCode

        /// <summary>
        /// Gets the access token by exchanging the code.
        /// </summary>
        /// <param name="code">
        /// The code to exchange.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the access token or expires if exists.
        /// </returns>
        object ExchangeCodeForAccessToken(string code, IDictionary<string, object> parameters);

#endif

    }
}