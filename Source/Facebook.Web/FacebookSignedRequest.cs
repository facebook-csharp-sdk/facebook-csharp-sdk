// --------------------------------
// <copyright file="FacebookSignedRequest.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a Facebook signed request.
    /// </summary>
    public sealed class FacebookSignedRequest
    {
        /// <summary>
        /// The actual value of the signed request.
        /// </summary>
        private object data;

        public FacebookSignedRequest(IDictionary<string, object> value)
        {
            if (value is JsonObject)
            {
                this.data = value;
            }
            else
            {
                this.data = FacebookUtils.ToDictionary(value);
            }

            // common
            this.Algorithm = value.ContainsKey("algorithm") ? (string)value["algorithm"] : null;
            this.IssuedAt = value.ContainsKey("issued_at") ? FacebookUtils.FromUnixTime(Convert.ToInt64(value["issued_at"])) : DateTime.MinValue;

            if (value.ContainsKey("payload"))
            {
                // new signed_request: http://developers.facebook.com/docs/authentication/canvas/encryption_proposal
                var payload = (IDictionary<string, object>)value["payload"];

                this.AccessToken = payload.ContainsKey("access_token") ? (string)payload["access_token"] : null;
                this.Expires = payload.ContainsKey("expires_in")
                                   ? FacebookUtils.FromUnixTime(Convert.ToInt64(payload["expires_in"]))
                                   : DateTime.MinValue;

                this.UserId = payload.ContainsKey("user_id") ? (string)payload["user_id"] : null;
            }
            else
            {
                // old signed_request: http://developers.facebook.com/docs/authentication/canvas
                this.UserId = value.ContainsKey("user_id") ? (string)value["user_id"] : null;
                this.AccessToken = value.ContainsKey("oauth_token") ? (string)value["oauth_token"] : null;
                this.Expires = value.ContainsKey("expires")
                                   ? FacebookUtils.FromUnixTime(Convert.ToInt64(value["expires"]))
                                   : DateTime.MinValue;
                this.ProfileId = value.ContainsKey("profile_id") ? (string)value["profile_id"] : null;
            }
        }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime IssuedAt { get; set; }

        /// <summary>
        /// Gets or sets the profile id.
        /// </summary>
        /// <value>The profile id.</value>
        public string ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>The algorithm.</value>
        public string Algorithm { get; set; }

        /// <summary>
        /// Gets actual value of signed request.
        /// </summary>
        public object Data
        {
            get { return this.data; }
        }

        /// <summary>
        /// Parse the signed request string.
        /// </summary>
        /// <param name="signedRequestValue">
        /// The encoded signed request value.
        /// </param>
        /// <param name="secret">
        /// The application secret.
        /// </param>
        /// <param name="maxAge">
        /// The max age.
        /// </param>
        /// <param name="currentTime">
        /// The current time (in unix time format).
        /// </param>
        /// <returns>
        /// The valid signed request.
        /// </returns>
        /// <remarks>
        /// Supports both http://developers.facebook.com/docs/authentication/canvas/encryption_proposal
        /// and http://developers.facebook.com/docs/authentication/canvas
        /// </remarks>
        internal static FacebookSignedRequest Parse(string secret, string signedRequestValue, int maxAge, double currentTime)
        {
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(!String.IsNullOrEmpty(secret));
            Contract.Requires(maxAge >= 0);
            Contract.Requires(currentTime >= 0);
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            // NOTE: currentTime added to parameters to make it unit testable.

            string[] split = signedRequestValue.Split('.');
            if (split.Length != 2)
            {
                // need to have exactly 2 parts
                throw new InvalidOperationException(Properties.Resources.InvalidSignedRequest);
            }

            string encodedSignature = split[0];
            string encodedEnvelope = split[1];

            if (string.IsNullOrEmpty(encodedSignature))
            {
                throw new InvalidOperationException(Properties.Resources.InvalidSignedRequest);
            }

            if (string.IsNullOrEmpty(encodedEnvelope))
            {
                throw new InvalidOperationException(Properties.Resources.InvalidSignedRequest);
            }

            var envelope = (IDictionary<string, object>)JsonSerializer.DeserializeObject(Encoding.UTF8.GetString(FacebookUtils.Base64UrlDecode(encodedEnvelope)));

            string algorithm = (string)envelope["algorithm"];

            if (!algorithm.Equals("AES-256-CBC HMAC-SHA256") && !algorithm.Equals("HMAC-SHA256"))
            {
                // TODO: test
                throw new InvalidOperationException("Invalid signed request. (Unsupported algorithm)");
            }

            byte[] key = Encoding.UTF8.GetBytes(secret);
            byte[] digest = FacebookUtils.ComputeHmacSha256Hash(Encoding.UTF8.GetBytes(encodedEnvelope), key);

            if (!digest.SequenceEqual(FacebookUtils.Base64UrlDecode(encodedSignature)))
            {
                throw new InvalidOperationException("Invalid signed request. (Invalid signature.)");
            }

            IDictionary<string, object> result;

            if (algorithm.Equals("HMAC-SHA256"))
            {
                // for requests that are signed, but not encrypted, we're done
                result = envelope;
            }
            else
            {
                result = new Dictionary<string, object>();

                result["algorithm"] = algorithm;

                long issuedAt = (long)envelope["issued_at"];

                if (issuedAt < currentTime)
                {
                    throw new InvalidOperationException("Invalid signed request. (Too old.)");
                }

                result["issued_at"] = issuedAt;

                // otherwise, decrypt the payload
                byte[] iv = FacebookUtils.Base64UrlDecode((string)envelope["iv"]);
                byte[] rawCipherText = FacebookUtils.Base64UrlDecode((string)envelope["payload"]);
                var plainText = FacebookUtils.DecryptAes256CBCNoPadding(rawCipherText, key, iv);

                var payload = (IDictionary<string, object>)JsonSerializer.DeserializeObject(plainText);
                result["payload"] = payload;
            }

            return new FacebookSignedRequest(result);
        }

        public static FacebookSignedRequest Parse(string secret, string signedRequestValue, int maxAge)
        {
            return Parse(secret, signedRequestValue, maxAge, FacebookUtils.ToUnixTime(DateTime.UtcNow));
        }

        public static FacebookSignedRequest Parse(string secret, string signedRequestValue)
        {
            return Parse(secret, signedRequestValue, 0);
        }
    }
}
