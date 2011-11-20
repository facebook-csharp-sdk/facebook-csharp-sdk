// --------------------------------
// <copyright file="FacebookWebHelper.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    class FacebookWebHelper
    {
        #region Signed Request

        private const string InvalidSignedRequest = "Invalid signed_request";

        public static object TryParseFacebookSignedRequest(string appSecret, string signedRequestValue, Func<string, object> deserializeObject, bool throws)
        {
            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException("appSecret");
            if (string.IsNullOrEmpty(signedRequestValue))
                throw new ArgumentNullException("signedRequestValue");

            try
            {
                string[] split = signedRequestValue.Split('.');
                if (split.Length != 2)
                {
                    // need to have exactly 2 parts
                    throw new InvalidOperationException(InvalidSignedRequest);
                }

                string encodedignature = split[0];
                string encodedEnvelope = split[1];

                if (string.IsNullOrWhiteSpace(encodedignature) || string.IsNullOrWhiteSpace(encodedEnvelope))
                    throw new InvalidOperationException(InvalidSignedRequest);

                var envelope = (IDictionary<string, object>)deserializeObject(Encoding.UTF8.GetString(Base64UrlDecode(encodedEnvelope)));
                var algorithm = (string)envelope["algorithm"];
                if (!algorithm.Equals("HMAC-SHA256"))
                    throw new InvalidOperationException("Unknown algorithm. Expected HMAC-SHA256");

                byte[] key = Encoding.UTF8.GetBytes(appSecret);
                byte[] digest = ComputeHmacSha256Hash(Encoding.UTF8.GetBytes(encodedEnvelope), key);

                if (!digest.SequenceEqual(Base64UrlDecode(encodedignature)))
                    throw new InvalidOperationException(InvalidSignedRequest);
                return envelope;
            }
            catch
            {
                if (throws)
                    throw;
                return null;
            }
        }

        public static object TryParseFacebookSignedRequest(string appId, string appSecret, Func<bool> isInCache, Func<object> getFromCache, Func<string> getSignedRequestFormValue, Func<string, string> getSignedRequestCookieValue, Action<object> cache, Func<string, object> deserializeObject, bool throws)
        {
            object signedRequest = null;

            if (isInCache())
            {
                // return the cached value if present
                signedRequest = getFromCache();
            }
            else
            {
                // check signed_request value from Form for canvas/tabs/page applications
                var signedRequestValue = getSignedRequestFormValue();
                if (!string.IsNullOrEmpty(signedRequestValue))
                    signedRequest = TryParseFacebookSignedRequest(appSecret, signedRequestValue, deserializeObject, throws);

                if (signedRequest == null)
                {
                    // if signed_request is null, check from the fb cookie set by FB JS SDK
                    if (string.IsNullOrEmpty(appSecret))
                        throw new ArgumentNullException("appId");
                    var signedRequestCookieValue = getSignedRequestCookieValue("fbsr_" + appId);
                    if (!string.IsNullOrEmpty(signedRequestCookieValue))
                        signedRequest = TryParseFacebookSignedRequest(appSecret, signedRequestCookieValue, deserializeObject, throws);
                }
                cache(signedRequest);
            }

            return signedRequest;
        }

        #endregion

        #region Canvas Application

        public static string FacebookCanvasPageUrl(string canvasPageOrAppName,
            Func<string> requestUrlScheme, Func<bool> isReferrerBeta,
            bool? https = null, bool? beta = null)
        {
            if (string.IsNullOrEmpty(canvasPageOrAppName))
                throw new ArgumentNullException("canvasPageOrAppName");

            var sb = new StringBuilder();

            if (https.HasValue)
                sb.Append(https.Value ? "https" : "http");
            else
                sb.Append(requestUrlScheme());
            sb.Append("://");

            bool useBeta = beta.HasValue ? beta.Value : isReferrerBeta();
            sb.Append(useBeta ? "apps.beta.facebook.com" : "apps.facebook.com");

            canvasPageOrAppName = RemoveTrailingSlash(canvasPageOrAppName);
            sb.Append(canvasPageOrAppName.Contains("/")
                          ? canvasPageOrAppName.Substring(canvasPageOrAppName.LastIndexOf('/'))
                          : "/" + canvasPageOrAppName);

            return sb.ToString();
        }

        #endregion

        #region Helper Methods

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

        /// <summary>
        /// Removes the trailing slash.
        /// </summary>
        /// <param name="input">
        /// The input string to remove the trailing slash from.
        /// </param>
        /// <returns>
        /// The string with trailing slash removed.
        /// </returns>
        private static string RemoveTrailingSlash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.EndsWith("/") ? input.Substring(0, input.Length - 1) : input;
        }

        #endregion
    }
}