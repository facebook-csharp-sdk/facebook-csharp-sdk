//-----------------------------------------------------------------------
// <copyright file="FacebookClient.SignedRequest.cs" company="The Outercurve Foundation">
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
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Diagnostics.CodeAnalysis;

    public partial class FacebookClient
    {
        private const string InvalidSignedRequest = "Invalid signed_request";

        /// <summary>
        /// Tries parsing the facebook signed_request.
        /// </summary>
        /// <param name="appSecret">The app secret.</param>
        /// <param name="signedRequestValue">The signed_request value.</param>
        /// <param name="signedRequest">The parsed signed request.</param>
        /// <returns>True if signed request parsed successfully otherwise false.</returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]           
        public virtual bool TryParseSignedRequest(string appSecret, string signedRequestValue, out object signedRequest)
        {
            signedRequest = null;
            try
            {
                signedRequest = ParseSignedRequest(appSecret, signedRequestValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the facebook signed_request.
        /// </summary>
        /// <param name="signedRequestValue">The signed_request value.</param>
        /// <param name="signedRequest">The parsed signed request.</param>
        /// <returns>True if signed request parsed successfully otherwise false.</returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public virtual bool TryParseSignedRequest(string signedRequestValue, out object signedRequest)
        {
            return TryParseSignedRequest(AppSecret, signedRequestValue, out signedRequest);
        }

        /// <summary>
        /// Parse the facebook signed_request.
        /// </summary>
        /// <param name="appSecret">The appsecret.</param>
        /// <param name="signedRequestValue">The signed_request value.</param>
        /// <returns>The parse signed_request value.</returns>
        /// <exception cref="ArgumentNullException">Throws if appSecret or signedRequestValue is null or empty.</exception>
        /// <exception cref="InvalidOperationException">If the signedRequestValue is an invalid signed_request.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:LiteralsShouldBeSpelledCorrectly")]
        public virtual object ParseSignedRequest(string appSecret, string signedRequestValue)
        {
            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException("appSecret");
            if (string.IsNullOrEmpty(signedRequestValue))
                throw new ArgumentNullException("signedRequestValue");

            string[] split = signedRequestValue.Split('.');
            if (split.Length != 2)
            {
                // need to have exactly 2 parts
                throw new InvalidOperationException(InvalidSignedRequest);
            }

            string encodedSignature = split[0];
            string encodedEnvelope = split[1];

            if (string.IsNullOrEmpty(encodedSignature) || string.IsNullOrEmpty(encodedEnvelope))
                throw new InvalidOperationException(InvalidSignedRequest);

            var base64UrlDecoded = Base64UrlDecode(encodedEnvelope);
            var envelope = DeserializeJson(Encoding.UTF8.GetString(base64UrlDecoded, 0, base64UrlDecoded.Length), null);

            byte[] key = Encoding.UTF8.GetBytes(appSecret);
            byte[] digest = ComputeHmacSha256Hash(Encoding.UTF8.GetBytes(encodedEnvelope), key);

            var decodedSignature = Base64UrlDecode(encodedSignature);
            if (digest.Length != decodedSignature.Length) 
            {
                throw new InvalidOperationException(InvalidSignedRequest);
            }

            bool result = true;
            for (int i = 0; i < digest.Length; i++) 
            {
                result = result & (digest[i] == decodedSignature[i]);
            }

            if (!result) 
            {
                throw new InvalidOperationException(InvalidSignedRequest);
            }
                
            return envelope;
        }

        /// <summary>
        /// Parse the facebook signed_request.
        /// </summary>
        /// <param name="signedRequestValue">The signed_request value.</param>
        /// <returns>The parse signed_request value.</returns>
        /// <exception cref="ArgumentNullException">Throws if appSecret or signedRequestValue is null or empty.</exception>
        /// <exception cref="InvalidOperationException">If the signedRequestValue is an invalid signed_request.</exception>
        public virtual object ParseSignedRequest(string signedRequestValue)
        {
            return ParseSignedRequest(AppSecret, signedRequestValue);
        }

        /// <summary>
        /// Base64 Url decode.
        /// </summary>
        /// <param name="base64UrlSafeString">
        /// The base 64 url safe string.
        /// </param>
        /// <returns>
        /// The base 64 url decoded string.
        /// </returns>c
        private static byte[] Base64UrlDecode(string base64UrlSafeString)
        {
            if (string.IsNullOrEmpty(base64UrlSafeString))
                throw new ArgumentNullException("base64UrlSafeString");

            base64UrlSafeString =
                base64UrlSafeString.PadRight(base64UrlSafeString.Length + (4 - base64UrlSafeString.Length % 4) % 4, '=');
            base64UrlSafeString = base64UrlSafeString.Replace('-', '+').Replace('_', '/');
            return Convert.FromBase64String(base64UrlSafeString);
        }

        /// <summary>
        /// Computes the Hmac Sha 256 Hash.
        /// </summary>
        /// <param name="data">
        /// The data to hash.
        /// </param>
        /// <param name="key">
        /// The hash key.
        /// </param>
        /// <returns>
        /// The Hmac Sha 256 hash.
        /// </returns>
        private static byte[] ComputeHmacSha256Hash(byte[] data, byte[] key)
        {
            using (var crypto = new HMACSHA256(key))
            {
                return crypto.ComputeHash(data);
            }
        }

    }
}