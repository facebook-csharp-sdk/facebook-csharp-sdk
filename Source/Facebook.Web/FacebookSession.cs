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
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents a Facebook session.
    /// </summary>
    [TypeForwardedFrom("Facebook, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
    public sealed class FacebookSession
    {

        private const string HttpContextKey = "facebook_session";

        /// <summary>
        /// The actual value of the facebook session.
        /// </summary>
        private readonly object data;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookSession"/> class.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public FacebookSession(string accessToken)
            : this(new JsonObject { { "access_token", accessToken } })
        {
            Contract.Requires(!string.IsNullOrEmpty(accessToken));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookSession"/> class.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        public FacebookSession(IDictionary<string, object> dictionary)
        {
            Contract.Requires(dictionary != null);

            var data = dictionary is JsonObject ? dictionary : FacebookUtils.ToDictionary(dictionary);

            this.AccessToken = data.ContainsKey("access_token") ? (string)data["access_token"] : null;

            if (!data.ContainsKey("uid") && !string.IsNullOrEmpty(this.AccessToken))
            {
                data.Add("uid", ParseUserIdFromAccessToken(this.AccessToken));
            }

            string sUserId = data.ContainsKey("uid") ? (string)data["uid"] : null;
            long userId = 0;
            long.TryParse(sUserId, out userId);
            this.UserId = userId;

            this.Secret = data.ContainsKey("secret") ? (string)data["secret"] : null;
            this.SessionKey = data.ContainsKey("session_key") ? (string)data["session_key"] : null;

            this.Expires = data.ContainsKey("expires")
                               ? DateTimeConvertor.FromUnixTime(Convert.ToInt64(data["expires"]))
                               : DateTime.MinValue;
            this.Signature = data.ContainsKey("sig") ? (string)data["sig"] : null;
            this.BaseDomain = data.ContainsKey("base_domain") ? (string)data["base_domain"] : null;

            this.data = data;
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public long UserId { get; private set; }

        /// <summary>
        /// Gets the secret.
        /// </summary>
        /// <value>The secret.</value>
        public string Secret { get; private set; }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Gets the session key.
        /// </summary>
        /// <value>The session key.</value>
        public string SessionKey { get; private set; }

        /// <summary>
        /// Gets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime Expires { get; private set; }

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <value>The signature.</value>
        public string Signature { get; private set; }

        /// <summary>
        /// Gets the base domain.
        /// </summary>
        /// <value>The base domain.</value>
        public string BaseDomain { get; private set; }

        /// <summary>
        /// Gets actual value of signed request.
        /// </summary>
        public object Data
        {
            get
            {
                return this.data;
            }
        }

        #region Internal helper methods related to facebook session.

        /// <summary>
        /// Extracts the user id from access token.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        /// <returns>
        /// Returns the user id if successful otherwise null.
        /// </returns>
        internal static string ParseUserIdFromAccessToken(string accessToken)
        {
            Contract.Requires(!string.IsNullOrEmpty(accessToken));

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
                if (!string.IsNullOrEmpty(idPart))
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


        /// <summary>
        /// Gets the facebook session cookie name for the specified facebook application.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <returns>
        /// Returns the name of the cookie name.
        /// </returns>
        internal static string GetCookieName(string appId)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            return string.Concat("fbs_", appId);
        }

        /// <summary>
        /// Gets the facebook session cookie value for the specified application.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="httpRequet">
        /// The http request.
        /// </param>
        /// <returns>
        /// Returns the facebook session cookie value if present otherwise null.
        /// </returns>
        internal static string GetSessionCookieValue(string appId, HttpRequestBase httpRequet)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(httpRequet != null);
            Contract.Requires(httpRequet.Params != null);

            var cookieName = GetCookieName(appId);

            return httpRequet.Params.AllKeys.Contains(cookieName) ? httpRequet.Params[cookieName] : null;
        }

        /// <summary>
        ///  Gets the facebook session from the http request.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <returns>
        /// Returns the facebook session if found, otherwise null.
        /// </returns>
        internal static FacebookSession GetSession(string appId, string appSecret, HttpContextBase httpContext)
        {
            return GetSession(appId, appSecret, httpContext, null);
        }

        /// <summary>
        ///  Gets the facebook session from the http request.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <returns>
        /// Returns the facebook session if found, otherwise null.
        /// </returns>
        internal static FacebookSession GetSession(string appId, string appSecret, HttpContextBase httpContext, FacebookSignedRequest signedRequest)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(!string.IsNullOrEmpty(appSecret));
            Contract.Requires(httpContext != null);
            Contract.Requires(httpContext.Request != null);
            Contract.Requires(httpContext.Request.Params != null);

            // If the session is not null, we explicitly DO NOT want to 
            // read from the cookie. Cookies in iFrames == BAD
            bool readSessionFromCookie = signedRequest == null;

            FacebookSession facebookSession = null;
            var httpRequest = httpContext.Request;
            var items = httpContext.Items;
            if (items[HttpContextKey] == null)
            {
                if (signedRequest == null)
                {
                    // try creating session from signed_request if exists.
                    signedRequest = FacebookSignedRequest.GetSignedRequest(appSecret, httpContext);

                    if (signedRequest != null)
                    {
                        facebookSession = FacebookSession.Create(appSecret, signedRequest);
                    }
                }

                if (readSessionFromCookie && facebookSession == null)
                {
                    // try creating session from cookie if exists.
                    var sessionCookieValue = GetSessionCookieValue(appId, httpRequest);
                    if (!string.IsNullOrEmpty(sessionCookieValue))
                    {
                        facebookSession = FacebookSession.ParseCookieValue(appSecret, sessionCookieValue);
                    }
                }

                if (facebookSession != null)
                {
                    items.Add(HttpContextKey, facebookSession);
                }
            }
            else
            {
                facebookSession = items["facebook_session"] as FacebookSession;
            }

            return facebookSession;
        }

        /// <summary>
        /// Creates a facebook session from a signed request.
        /// </summary>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <param name="signedRequest">
        /// The signed request.
        /// </param>
        /// <returns>
        /// The facebook session.
        /// </returns>
        internal static FacebookSession Create(string appSecret, FacebookSignedRequest signedRequest)
        {
            if (signedRequest == null || String.IsNullOrEmpty(signedRequest.AccessToken))
            {
                return null;
            }

            var dictionary = new Dictionary<string, object>
            {
                { "uid", signedRequest.UserId.ToString() },
                { "access_token", signedRequest.AccessToken },
                { "expires", DateTimeConvertor.ToUnixTime(signedRequest.Expires) }
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

            parts.ForEach(s => payload.Append(s));
            payload.Append(secret);

            var hash = FacebookUtils.ComputerMd5Hash(Encoding.UTF8.GetBytes(payload.ToString()));

            var signature = new StringBuilder();
            foreach (var h in hash)
            {
                signature.Append(h.ToString("x2", CultureInfo.InvariantCulture));
            }

            return signature.ToString();
        }

        #endregion
    }
}
