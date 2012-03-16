//-----------------------------------------------------------------------
// <copyright file="FacebookOAuthResult.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

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
                var expiresIn = Convert.ToDouble(parameters["expires_in"], CultureInfo.InvariantCulture);
                _expires = expiresIn > 0 ? DateTime.UtcNow.AddSeconds(expiresIn) : DateTime.MaxValue;
            }
        }

        /// <summary>
        /// Error that happens when using OAuth2 protocol.
        /// </summary>
        /// <remarks>
        /// https://developers.facebook.com/docs/oauth/errors/
        /// </remarks>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
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
    }
}
