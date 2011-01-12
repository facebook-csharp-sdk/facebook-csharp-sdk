// --------------------------------
// <copyright file="FacebookSignedRequest.cs" company="Facebook C# SDK">
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
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Newtonsoft.Json.Linq;
    using System.Text;

    /// <summary>
    /// Rerpesents a Facebook signed request.
    /// </summary>
    public sealed class FacebookSignedRequest
    {

        public FacebookSignedRequest(IDictionary<string, object> value)
        {
            this.UserId = value.ContainsKey("user_id") ? (string)value["user_id"] : null;
            this.AccessToken = value.ContainsKey("oauth_token") ? (string)value["oauth_token"] : null;
            this.Expires = value.ContainsKey("expires") ? DateTimeConvertor.FromUnixTime(Convert.ToInt64(value["expires"])) : DateTime.MinValue;
            this.IssuedAt = value.ContainsKey("issued_at") ? DateTimeConvertor.FromUnixTime(Convert.ToInt64(value["issued_at"])) : DateTime.MinValue;
            this.ProfileId = value.ContainsKey("profile_id") ? (string)value["profile_id"] : null;
            this.Algorithm = value.ContainsKey("algorithm") ? (string)value["algorithm"] : null;

            if (value.ContainsKey("user"))
            {
                var user = (IDictionary<string, object>)value["user"];
                this.User = new FacebookSignedRequestUser
                {
                    Country = user.ContainsKey("country") ? (string)user["country"] : null,
                    Locale = user.ContainsKey("locale") ? (string)user["locale"] : null,
                };
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

        public FacebookSignedRequestUser User { get; set; }

        /// <summary>
        /// Parses the signed request string.
        /// </summary>
        /// <param name="value">The encoded signed request value.</param>
        /// <returns>The valid signed request.</returns>
        public static FacebookSignedRequest Parse(string appSecret, string value)
        {
            Contract.Requires(!String.IsNullOrEmpty(value));
            Contract.Requires(value.Contains("."), Properties.Resources.InvalidSignedRequest);

            string[] parts = value.Split('.');
            var encodedValue = parts[0];
            if (String.IsNullOrEmpty(encodedValue))
            {
                throw new InvalidOperationException(Properties.Resources.InvalidSignedRequest);
            }

            var sig = Base64UrlDecode(encodedValue);
            var payload = parts[1];

            using (var cryto = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(appSecret)))
            {
                var hash = Convert.ToBase64String(cryto.ComputeHash(Encoding.UTF8.GetBytes(payload)));
                var hashDecoded = Base64UrlDecode(hash);
                if (hashDecoded != sig)
                {
                    return null;
                }
            }

            var payloadBase64 = Base64UrlDecode(payload);
            var payloadBytes = Convert.FromBase64String(payloadBase64);
            var payloadJson = Encoding.UTF8.GetString(payloadBytes);
            var data = JsonSerializer.DeserializeObject(payloadJson) as IDictionary<string, object>;
            if (data != null)
            {
                return new FacebookSignedRequest(data);
            }
            return null;
        }

        /// <summary>
        /// Converts the base 64 url encoded string to standard base 64 encoding.
        /// </summary>
        /// <param name="encodedValue">The encoded value.</param>
        /// <returns>The base 64 string.</returns>
        private static string Base64UrlDecode(string encodedValue)
        {
            Contract.Requires(!String.IsNullOrEmpty(encodedValue));

            encodedValue = encodedValue.Replace('+', '-').Replace('/', '_').Trim();
            int pad = encodedValue.Length % 4;
            if (pad > 0)
            {
                pad = 4 - pad;
            }

            encodedValue = encodedValue.PadRight(encodedValue.Length + pad, '=');
            return encodedValue;
        }

    }

    public class FacebookSignedRequestUser
    {
        public string Locale { get; set; }

        public string Country { get; set; }
    }
}
