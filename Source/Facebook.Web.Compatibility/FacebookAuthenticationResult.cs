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
    using System.Runtime.CompilerServices;

    [Obsolete]
    [TypeForwardedFrom("Facebook, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
    public sealed class FacebookAuthenticationResult
    {
        private FacebookAuthenticationResult(string accessToken, DateTime expires, string errorReason, string errorDescription)
        {
            throw new NotImplementedException();
        }

        private FacebookAuthenticationResult(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public string ErrorReason
        {
            get { throw new NotImplementedException(); }
        }

        public string ErrorDescription
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime Expires
        {
            get { throw new NotImplementedException(); }
        }

        public string AccessToken
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSuccess
        {
            get { return string.IsNullOrEmpty(this.ErrorReason) && !string.IsNullOrEmpty(this.AccessToken); }
        }

        public FacebookSession ToSession()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public static bool TryParse(string uriString, out FacebookAuthenticationResult result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(string uriString, IFacebookSettings settings, out FacebookAuthenticationResult result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(Uri uri, out FacebookAuthenticationResult result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(Uri uri, IFacebookSettings facebookSettings, out FacebookAuthenticationResult result)
        {
            throw new NotImplementedException();
        }

    }
}
