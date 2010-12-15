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

    public sealed class FacebookAuthenticationResult
    {
        private readonly string accessToken;
        private readonly long expiresIn;
        private readonly string errorReason;
        private readonly string errorDescription;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthenticationResult"/> class.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        /// <param name="expiresIn">
        /// The expires in.
        /// </param>
        /// <param name="errorReasonText">
        /// The error reason text.
        /// </param>
        private FacebookAuthenticationResult(string accessToken, long expiresIn, string errorReasonText)
        {
            this.accessToken = accessToken;
            this.expiresIn = expiresIn;
            this.errorReason = errorReasonText;
        }

        private FacebookAuthenticationResult(IDictionary<string, object> parameters)
        {
            // decode access token first
            if (parameters.ContainsKey("access_token"))
            {
                this.accessToken = Uri.UnescapeDataString((string)parameters["access_token"]);
            }

            if (parameters.ContainsKey("expires_in"))
            {
                this.expiresIn = Convert.ToInt64(parameters["expires_in"]);
            }

            if (parameters.ContainsKey("error_reason"))
            {
                this.errorReason = Uri.UnescapeDataString((string)parameters["error_reason"]);
            }
            if (parameters.ContainsKey("error_description"))
            {
                this.errorDescription = Uri.UnescapeDataString((string)parameters["error_description"]);
            }
        }

        public string ErrorReason
        {
            get { return this.errorReason; }
        }

        public string ErrorDescription
        {
            get { return this.errorDescription; }
        }

        public long ExpiresIn
        {
            get { return this.expiresIn; }
        }

        public string AccessToken
        {
            get { return this.accessToken; }
        }

        public static FacebookAuthenticationResult Parse(string uriString)
        {
            return Parse(uriString);
        }

        public static FacebookAuthenticationResult Parse(Uri uri)
        {
            return Parse(uri, true);
        }

        public static bool TryParse(string uriString, out FacebookAuthenticationResult result)
        {
            if (Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
            {
                return TryParse(new Uri(uriString), out result);
            }
            result = null;
            return false;
        }

        public static bool TryParse(Uri uri, out FacebookAuthenticationResult result)
        {
            result = Parse(uri, false);
            return result != null;
        }

        private static FacebookAuthenticationResult Parse(Uri uri, bool throws)
        {
            var parameters = new Dictionary<string, object>();
            try
            {
                if (uri.AbsoluteUri.StartsWith("http://www.facebook.com/connect/login_success.html"))
                {
                    // if it is a desktop login
                    if (!string.IsNullOrEmpty(uri.Fragment))
                    {
                        // contains #access_token so replace # with ?
                        var queryFragment = "?" + uri.Fragment.Substring(1);
                        FacebookAppBase.ParseQueryParametersToDictionary(queryFragment, parameters);
                    }
                    else
                    {
                        // else it is part of querystring
                        // ?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.
                        FacebookAppBase.ParseQueryParametersToDictionary(uri.Query, parameters);
                    }

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
