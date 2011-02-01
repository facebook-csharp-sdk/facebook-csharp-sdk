// --------------------------------
// <copyright file="FacebookSession.cs" company="Facebook C# SDK">
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
    using System.Text;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a Facebook session.
    /// </summary>
    public sealed class FacebookSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookSession"/> class.
        /// </summary>
        public FacebookSession()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookSession"/> class.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public FacebookSession(string accessToken)
        {
            this.AccessToken = accessToken;
            if (!string.IsNullOrEmpty(accessToken))
            {
                this.UserId = ParseUserIdFromAccessToken(accessToken);
            }
        }

        public FacebookSession(IDictionary<string, object> dictionary)
        {
            this.UserId = dictionary.ContainsKey("uid") ? (string)dictionary["uid"] : null;
            this.Secret = dictionary.ContainsKey("secret") ? (string)dictionary["secret"] : null;
            this.SessionKey = dictionary.ContainsKey("session_key") ? (string)dictionary["session_key"] : null;
            this.AccessToken = dictionary.ContainsKey("access_token") ? (string)dictionary["access_token"] : null;
            this.Expires = dictionary.ContainsKey("expires") ? FacebookUtils.FromUnixTime(Convert.ToInt64(dictionary["expires"])) : DateTime.MinValue;
            this.Signature = dictionary.ContainsKey("sig") ? (string)dictionary["sig"] : null;
            this.BaseDomain = dictionary.ContainsKey("base_domain") ? (string)dictionary["base_domain"] : null;
        }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>The secret.</value>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the session key.
        /// </summary>
        /// <value>The session key.</value>
        public string SessionKey { get; set; }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        /// <value>The signature.</value>
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the base domain.
        /// </summary>
        /// <value>The base domain.</value>
        public string BaseDomain { get; set; }

        internal static string ParseUserIdFromAccessToken(string accessToken)
        {
            Contract.Requires(!string.IsNullOrEmpty(accessToken));
            // Contract.Ensures(Contract.Result<long>() >= 0);

            /*
             * access_token:
             *   1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc
             *                                               |_______|
             *                                                   |
             *                                                user id
             */

            var accessTokenParts = accessToken.Split('|');

            if (accessTokenParts.Length == 3)
            {
                var idPart = accessTokenParts[1];
                if (!String.IsNullOrEmpty(idPart))
                {
                    var idParts = idPart.Split('-');
                    if (idParts.Length == 2 && !string.IsNullOrEmpty(idParts[1]))
                    {
                        return idParts[1];
                    }
                }
            }

            return null;
        }

#if !SILVERLIGHT

        /// <summary>
        /// Creates a facebook session from a signed request.
        /// </summary>
        /// <param name="signedRequest">The signed request.</param>
        /// <returns>The facebook session.</returns>
        internal static FacebookSession Create(string appSecret, FacebookSignedRequest signedRequest)
        {
            if (signedRequest == null || String.IsNullOrEmpty(signedRequest.AccessToken))
            {
                return null;
            }

            var dictionary = new Dictionary<string, object>
            {
                { "uid", signedRequest.UserId },
                { "access_token", signedRequest.AccessToken },
                { "expires", FacebookUtils.ToUnixTime(signedRequest.Expires) }
            };
            dictionary["sig"] = GenerateSessionSignature(appSecret, dictionary);

            return new FacebookSession(dictionary);
        }

        /// <summary>
        /// Parses the session value from a cookie.
        /// </summary>
        /// <param name="appSecret">
        /// The app Secret.
        /// </param>
        /// <param name="cookieValue">
        /// The session value.
        /// </param>
        /// <returns>
        /// The Facebook session object.
        /// </returns>
        internal static FacebookSession ParseCookieValue(string appSecret, string cookieValue)
        {
            Contract.Requires(!String.IsNullOrEmpty(appSecret));
            Contract.Requires(!String.IsNullOrEmpty(cookieValue));
            Contract.Requires(!cookieValue.Contains(","), "Session value must not contain a comma.");

            // var cookieValue = "\"access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026\"";
            // var result = FacebookSession.Parse("3b4a872617be2ae1932baa1d4d240272", cookieValue);

            // Parse the cookie
            var dictionary = new Dictionary<string, object>();
            var parts = cookieValue.Replace("\"", string.Empty).Split('&');
            foreach (var part in parts)
            {
                if (!string.IsNullOrEmpty(part) && part.Contains("="))
                {
                    var nameValue = part.Split('=');
                    if (nameValue.Length == 2)
                    {
                        var s = FacebookUtils.UrlDecode(nameValue[1]);
                        dictionary.Add(nameValue[0], s);
                    }
                }
            }

            var signature = GenerateSessionSignature(appSecret, dictionary);
            if (dictionary.ContainsKey("sig") && dictionary["sig"].ToString() == signature)
            {
                return new FacebookSession(dictionary);
            }

            return null;
        }

        /// <summary>
        /// Generates a MD5 signature for the facebook session.
        /// </summary>
        /// <param name="secret">
        /// The app secret.
        /// </param>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        /// <returns>
        /// Returns the generated signature.
        /// </returns>
        /// <remarks>
        /// http://developers.facebook.com/docs/authentication/
        /// </remarks>
        internal static string GenerateSessionSignature(string secret, IDictionary<string, object> dictionary)
        {
            Contract.Requires(!string.IsNullOrEmpty(secret));
            Contract.Requires(dictionary != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            var payload = new StringBuilder();

            // sort by the key and remove "sig" if present
            var parts = (from a in dictionary
                         orderby a.Key
                         where a.Key != "sig"
                         select string.Format(CultureInfo.InvariantCulture, "{0}={1}", a.Key, a.Value)).ToList();
            parts.ForEach((s) => { payload.Append(s); });
            payload.Append(appSecret);
            byte[] hash = null;
            using (var md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create())
            {
                if (md5 != null)
                {
                    hash = md5.ComputeHash(Encoding.UTF8.GetBytes(payload.ToString()));
                }
            }

            parts.ForEach(s => payload.Append(s));
            payload.Append(secret);
                throw new InvalidOperationException("Hash is not valid.");
            }

            var hash = FacebookUtils.ComputerMd5Hash(Encoding.UTF8.GetBytes(payload.ToString()));

            var signature = new StringBuilder();
            foreach (var h in hash)
            {
                signature.Append(h.ToString("x2", CultureInfo.InvariantCulture));
            }
            }

            return signature.ToString();
        }
#endif
    }
}
