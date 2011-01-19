// --------------------------------
// <copyright file="FacebookAuthenticationResult.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the authentication result of Facebook.
    /// </summary>
    public sealed class FacebookAuthenticationResult
    {
        /// <summary>
        /// The access token.
        /// </summary>
        private readonly string accessToken;

        /// <summary>
        /// Date and Time when the access token expires.
        /// </summary>
        private readonly DateTime expires;

        /// <summary>
        /// Short error reason for failed authentication if there was an error.
        /// </summary>
        private readonly string errorReason;

        /// <summary>
        /// Long error description for failed authentication if there was an error.
        /// </summary>
        private readonly string errorDescription;

        /// <summary>
        /// The code used to exchange access token.
        /// </summary>
        private readonly string code;

        /// <summary>
        /// Gets or sets an opaque state used to maintain application state between the request and callback.
        /// </summary>
        private readonly string state;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthenticationResult"/> class.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <remarks>
        /// The values of parameters should not be url encoded.
        /// </remarks>
        internal FacebookAuthenticationResult(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            if (parameters.ContainsKey("error_reason"))
            {
                this.errorReason = parameters["error_reason"].ToString();

                if (parameters.ContainsKey("error_description"))
                {
                    this.errorDescription = parameters["error_description"].ToString();
                }

                return;
            }

            if (parameters.ContainsKey("code"))
            {
                this.code = parameters["code"].ToString();
            }

            if (parameters.ContainsKey("state"))
            {
                this.state = parameters["state"].ToString();
            }

            if (parameters.ContainsKey("access_token"))
            {
                this.accessToken = parameters["access_token"].ToString();
            }

            if (parameters.ContainsKey("expires_in"))
            {
                var expiresIn = Convert.ToInt64(parameters["expires_in"]);
                this.expires = FacebookUtils.FromUnixTime(expiresIn);
            }
        }

        /// <summary>
        /// Gets the short error reason for failed authentication if an error occurred.
        /// </summary>
        public string ErrorReason
        {
            get { return this.errorReason; }
        }

        /// <summary>
        /// Gets the long error description for failed authentication if an error occurred.
        /// </summary>
        public string ErrorDescription
        {
            get { return this.errorDescription; }
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> when the access token will expire.
        /// </summary>
        public DateTime Expires
        {
            get { return this.expires; }
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        public string AccessToken
        {
            get { return this.accessToken; }
        }

        /// <summary>
        /// Gets a value indicating whether access token or code was successfully retrieved.
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return string.IsNullOrEmpty(this.ErrorReason) &&
                       (!string.IsNullOrEmpty(this.AccessToken) || !string.IsNullOrEmpty(this.Code));
            }
        }

        /// <summary>
        /// Gets the code used to exchange with facebook to retrieve access token.
        /// </summary>
        public string Code
        {
            get { return this.code; }
        }

        /// <summary>
        /// Gets an opaque state used to maintain application state between the request and callback.
        /// </summary>
        public string State
        {
            get { return this.state; }
        }

        /// <summary>
        /// Converts <see cref="FacebookAuthenticationResult"/> to <see cref="FacebookSession"/>.
        /// </summary>
        /// <returns>
        /// An instance of converted <see cref="FacebookSession"/>.
        /// </returns>
        public FacebookSession ToSession()
        {
            return new FacebookSession
                       {
                           AccessToken = this.AccessToken,
                           Expires = this.Expires,
                       };
        }

        /// <summary>
        /// Parse the uri to <see cref="FacebookAuthenticationResult"/>.
        /// </summary>
        /// <param name="uriString">
        /// The uri string.
        /// </param>
        /// <returns>
        /// Returns an instance of <see cref="FacebookAuthenticationResult"/>.
        /// </returns>
        public static FacebookAuthenticationResult Parse(string uriString)
        {
            return Parse(new Uri(uriString));
        }

        /// <summary>
        /// Parse the uri to <see cref="FacebookAuthenticationResult"/>.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <returns>
        /// Returns an instance of <see cref="FacebookAuthenticationResult"/>.
        /// </returns>
        public static FacebookAuthenticationResult Parse(Uri uri)
        {
            return Parse(uri, null);
        }

        /// <summary>
        /// Parse the uri to <see cref="FacebookAuthenticationResult"/>.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <param name="facebookSettings">
        /// The facebook settings.
        /// </param>
        /// <returns>
        /// Returns an instance of <see cref="FacebookAuthenticationResult"/>.
        /// </returns>
        public static FacebookAuthenticationResult Parse(Uri uri, IFacebookSettings facebookSettings)
        {
            return Parse(uri, facebookSettings, true);
        }

        /// <summary>
        /// Try parsing the uri to <see cref="FacebookAuthenticationResult"/>.
        /// </summary>
        /// <param name="uriString">
        /// The uri string.
        /// </param>
        /// <param name="result">
        /// An instance of <see cref="FacebookAuthenticationResult"/>.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public static bool TryParse(string uriString, out FacebookAuthenticationResult result)
        {
            return TryParse(uriString, null, out result);
        }

        /// <summary>
        /// Try parsing the uri to <see cref="FacebookAuthenticationResult"/>.
        /// </summary>
        /// <param name="uriString">
        /// The uri string.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="result">
        /// An instance of <see cref="FacebookAuthenticationResult"/>.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public static bool TryParse(string uriString, IFacebookSettings settings, out FacebookAuthenticationResult result)
        {
            if (Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
            {
                return TryParse(new Uri(uriString), settings, out result);
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Try parsing the uri to <see cref="FacebookAuthenticationResult"/>.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <param name="result">
        /// An instance of <see cref="FacebookAuthenticationResult"/>.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public static bool TryParse(Uri uri, out FacebookAuthenticationResult result)
        {
            return TryParse(uri, null, out result);
        }

        /// <summary>
        /// Try parsing the uri to <see cref="FacebookAuthenticationResult"/>.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <param name="facebookSettings">
        /// The facebook settings.
        /// </param>
        /// <param name="result">
        /// An instance of <see cref="FacebookAuthenticationResult"/>.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public static bool TryParse(Uri uri, IFacebookSettings facebookSettings, out FacebookAuthenticationResult result)
        {
            result = Parse(uri, facebookSettings, false);
            return result != null;
        }

        private static FacebookAuthenticationResult Parse(Uri uri, IFacebookSettings facebookSettings, bool throws)
        {
            IDictionary<string, object> parameters = null;

            try
            {
                bool found = false;
                if (!string.IsNullOrEmpty(uri.Fragment))
                {
                    // #access_token and expires_in are in fragement
                    var fragment = uri.Fragment.Substring(1);
                    parameters = FacebookUtils.ParseUrlQueryString(fragment);
                    if (parameters.ContainsKey("access_token"))
                    {
                        found = true;
                    }
                }

                // code, state, error_reason, error and error_description are in query
                // ?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.
                var queryPart = FacebookUtils.ParseUrlQueryString(uri.Query);
                if (queryPart.ContainsKey("code") || (queryPart.ContainsKey("error") && queryPart.ContainsKey("error_description")))
                {
                    found = true;
                }

                if (found)
                {
                    parameters = FacebookUtils.Merge(parameters, queryPart);
                    return new FacebookAuthenticationResult(parameters);
                }
            }
            catch
            {
                if (throws)
                {
                    throw;
                }

                return null;
            }

            if (throws)
            {
                throw new InvalidOperationException("Could not parse authentication url.");
            }

            return null;
        }

    }
}
