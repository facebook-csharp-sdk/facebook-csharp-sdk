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
        private readonly DateTime expires;
        private readonly string errorReason;
        private readonly string errorDescription;


        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthenticationResult"/> class.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <remarks>
        /// The values of parameters should not be url encoded.
        /// </remarks>
        private FacebookAuthenticationResult(IDictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("access_token"))
            {
                this.accessToken = parameters["access_token"].ToString();
            }

            if (parameters.ContainsKey("expires_in"))
            {
                var expiresIn = Convert.ToInt64(parameters["expires_in"]);
                this.expires = FacebookUtils.FromUnixTime(expiresIn);
            }

            if (parameters.ContainsKey("error_reason"))
            {
                this.errorReason = parameters["error_reason"].ToString();
            }

            if (parameters.ContainsKey("error_description"))
            {
                this.errorDescription = parameters["error_description"].ToString();
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

        public DateTime Expires
        {
            get { return this.expires; }
        }

        public string AccessToken
        {
            get { return this.accessToken; }
        }

        public bool IsSuccess
        {
            get { return string.IsNullOrEmpty(this.ErrorReason) && !string.IsNullOrEmpty(this.AccessToken); }
        }

        public FacebookSession ToSession()
        {
            return new FacebookSession
            {
                AccessToken = this.AccessToken,
                Expires = this.Expires,
            };
        }

        public static FacebookAuthenticationResult Parse(string uriString)
        {
            return Parse(new Uri(uriString));
        }

        public static FacebookAuthenticationResult Parse(Uri uri)
        {
            return Parse(uri, null);
        }

        public static FacebookAuthenticationResult Parse(Uri uri, IFacebookSettings facebookSettings)
        {
            return Parse(uri, facebookSettings, true);
        }

        public static bool TryParse(string uriString, out FacebookAuthenticationResult result)
        {
            return TryParse(uriString, null, out result);
        }

        public static bool TryParse(string uriString, IFacebookSettings settings, out FacebookAuthenticationResult result)
        {
            if (Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
            {
                return TryParse(new Uri(uriString), settings, out result);
            }

            result = null;
            return false;
        }

        public static bool TryParse(Uri uri, out FacebookAuthenticationResult result)
        {
            return TryParse(uri, null, out result);
        }

        public static bool TryParse(Uri uri, IFacebookSettings facebookSettings, out FacebookAuthenticationResult result)
        {
            result = Parse(uri, facebookSettings, false);
            return result != null;
        }

        private static FacebookAuthenticationResult Parse(Uri uri, IFacebookSettings facebookSettings, bool throws)
        {
            IDictionary<string, object> parameters;

            try
            {
                if (uri.AbsoluteUri.StartsWith("http://www.facebook.com/connect/login_success.html"))
                {
                    // if it is a desktop login
                    if (!string.IsNullOrEmpty(uri.Fragment))
                    {
                        // contains #access_token so remove #
                        var queryFragment = uri.Fragment.Substring(1);
                        parameters = FacebookUtils.ParseUrlQueryString(queryFragment);
                    }
                    else
                    {
                        // else it is part of querystring
                        // ?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.
                        parameters = FacebookUtils.ParseUrlQueryString(uri.Query);
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
