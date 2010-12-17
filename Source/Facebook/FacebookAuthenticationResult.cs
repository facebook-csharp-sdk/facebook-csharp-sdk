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

        private FacebookAuthenticationResult(string accessToken, DateTime expires, string errorReason, string errorDescription)
        {
            this.accessToken = accessToken;
            this.expires = expires;
            this.errorReason = errorReason;
            this.errorDescription = errorDescription;
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
                var expiresIn = Convert.ToDouble(parameters["expires_in"]);
                this.expires = DateTimeConvertor.FromUnixTime(expiresIn);
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
