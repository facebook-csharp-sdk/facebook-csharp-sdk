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
    using System.Collections.Generic;

    /// <summary>
    /// Represents the authentication result of Facebook.
    /// </summary>
    public class FacebookOAuthResult
    {
        /// <summary>
        /// The access token.
        /// </summary>
        private readonly string _accessToken;

        /// <summary>
        /// Date and Time when the access token expires.
        /// </summary>
        private readonly DateTime _expires;

        /// <summary>
        /// Error that happens when using OAuth2 protocol.
        /// </summary>
        private readonly string _error;

        /// <summary>
        /// Short error reason for failed authentication if there was an error.
        /// </summary>
        private readonly string _errorReason;

        /// <summary>
        /// Long error description for failed authentication if there was an error.
        /// </summary>
        private readonly string _errorDescription;

        /// <summary>
        /// The code used to exchange access token.
        /// </summary>
        private readonly string _code;

        /// <summary>
        /// Gets or sets an opaque state used to maintain application state between the request and callback.
        /// </summary>
        private readonly string _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthResult"/> class.
        /// </summary>
        protected FacebookOAuthResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthResult"/> class.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <remarks>
        /// The values of parameters should not be url encoded.
        /// </remarks>
        internal FacebookOAuthResult(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            if (parameters.ContainsKey("state"))
            {
                _state = parameters["state"].ToString();
            }

            if (parameters.ContainsKey("error"))
            {
                _error = parameters["error"].ToString();

                if (parameters.ContainsKey("error_reason"))
                {
                    _errorReason = parameters["error_reason"].ToString();
                }

                if (parameters.ContainsKey("error_description"))
                {
                    _errorDescription = parameters["error_description"].ToString();
                }

                return;
            }

            if (parameters.ContainsKey("code"))
            {
                _code = parameters["code"].ToString();
            }

            if (parameters.ContainsKey("access_token"))
            {
                _accessToken = parameters["access_token"].ToString();
            }

            if (parameters.ContainsKey("expires_in"))
            {
                var expiresIn = Convert.ToDouble(parameters["expires_in"]);
                _expires = expiresIn == 0 ? DateTime.MaxValue : DateTime.UtcNow.AddSeconds(expiresIn);
            }
        }

        /// <summary>
        /// Error that happens when using OAuth2 protocol.
        /// </summary>
        /// <remarks>
        /// https://developers.facebook.com/docs/oauth/errors/
        /// </remarks>
        public virtual string Error
        {
            get { return _error; }
        }

        /// <summary>
        /// Gets the short error reason for failed authentication if an error occurred.
        /// </summary>
        public virtual string ErrorReason
        {
            get { return _errorReason; }
        }

        /// <summary>
        /// Gets the long error description for failed authentication if an error occurred.
        /// </summary>
        public virtual string ErrorDescription
        {
            get { return _errorDescription; }
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> when the access token will expire.
        /// </summary>
        public virtual DateTime Expires
        {
            get { return _expires; }
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        public virtual string AccessToken
        {
            get { return _accessToken; }
        }

        /// <summary>
        /// Gets a value indicating whether access token or code was successfully retrieved.
        /// </summary>
        public virtual bool IsSuccess
        {
            get
            {
                return string.IsNullOrEmpty(Error) &&
                       (!string.IsNullOrEmpty(AccessToken) || !string.IsNullOrEmpty(Code));
            }
        }

        /// <summary>
        /// Gets the code used to exchange with Facebook to retrieve access token.
        /// </summary>
        public virtual string Code
        {
            get { return _code; }
        }

        /// <summary>
        /// Gets an opaque state used to maintain application state between the request and callback.
        /// </summary>
        public virtual string State
        {
            get { return _state; }
        }

        /// <summary>
        /// Parse the url to <see cref="FacebookOAuthResult"/>.
        /// </summary>
        /// <param name="uriString">
        /// The url to parse.
        /// </param>
        /// <returns>
        /// Returns an instance of <see cref="FacebookOAuthResult"/>.
        /// </returns>
        public static FacebookOAuthResult Parse(string uriString)
        {
            return Parse(new Uri(uriString));
        }

        /// <summary>
        /// Parse the url to <see cref="FacebookOAuthResult"/>.
        /// </summary>
        /// <param name="uri">
        /// The url to parse.
        /// </param>
        /// <returns>
        /// Returns an instance of <see cref="FacebookOAuthResult"/>.
        /// </returns>
        public static FacebookOAuthResult Parse(Uri uri)
        {
            return Parse(uri, true);
        }

        /// <summary>
        /// Try parsing the url to <see cref="FacebookOAuthResult"/>.
        /// </summary>
        /// <param name="uriString">
        /// The url to parse.
        /// </param>
        /// <param name="result">
        /// An instance of <see cref="FacebookOAuthResult"/>.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public static bool TryParse(string uriString, out FacebookOAuthResult result)
        {
            if (Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
            {
                return TryParse(new Uri(uriString), out result);
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Try parsing the url to <see cref="FacebookOAuthResult"/>.
        /// </summary>
        /// <param name="uri">
        /// The url to parse.
        /// </param>
        /// <param name="result">
        /// An instance of <see cref="FacebookOAuthResult"/>.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public static bool TryParse(Uri uri, out FacebookOAuthResult result)
        {
            result = Parse(uri, false);
            return result != null;
        }

        /// <summary>
        /// Internal method for parsing the Facebook OAuth url.
        /// </summary>
        /// <param name="uri">
        /// The url to parse.
        /// </param>
        /// <param name="throws">
        /// Whether to throw the exception or not incase an error occurs.
        /// </param>
        /// <returns>
        /// The <see cref="FacebookOAuthResult"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throws if cannot parse the specified url.
        /// </exception>
        private static FacebookOAuthResult Parse(Uri uri, bool throws)
        {
            IDictionary<string, object> parameters = null;

            try
            {
                bool found = false;
                if (!string.IsNullOrEmpty(uri.Fragment))
                {
                    // #access_token and expires_in are in fragment
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
                    return new FacebookOAuthResult(parameters);
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
