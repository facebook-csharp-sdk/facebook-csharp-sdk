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
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Represents a Facebook signed request.
    /// </summary>
    public sealed class FacebookSignedRequest
    {
        /// <summary>
        /// The actual value of the signed request.
        /// </summary>
        private object data;

        /// <summary>
        /// The access token.
        /// </summary>
        private string accessToken;

        /// <summary>
        /// The user id.
        /// </summary>
        private string userId;

        /// <summary>
        /// The profile id.
        /// </summary>
        private string profileId;

        /// <summary>
        /// The expires.
        /// </summary>
        private DateTime expires;

        /// <summary>
        /// The issued at.
        /// </summary>
        private DateTime issuedAt;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookSignedRequest"/> class.
        /// </summary>
        /// <param name="data">
        /// The signed request data.
        /// </param>
        public FacebookSignedRequest(IDictionary<string, object> data)
        {
            Contract.Requires(data != null);

            this.Data = data;
        }

        /// <summary>
        /// Gets actual value of signed request.
        /// </summary>
        public object Data
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);
                return this.data;
            }

            private set
            {
                Contract.Requires(value != null);

                var data = (IDictionary<string, object>)(value is JsonObject ? value : FacebookUtils.ToDictionary(value));

                this.issuedAt = data.ContainsKey("issued_at") ? FacebookUtils.FromUnixTime(Convert.ToInt64(data["issued_at"])) : DateTime.MinValue;

                if (data.ContainsKey("payload"))
                {
                    // new signed_request: http://developers.facebook.com/docs/authentication/canvas/encryption_proposal
                    var payload = (IDictionary<string, object>)data["payload"];

                    if (payload != null)
                    {
                        this.accessToken = payload.ContainsKey("access_token") ? (string)payload["access_token"] : null;
                        this.expires = payload.ContainsKey("expires_in")
                                           ? FacebookUtils.FromUnixTime(Convert.ToInt64(payload["expires_in"]))
                                           : DateTime.MinValue;
                        this.userId = payload.ContainsKey("user_id") ? (string)payload["user_id"] : null;
                        this.profileId = data.ContainsKey("profile_id") ? (string)data["profile_id"] : null;
                    }
                }
                else
                {
                    // old signed_request: http://developers.facebook.com/docs/authentication/canvas
                    this.userId = data.ContainsKey("user_id") ? (string)data["user_id"] : null;
                    this.accessToken = data.ContainsKey("oauth_token") ? (string)data["oauth_token"] : null;
                    this.expires = data.ContainsKey("expires")
                                       ? FacebookUtils.FromUnixTime(Convert.ToInt64(data["expires"]))
                                       : DateTime.MinValue;
                    this.profileId = data.ContainsKey("profile_id") ? (string)data["profile_id"] : null;
                }

                this.data = data;
            }
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        public string AccessToken
        {
            get { return this.accessToken; }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public string UserId
        {
            get { return this.userId; }
        }

        /// <summary>
        /// Gets the profile id.
        /// </summary>
        public string ProfileId
        {
            get { return this.profileId; }
        }

        /// <summary>
        /// Gets the expires.
        /// </summary>
        public DateTime Expires
        {
            get { return this.expires; }
        }

        /// <summary>
        /// Gets the issued at.
        /// </summary>
        public DateTime IssuedAt
        {
            get { return this.issuedAt; }
        }

        /// <summary>
        /// Try parsing the signed request.
        /// </summary>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <param name="signedRequestValue">
        /// The signed request value.
        /// </param>
        /// <param name="signedRequest">
        /// The signed request.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public static bool TryParse(string secret, string signedRequestValue, out FacebookSignedRequest signedRequest)
        {
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(!String.IsNullOrEmpty(secret));
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            var result = TryParse(secret, signedRequestValue, 0, FacebookUtils.ToUnixTime(DateTime.UtcNow), false);
            signedRequest = result == null ? null : new FacebookSignedRequest(result);
            return result != null;
        }

        /// <summary>
        /// Try parsing the signed request.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <param name="signedRequestValue">
        /// The signed request value.
        /// </param>
        /// <param name="signedRequest">
        /// The signed request.
        /// </param>
        /// <returns>
        /// Returns true if parsing was successful otherwise false.
        /// </returns>
        public static bool TryParse(IFacebookApplication facebookApplication, string signedRequestValue, out FacebookSignedRequest signedRequest)
        {
            Contract.Requires(facebookApplication != null);
            Contract.Requires(!string.IsNullOrEmpty(facebookApplication.AppSecret));
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            return TryParse(facebookApplication.AppSecret, signedRequestValue, out signedRequest);
        }

        /// <summary>
        /// Parse the signed request.
        /// </summary>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <param name="signedRequestValue">
        /// The signed request value.
        /// </param>
        /// <returns>
        /// Returns the signed request.
        /// </returns>
        public static FacebookSignedRequest Parse(string secret, string signedRequestValue)
        {
            Contract.Requires(!string.IsNullOrEmpty(secret));
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            var result = TryParse(secret, signedRequestValue, 0, FacebookUtils.ToUnixTime(DateTime.UtcNow), true);
            return result == null ? null : new FacebookSignedRequest(result);
        }

        /// <summary>
        /// Parse the signed request.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <param name="signedRequestValue">
        /// The signed request value.
        /// </param>
        /// <returns>
        /// Returns the signed request.
        /// </returns>
        public static FacebookSignedRequest Parse(IFacebookApplication facebookApplication, string signedRequestValue)
        {
            Contract.Requires(facebookApplication != null);
            Contract.Requires(!string.IsNullOrEmpty(facebookApplication.AppSecret));
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            return Parse(facebookApplication.AppSecret, signedRequestValue);
        }

        /// <summary>
        /// Parse the signed request.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// Returns the signed request.
        /// </returns>
        public static FacebookSignedRequest Parse(IFacebookApplication facebookApplication, HttpRequestBase request)
        {
            return request.Params.AllKeys.Contains("signed_request")
                       ? Parse(facebookApplication, request.Params["signed_request"])
                       : null;
        }

        /// <summary>
        /// Try parsing the signed request.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="signedRequest">
        /// The signed request.
        /// </param>
        /// <returns>
        /// Returns true if parsing is successful otherwise false.
        /// </returns>
        public static bool TryParse(IFacebookApplication facebookApplication, HttpRequestBase request, out FacebookSignedRequest signedRequest)
        {
            signedRequest = null;

            return request.Params.AllKeys.Contains("signed_request") &&
                   TryParse(facebookApplication, request.Params["signed_request"], out signedRequest);
        }

        /// <summary>
        /// Parse the signed request string.
        /// </summary>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <param name="signedRequestValue">
        /// The signed request value.
        /// </param>
        /// <param name="maxAge">
        /// The max age.
        /// </param>
        /// <param name="currentTime">
        /// The current time.
        /// </param>
        /// <param name="throws">
        /// The throws.
        /// </param>
        /// <returns>
        /// The FacebookSignedRequest.
        /// </returns>
        internal static IDictionary<string, object> TryParse(string secret, string signedRequestValue, int maxAge, double currentTime, bool throws)
        {
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(!String.IsNullOrEmpty(secret));
            Contract.Requires(maxAge >= 0);
            Contract.Requires(currentTime >= 0);
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            try
            {
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

                return result;
            }
            catch
            {
                if (throws)
                {
                    throw;
                }

                return null;
            }
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        [ContractInvariantMethod]
        private void InvariantObject()
        {
            Contract.Invariant(this.data != null);
        }
    }
}