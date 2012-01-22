// --------------------------------
// <copyright file="FacebookClient.Subscriptions.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.ComponentModel;
    using System.Text;

    public partial class FacebookClient
    {
        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE for http GET method.
        /// </summary>
        /// <param name="verifyToken">The verify token.</param>
        /// <param name="requestParams">The request parameter.</param>
        /// <returns>True if verification is successful otherwise false.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual bool IsVerifiedGetSubscription(string verifyToken, Func<string, string> requestParams)
        {
            if (string.IsNullOrEmpty(verifyToken))
                throw new ArgumentNullException("verifyToken");
            if (requestParams == null)
                throw new ArgumentNullException("requestParams");

            return requestParams("hub.mode") == "subscribe" &&
                   requestParams("hub.verify_token") == verifyToken &&
                   !string.IsNullOrEmpty(requestParams("hub.challenge"));
        }

        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE for http POST method.
        /// </summary>
        /// <param name="appSecret">The AppSecret.</param>
        /// <param name="jsonString">The json string.</param>
        /// <param name="requestParams">The request params.</param>
        /// <returns>True if verification is successful otherwise false.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual bool IsVerifiedPostSubscription(string appSecret, string jsonString, Func<string, string> requestParams)
        {
            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException("appSecret");
            if (requestParams == null)
                throw new ArgumentNullException("requestParams");

            var signature = requestParams("HTTP_X_HUB_SIGNATURE");

            if (!string.IsNullOrEmpty(signature) && signature.StartsWith("sha1="))
            {
                var expectedSha1 = signature.Substring(5);

                if (string.IsNullOrEmpty(expectedSha1) || string.IsNullOrEmpty(jsonString))
                    return false;

                var sha1 = ComputeHmacSha1Hash(Encoding.UTF8.GetBytes(jsonString), Encoding.UTF8.GetBytes(appSecret));

                var hashString = new StringBuilder();
                foreach (var b in sha1)
                    hashString.Append(b.ToString("x2"));

                if (expectedSha1 == hashString.ToString())
                    return true;
            }

            return false;
        }

        private static byte[] ComputeHmacSha1Hash(byte[] data, byte[] key)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (key == null)
                throw new ArgumentNullException("key");

            using (var crypto = new System.Security.Cryptography.HMACSHA1(key))
            {
                return crypto.ComputeHash(data);
            }
        }
    }
}