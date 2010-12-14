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

    public class FacebookAuthenticationResult
    {
        private readonly string accessToken;
        private readonly long expiresIn;
        private readonly string errorReasonText;

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
        public FacebookAuthenticationResult(string accessToken, long expiresIn, string errorReasonText)
        {
            this.accessToken = accessToken;
            this.expiresIn = expiresIn;
            this.errorReasonText = errorReasonText;
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
                this.errorReasonText = Uri.UnescapeDataString((string)parameters["error_reason"]);
            }
        }

        public string ErrorReasonText
        {
            get { return errorReasonText; }
        }

        public long ExpiresIn
        {
            get { return expiresIn; }
        }

        public string AccessToken
        {
            get { return accessToken; }
        }

        public bool IsSuccess
        {
            get { return string.IsNullOrEmpty(ErrorReasonText) && !string.IsNullOrEmpty(AccessToken); }
        }

        public static FacebookAuthenticationResult Parse(string url)
        {
            return Parse(url, null);
        }

        public static FacebookAuthenticationResult Parse(string url, IFacebookSettings facebookSettings)
        {
            return Parse(new Uri(url), facebookSettings);
        }

        public static FacebookAuthenticationResult Parse(Uri uri)
        {
            return Parse(uri, null);
        }

        public static FacebookAuthenticationResult Parse(Uri uri, IFacebookSettings facebookSettings)
        {
            var parameters = new Dictionary<string, object>();

            if (uri.AbsoluteUri.StartsWith("http://www.facebook.com/connect/login_success.html"))
            {
                // if it is a desktop login
                if (!string.IsNullOrEmpty(uri.Fragment))
                {
                    // contains #access_token so replace # with ?
                    var queryFragment = "?" + uri.Fragment.Substring(1);
                    FacebookAppBase.ParseUrlParameters(queryFragment, parameters);
                }
                else
                {
                    // else it is part of querystring
                    // ?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.
                    FacebookAppBase.ParseUrlParameters(uri.Query, parameters);
                }

                return new FacebookAuthenticationResult(parameters);
            }

            // NOTE: throw error or return null or return FacebookAuthenticationResult with IsSuccess false.
            // return null or throwing error might not be so good idea
            // coz the user would have to wrap it with try catch or check for if null
            // extra headache for the user consuming this sdk.
            return null;
        }
    }
}
