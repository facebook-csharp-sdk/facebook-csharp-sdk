// --------------------------------
// <copyright file="FacebookSubscriptionVerifier.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Facebook helper methods for web.
    /// </summary>
    internal static class FacebookSubscriptionVerifier
    {
        internal const string HTTP_X_HUB_SIGNATURE_KEY = "HTTP_X_HUB_SIGNATURE";

        internal static byte[] ComputeHmacSha1Hash(byte[] data, byte[] key)
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

        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE.
        /// </summary>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <param name="httpXHubSignature">
        /// The http x hub signature.
        /// </param>
        /// <param name="jsonString">
        /// The json string.
        /// </param>
        /// <returns>
        /// Returns true if validation is successful.
        /// </returns>
        internal static bool VerifyHttpXHubSignature(string secret, string httpXHubSignature, string jsonString)
        {
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException("secret");

            if (!string.IsNullOrEmpty(httpXHubSignature) && httpXHubSignature.StartsWith("sha1=") && httpXHubSignature.Length > 5 && !string.IsNullOrEmpty(jsonString))
            {
                // todo: test inner parts
                var expectedSignature = httpXHubSignature.Substring(5);

                var sha1 = ComputeHmacSha1Hash(Encoding.UTF8.GetBytes(jsonString), Encoding.UTF8.GetBytes(secret));

                var hashString = new StringBuilder();
                foreach (var b in sha1)
                {
                    hashString.Append(b.ToString("x2"));
                }

                if (expectedSignature == hashString.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE for http GET method.
        /// </summary>
        /// <param name="request">
        /// The http request.
        /// </param>
        /// <param name="verifyToken">
        /// The verify token.
        /// </param>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        /// <returns>
        /// Returns true if successful otherwise false.
        /// </returns>
        internal static bool VerifyGetSubscription(HttpRequestBase request, string verifyToken, out string errorMessage)
        {
            if (!request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Invalid HttpRequest method");
            if (string.IsNullOrEmpty(verifyToken))
                throw new ArgumentNullException("verifyToken");

            errorMessage = null;

            if (request.Params["hub.mode"] == "subscribe")
            {
                if (request.Params["hub.verify_token"] == verifyToken)
                {
                    if (string.IsNullOrEmpty(request.Params["hub.challenge"]))
                    {
                        errorMessage = Properties.Resources.InvalidHubChallenge;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    errorMessage = Properties.Resources.InvalidVerifyToken;
                }
            }
            else
            {
                errorMessage = Properties.Resources.InvalidHubMode;
            }

            return false;
        }

        /// <summary>
        /// Verify HTTP_X_HUB_SIGNATURE for http POST method.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <param name="jsonString">
        /// The json string.
        /// </param>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        /// <returns>
        /// Returns true if successful otherwise false.
        /// </returns>
        internal static bool VerifyPostSubscription(HttpRequestBase request, string secret, string jsonString, out string errorMessage)
        {
            if (!request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Invalid HttpRequest method");
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException("secret");

            errorMessage = null;

            // signatures looks somewhat like "sha1=4594ae916543cece9de48e3289a5ab568f514b6a"
            var signature = request.Params["HTTP_X_HUB_SIGNATURE"];

            if (!string.IsNullOrEmpty(signature) && signature.StartsWith("sha1="))
            {
                var expectedSha1 = signature.Substring(5);

                if (string.IsNullOrEmpty(expectedSha1))
                {
                    errorMessage = Properties.Resources.InvalidHttpXHubSignature;
                }
                else
                {
                    if (string.IsNullOrEmpty(jsonString))
                    {
                        errorMessage = Properties.Resources.InvalidJsonString;
                        return false;
                    }

                    var sha1 = ComputeHmacSha1Hash(Encoding.UTF8.GetBytes(jsonString), Encoding.UTF8.GetBytes(secret));

                    var hashString = new StringBuilder();
                    foreach (var b in sha1)
                    {
                        hashString.Append(b.ToString("x2"));
                    }

                    if (expectedSha1 == hashString.ToString())
                    {
                        // todo: test
                        return true;
                    }

                    // todo: test
                    errorMessage = Properties.Resources.InvalidHttpXHubSignature;
                }
            }
            else
            {
                errorMessage = Properties.Resources.InvalidHttpXHubSignature;
            }

            return false;
        }
    }
}