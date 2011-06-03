using System;
using System.Collections.Generic;

namespace Facebook
{
    /// <summary>
    /// Represents the Facebook OAuth Client.
    /// </summary>
    public interface IFacebookOAuthClient
    {
        /// <summary>
        /// Gets or sets the app id.
        /// </summary>
        string AppId { get; set; }

        /// <summary>
        /// Gets or sets the app secret.
        /// </summary>
        string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets the redirect uri.
        /// </summary>
        Uri RedirectUri { get; set; }

        /// <summary>
        /// Gets the login uri.
        /// </summary>
        /// <returns>
        /// Returns the facebook login uri.
        /// </returns>
        Uri GetLoginUrl();

        /// <summary>
        /// Gets the login uri.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the facebook login uri.
        /// </returns>
        /// <remarks>
        /// http://developers.facebook.com/docs/reference/dialogs/oauth
        /// Parameters that can be used:
        ///     client_id     : Your application's identifier. This is called client_id instead of app_id for this particular method to be compliant with the OAuth 2.0 specification. Required, but automatically specified by most SDKs.
        ///     redirect_uri  : The URL to redirect to after the user clicks a button on the dialog. Required, but automatically specified by most SDKs.
        ///     scope         : Optional. A comma-delimited list of permissions.
        ///     state         : Optional. An opaque string used to maintain application state between the request and callback. When Facebook redirects the user back to your redirect_uri, this value will be included unchanged in the response.
        ///     response_type : Optional, default is token. The requested response: an access token (token), an authorization code (code), or both (code_and_token).
        ///     display       : The display mode in which to render the dialog. The default is page on the www subdomain and wap on the m subdomain. This is automatically specified by most SDKs. (For WP7 builds it is set to touch.)
        /// </remarks>
        Uri GetLoginUrl(IDictionary<string, object> parameters);

        /// <summary>
        /// Gets the logout url.
        /// </summary>
        /// <returns>
        /// Returns the logout url.
        /// </returns>
        Uri GetLogoutUrl();

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

        /// <summary>
        /// Event handler for application access token completion.
        /// </summary>
        event EventHandler<FacebookApiEventArgs> GetApplicationAccessTokenCompleted;

#if !SILVERLIGHT

        /// <summary>
        /// Gets the application access token.
        /// </summary>
        /// <returns>
        /// The application access token.
        /// </returns>
        object GetApplicationAccessToken();

        /// <summary>
        /// Gets the application access token.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The application access token.
        /// </returns>
        object GetApplicationAccessToken(IDictionary<string, object> parameters);

#endif

        /// <summary>
        /// Gets the application access token asynchronously.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        void GetApplicationAccessTokenAsync(IDictionary<string, object> parameters, object userToken);

        /// <summary>
        /// Gets the application access token asynchronously.
        /// </summary>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        void GetApplicationAccessTokenAsync(object userToken);

        /// <summary>
        /// Gets the application access token asynchronously.
        /// </summary>
        void GetApplicationAccessTokenAsync();

        /// <summary>
        /// Event handler for application access token completion.
        /// </summary>
        event EventHandler<FacebookApiEventArgs> ExchangeCodeForAccessTokenCompleted;

#if !SILVERLIGHT

        /// <summary>
        /// Exchange code for access token.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        object ExchangeCodeForAccessToken(string code, IDictionary<string, object> parameters);

        /// <summary>
        /// Exchange code for access token.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        object ExchangeCodeForAccessToken(string code);

#endif

        /// <summary>
        /// Exchange code for access token asynchronously.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        void ExchangeCodeForAccessTokenAsync(string code, IDictionary<string, object> parameters, object userToken);

        /// <summary>
        /// Exchange code for access token asynchronously.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void ExchangeCodeForAccessTokenAsync(string code, IDictionary<string, object> parameters);

        /// <summary>
        /// Exchange code for access token asynchronously.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        void ExchangeCodeForAccessTokenAsync(string code);
    }
}