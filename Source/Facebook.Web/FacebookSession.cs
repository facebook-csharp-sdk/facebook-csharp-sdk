// --------------------------------
// <copyright file="FacebookAppRedirectHttpHandler.cs" company="Thuzi LLC (www.thuzi.com)">
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
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;
    using Facebook.Web;

    /// <summary>
    /// Represents a Facebook session.
    /// </summary>
#if !NET35
    [TypeForwardedFrom("Facebook, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
#endif
    public sealed class FacebookSession
    {
        private const string HttpContextKey = "facebook_session";

        /// <summary>
        /// The actual value of the facebook session.
        /// </summary>
        private readonly object _data;

        private readonly IFacebookApplication _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookSession"/> class.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public FacebookSession(string accessToken)
            : this(new JsonObject { { "access_token", accessToken } })
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException("accessToken");
        }

        public FacebookSession(IDictionary<string, object> dictionary, IFacebookApplication settings)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            if (settings == null)
                throw new ArgumentNullException("settings");

            _settings = settings;
            var data = dictionary is JsonObject ? dictionary : FacebookUtils.ToDictionary(dictionary);

            AccessToken = data.ContainsKey("access_token") ? (string)data["access_token"] : null;

            if (!data.ContainsKey("uid") && !string.IsNullOrEmpty(AccessToken))
            {
                data.Add("uid", ParseUserIdFromAccessToken(AccessToken));
            }

            string sUserId = data.ContainsKey("uid") && data["uid"] != null ? data["uid"].ToString() : null;
            long userId = 0;
            long.TryParse(sUserId, out userId);
            UserId = userId;

            Secret = data.ContainsKey("secret") ? (string)data["secret"] : null;
            SessionKey = data.ContainsKey("session_key") ? (string)data["session_key"] : null;

            if (data.ContainsKey("expires"))
            {
                Expires = data["expires"].ToString() == "0" ? DateTime.MaxValue : DateTimeConvertor.FromUnixTime(Convert.ToDouble(data["expires"]));
            }
            else
            {
                Expires = DateTime.MinValue;
            }

            Signature = data.ContainsKey("sig") ? (string)data["sig"] : null;
            BaseDomain = data.ContainsKey("base_domain") ? (string)data["base_domain"] : null;

            _data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookSession"/> class.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        public FacebookSession(IDictionary<string, object> dictionary)
            : this(dictionary, FacebookApplication.Current)
        {
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

        private string _accessToken;
        private bool _invalidAccessToken;

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken
        {
            get
            {
                if (!_invalidAccessToken && string.IsNullOrEmpty(_accessToken))
                {
                    var data = Data as IDictionary<string, object>;
                    if (data != null && data.ContainsKey("code"))
                    {
                        var oauth = new FacebookOAuthClient(_settings);
                        try
                        {
                            var result = (IDictionary<string, object>)oauth.ExchangeCodeForAccessToken((string)data["code"],
                                                                          new Dictionary<string, object> { { "redirect_uri", null } });
                            _invalidAccessToken = false;
                            _accessToken = (string)result["access_token"];

                            if (result.ContainsKey("expires"))
                            {
                                data["expires"] = result["expires"];
                                Expires = data["expires"].ToString() == "0" ? DateTime.MaxValue : DateTime.UtcNow.AddSeconds(Convert.ToDouble(data["expires"]));
                            }
                            else
                            {
                                Expires = DateTime.MaxValue;
                            }
                        }
                        catch (FacebookOAuthException)
                        {
                            _accessToken = null;
                            _invalidAccessToken = true;
                        }
                    }
                }

                return _accessToken;
            }
            set
            {
                _accessToken = value;
                _invalidAccessToken = false;
            }
        }

        /// <summary>
        /// Gets the session key.
        /// </summary>
        /// <value>The session key.</value>
        public string SessionKey { get; private set; }

        private DateTime _expires;
        /// <summary>
        /// Gets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime Expires
        {
            get
            {
                // this forces to exchange code for access token, incase there is code.
                string accessToken = AccessToken;
                return _expires;
            }
            private set { _expires = value; }
        }

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
                return _data;
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
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException("accessToken");

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
                    var index = idPart.LastIndexOf('-');
                    if (index >= 0)
                    {
                        string id = idPart.Substring(index + 1);
                        if (!string.IsNullOrEmpty(id))
                        {
                            return id;
                        }
                    }
                }
            }
            else
            {
                // we have an encrypted access token
                try
                {
                    var fb = new FacebookClient(accessToken);
                    var result = (IDictionary<string, object>)fb.Get("/me?fields=id");
                    return (string)result["id"];
                }
                catch (FacebookOAuthException)
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the Facebook session cookie name for the specified Facebook application.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <returns>
        /// Returns the name of the cookie name.
        /// </returns>
        internal static string GetCookieName(string appId)
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException("appId");
            return string.Concat("fbs_", appId);
        }

        /// <summary>
        /// Gets the Facebook session cookie value for the specified application.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="httpRequet">
        /// The http request.
        /// </param>
        /// <returns>
        /// Returns the Facebook session cookie value if present otherwise null.
        /// </returns>
        internal static string GetSessionCookieValue(string appId, HttpRequestBase httpRequet)
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException("appId");
            if (httpRequet == null)
                throw new ArgumentNullException("httpRequet");

            var cookieName = GetCookieName(appId);

            return httpRequet.Params.AllKeys.Contains(cookieName) ? httpRequet.Params[cookieName] : null;
        }

        /// <summary>
        ///  Gets the Facebook session from the http request.
        /// </summary>
        /// <param name="settings">
        /// The app settings.
        /// </param>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        /// <returns>
        /// Returns the Facebook session if found, otherwise null.
        /// </returns>
        internal static FacebookSession GetSession(IFacebookApplication settings, HttpContextBase httpContext)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            return GetSession(settings, httpContext, null);
        }

        /// <summary>
        ///  Gets the Facebook session from the http request.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        /// <returns>
        /// Returns the Facebook session if found, otherwise null.
        /// </returns>
        internal static FacebookSession GetSession(IFacebookApplication settings, HttpContextBase httpContext, FacebookSignedRequest signedRequest)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (string.IsNullOrEmpty(settings.AppId))
                throw new Exception("settings.AppId is null.");
            if (string.IsNullOrEmpty(settings.AppSecret))
                throw new Exception("settings.AppSecret is null.");
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

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
                    signedRequest = FacebookSignedRequest.GetSignedRequest(settings.AppId, settings.AppSecret, httpContext);
                }

                if (signedRequest != null)
                {
                    facebookSession = FacebookSession.Create(settings, signedRequest);
                }

                if (readSessionFromCookie && facebookSession == null)
                {
                    // try creating session from cookie if exists.
                    var sessionCookieValue = GetSessionCookieValue(settings.AppId, httpRequest);
                    if (!string.IsNullOrEmpty(sessionCookieValue))
                    {
                        facebookSession = FacebookSession.ParseCookieValue(settings, sessionCookieValue);
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
        /// Creates a Facebook session from a signed request.
        /// </summary>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <param name="signedRequest">
        /// The signed request.
        /// </param>
        /// <returns>
        /// The Facebook session.
        /// </returns>
        internal static FacebookSession Create(IFacebookApplication settings, FacebookSignedRequest signedRequest)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            if (signedRequest == null)
            {
                return null;
            }

            var data = (IDictionary<string, object>)signedRequest.Data;
            if (data == null)
            {
                return null;
            }

            if (!data.ContainsKey("code") && string.IsNullOrEmpty(signedRequest.AccessToken))
            {
                return null;
            }

            var dictionary = new JsonObject
            {
                { "uid", signedRequest.UserId.ToString() }
            };

            if (!string.IsNullOrEmpty(signedRequest.AccessToken))
            {
                dictionary["access_token"] = signedRequest.AccessToken;
            }

            if (data.ContainsKey("code"))
            {
                foreach (var key in data.Keys)
                {
                    dictionary[key] = data[key];
                }
            }
            else
            {
                if (signedRequest.Expires == DateTime.MaxValue)
                {
                    dictionary["expires"] = 0;
                }
                else if (signedRequest.Expires != DateTime.MinValue)
                {
                    dictionary["expires"] = DateTimeConvertor.ToUnixTime(signedRequest.Expires);
                }

                if (settings != null && !string.IsNullOrEmpty(settings.AppSecret))
                {
                    dictionary["sig"] = GenerateSessionSignature(settings.AppSecret, dictionary);
                }
            }

            return new FacebookSession(dictionary, settings);
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
        internal static FacebookSession ParseCookieValue(IFacebookApplication settings, string cookieValue)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (string.IsNullOrEmpty(settings.AppSecret))
                throw new Exception("settings.AppSecret is null.");

            // var cookieValue = "\"access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026\"";
            // var result = FacebookSession.Parse("3b4a872617be2ae1932baa1d4d240272", cookieValue);

            // Parse the cookie
            var dictionary = new JsonObject();
            var parts = cookieValue.Replace("\"", string.Empty).Split('&');
            foreach (var part in parts)
            {
                if (!string.IsNullOrEmpty(part) && part.Contains("="))
                {
                    var nameValue = part.Split('=');
                    if (nameValue.Length == 2)
                    {
                        var s = FluentHttp.HttpHelper.UrlDecode(nameValue[1]);
                        dictionary.Add(nameValue[0], s);
                    }
                }
            }

            var signature = GenerateSessionSignature(settings.AppSecret, dictionary);
            if (dictionary.ContainsKey("sig") && dictionary["sig"].ToString() == signature)
            {
                return new FacebookSession(dictionary, settings);
            }

            return null;
        }

        /// <summary>
        /// Generates a MD5 signature for the Facebook session.
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
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException("secret");
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            var payload = new StringBuilder();

            // sort by the key and remove "sig" if present
            var parts = (from a in dictionary
                         orderby a.Key
                         where a.Key != "sig"
                         select string.Format(CultureInfo.InvariantCulture, "{0}={1}", a.Key, a.Value)).ToList();

            parts.ForEach(s => payload.Append(s));
            payload.Append(secret);

            var hash = FacebookWebUtils.ComputerMd5Hash(Encoding.UTF8.GetBytes(payload.ToString()));

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
