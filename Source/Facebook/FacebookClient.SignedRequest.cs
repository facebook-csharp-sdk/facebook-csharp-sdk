// --------------------------------
// <copyright file="FacebookClient.SignedRequest.cs" company="Thuzi LLC (www.thuzi.com)">
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
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

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
        /// Parse the facebook signed_request.
        /// </summary>
        /// <param name="appSecret">The appsecret.</param>
        /// <param name="signedRequestValue">The signed_request value.</param>
        /// <returns>The parse signed_request value.</returns>
        /// <exception cref="ArgumentNullException">Throws if appSecret or signedRequestValue is null or empty.</exception>
        /// <exception cref="InvalidOperationException">If the signedRequestValue is an invalid signed_request.</exception>
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

            string encodedignature = split[0];
            string encodedEnvelope = split[1];

            if (string.IsNullOrEmpty(encodedignature) || string.IsNullOrEmpty(encodedEnvelope))
                throw new InvalidOperationException(InvalidSignedRequest);

            var base64UrlDecoded = Base64UrlDecode(encodedEnvelope);
            var envelope = (IDictionary<string, object>)DeserializeJson(Encoding.UTF8.GetString(base64UrlDecoded, 0, base64UrlDecoded.Length), null);
            var algorithm = (string)envelope["algorithm"];
            if (!algorithm.Equals("HMAC-SHA256"))
                throw new InvalidOperationException("Unknown algorithm. Expected HMAC-SHA256");

            byte[] key = Encoding.UTF8.GetBytes(appSecret);
            IEnumerable<byte> digest = ComputeHmacSha256Hash(Encoding.UTF8.GetBytes(encodedEnvelope), key);

            if (!digest.SequenceEqual(Base64UrlDecode(encodedignature)))
                throw new InvalidOperationException(InvalidSignedRequest);
            return envelope;
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
        private static IEnumerable<byte> ComputeHmacSha256Hash(byte[] data, byte[] key)
        {
            using (var crypto = new HMACSHA256(key))
            {
                return crypto.ComputeHash(data);
            }
        }
    }
}