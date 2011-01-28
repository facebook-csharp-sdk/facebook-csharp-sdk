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
        /// Gets the login uri.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the facebook login uri.
        /// </returns>
        Uri GetLoginUri(IDictionary<string, object> parameters);

        /// <summary>
        /// Gets the logout url.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the logout url.
        /// </returns>
        Uri GetLogoutUrl(IDictionary<string, object> parameters);

#if !SILVERLIGHT // Silverlight should have only async calls

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

        /// <summary>
        /// Gets the access token by exchanging the code.
        /// </summary>
        /// <param name="code">
        /// The code to exchange.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        void ExchangeCodeForAccessTokenAsync(string code, IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state);

        /// <summary>
        /// Gets the access token by exchanging the code.
        /// </summary>
        /// <param name="code">
        /// The code to exchange.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        void ExchangeCodeForAccessTokenAsync(string code, IDictionary<string, object> parameters, FacebookAsyncCallback callback);

    }
}