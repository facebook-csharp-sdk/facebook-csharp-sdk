namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Helper methods for Facebook.
    /// </summary>
    public class FacebookUtils
    {
        #region DateTime Utils

        /// <summary>
        /// Gets the epoch time.
        /// </summary>
        /// <value>The epoch time.</value>
        public static DateTime Epoch
        {
            get { return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); }
        }

        /// <summary>
        /// Converts a unix time string to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The unix time.</param>
        /// <returns>The DateTime object.</returns>
        public static DateTime FromUnixTime(double unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Converts a unix time string to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The string representation of the unix time.</param>
        /// <returns>The DateTime object.</returns>
        public static DateTime FromUnixTime(string unixTime)
        {
            double d;
            if (!double.TryParse(unixTime, out d))
            {
                return FromUnixTime(0);
            }

            return FromUnixTime(d);
        }

        /// <summary>
        /// Converts a DateTime object to unix time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The unix date time.</returns>
        public static double ToUnixTime(DateTime dateTime)
        {
            Contract.Requires(dateTime >= Epoch);
            return (double)(dateTime.ToUniversalTime() - Epoch).TotalSeconds;
        }

        /// <summary>
        /// Converts a DateTimeOffset object to unix time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The unix date time.</returns>
        public static double ToUnixTime(DateTimeOffset dateTime)
        {
            Contract.Requires(dateTime >= Epoch);
            return (double)(dateTime.ToUniversalTime() - Epoch).TotalSeconds;
        }

        #endregion

        #region Dictionary Utilities

        /// <summary>
        /// Merges two dictionaries.
        /// </summary>
        /// <param name="first">Default values, only used if second does not contain a value.</param>
        /// <param name="second">Every value of the merged object is used.</param>
        /// <returns>The merged dictionary</returns>
        internal static IDictionary<string, object> Merge(IDictionary<string, object> first, IDictionary<string, object> second)
        {
            Contract.Ensures(Contract.Result<IDictionary<string, object>>() != null);

            first = first ?? new Dictionary<string, object>();
            second = second ?? new Dictionary<string, object>();
            var merged = new Dictionary<string, object>();
            foreach (var kvp in second)
            {
                merged.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in first)
            {
                if (!merged.ContainsKey(kvp.Key))
                {
                    merged.Add(kvp.Key, kvp.Value);
                }
            }

            return merged;
        }

        /// <summary>
        /// Converts the dictionary to a json formatted query string.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>A Json formatted querystring.</returns>
        internal static string ToJsonQueryString(IDictionary<string, object> dictionary)
        {
            Contract.Requires(dictionary != null);
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.EndContractBlock();

            StringBuilder sb = new StringBuilder();
            bool isFirst = true;
            foreach (var key in dictionary.Keys)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append("&");
                }

                if (dictionary[key] != null)
                {
                    // Format Object As Json And Remove leading and trailing perenthesis
                    string jsonValue = JsonSerializer.SerializeObject(dictionary[key]);
                    if (jsonValue.StartsWith("\"", StringComparison.Ordinal))
                    {
                        jsonValue = jsonValue.Substring(1, jsonValue.Length - 1);
                    }

                    if (jsonValue.EndsWith("\"", StringComparison.Ordinal))
                    {
                        jsonValue = jsonValue.Substring(0, jsonValue.Length - 1);
                    }

                    if (!String.IsNullOrEmpty(jsonValue))
                    {
                        var encodedValue = UrlEncoder.EscapeUriString(jsonValue);
                        sb.AppendFormat(CultureInfo.InvariantCulture, "{0}={1}", key, encodedValue);
                    }
                }
                else
                {
                    sb.Append(key);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts the dictionary to a json formatted query string.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>A Json formatted querystring.</returns>
        internal static string ToJsonQueryString(IDictionary<string, string> dictionary)
        {
            Contract.Requires(dictionary != null);
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.EndContractBlock();

            return ToJsonQueryString(dictionary.ToDictionary(kv => kv.Key, kv => (object)kv.Value));
        }

        #endregion

        internal static byte[] Base64UrlDecode(string base64UrlSafeString)
        {
            Contract.Requires(!string.IsNullOrEmpty(base64UrlSafeString));
            Contract.Ensures(Contract.Result<byte[]>() != null);

            base64UrlSafeString = base64UrlSafeString.PadRight(base64UrlSafeString.Length + (4 - base64UrlSafeString.Length % 4) % 4, '=');
            base64UrlSafeString = base64UrlSafeString.Replace('-', '+').Replace('_', '/');
            return Convert.FromBase64String(base64UrlSafeString);
        }

        internal static string Base64UrlEncode(byte[] input)
        {
            Contract.Requires(input != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            return Convert.ToBase64String(input).Replace("=", String.Empty).Replace('+', '-').Replace('/', '_');
        }

#if !SILVERLIGHT
        internal static byte[] ComputeHmacSha256Hash(byte[] data, byte[] key)
        {
            Contract.Requires(data != null);
            Contract.Requires(key != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            using (var crypto = new HMACSHA256(key))
            {
                return crypto.ComputeHash(data);
            }
        }

        internal static string DecryptAes256CBCNoPadding(byte[] encryptedData, byte[] key, byte[] iv)
        {
            Contract.Requires(encryptedData != null);
            Contract.Requires(key != null);
            Contract.Requires(iv != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            string result;

            var rijn = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                KeySize = 256
            };

            using (var msDecrypt = new MemoryStream(encryptedData))
            {
                using (var decryptor = rijn.CreateDecryptor(key, iv))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var swDecrypt = new StreamReader(csDecrypt))
                        {
                            result = swDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            rijn.Clear();

            // remove the last \0\0\0\0\0
            if (!string.IsNullOrEmpty(result))
            {
                var index = result.IndexOf('\0');
                if (index > 0)
                {
                    result = result.Substring(0, index);
                }
            }

            return result;
        }

        internal static object ParseSignedRequest(string signedRequestValue, string secret, int maxAge)
        {
            Contract.Requires(!String.IsNullOrEmpty(signedRequestValue));
            Contract.Requires(!String.IsNullOrEmpty(secret));
            Contract.Requires(maxAge >= 0);
            Contract.Requires(signedRequestValue.Contains("."), Properties.Resources.InvalidSignedRequest);

            return ParseSignedRequest(signedRequestValue, secret, maxAge, ToUnixTime(DateTime.UtcNow));
        }

        /// <remarks>
        /// Based on http://developers.facebook.com/docs/authentication/canvas/encryption_proposal
        /// </remarks>
        internal static object ParseSignedRequest(string signedRequestValue, string secret, int maxAge, double currentTime)
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

            var envelope = (Newtonsoft.Json.Linq.JObject)JsonSerializer.DeserializeObject(Encoding.UTF8.GetString(Base64UrlDecode(encodedEnvelope)));

            string algorithm = (string)envelope["algorithm"];

            if (!algorithm.Equals("AES-256-CBC HMAC-SHA256") && !algorithm.Equals("HMAC-SHA256"))
            {
                // TODO: test
                throw new InvalidOperationException("Invalid signed request. (Unsupported algorithm)");
            }

            long issuedAt = (long)envelope["issued_at"];

            if (issuedAt < currentTime)
            {
                throw new InvalidOperationException("Invalid signed request. (Too old.)");
            }

            byte[] key = Encoding.UTF8.GetBytes(secret);
            byte[] digest = ComputeHmacSha256Hash(Encoding.UTF8.GetBytes(encodedEnvelope), key);

            if (!digest.SequenceEqual(Base64UrlDecode(encodedSignature)))
            {
                throw new InvalidOperationException("Invalid signed request. (Invalid signature.)");
            }

            // for requests that are signed, but not encrypted, we"re done
            if (algorithm.Equals("HMAC-SHA256"))
            {
                return envelope;
            }

            // otherwise, decrypt the payload
            byte[] iv = Base64UrlDecode((string)envelope["iv"]);
            byte[] rawCipherText = Base64UrlDecode((string)envelope["payload"]);
            var plainText = DecryptAes256CBCNoPadding(rawCipherText, key, iv);

            var payload = (Newtonsoft.Json.Linq.JObject)JsonSerializer.DeserializeObject(plainText);

            var result = new Newtonsoft.Json.Linq.JObject();
            result["algorithm"] = algorithm;
            result["issued_at"] = issuedAt;
            result["payload"] = payload;

            // return new FacebookSignedRequest(result);
            return result;
        }
#endif
    }

}