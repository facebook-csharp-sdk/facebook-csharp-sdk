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

        /// <summary>
        /// Converts to specified <see cref="DateTime"/> to ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ).
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// Returns the string representation of date time in ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ).
        /// </returns>
        public static string ToIso8601FormattedDateTime(DateTime dateTime)
        {
            Contract.Requires(dateTime != null);
            return dateTime.ToString("o");
        }

        /// <summary>
        /// Converts ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ) date time to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="iso8601DateTime">
        /// The iso 8601 formatted date time.
        /// </param>
        /// <returns>
        /// Returns the <see cref="DateTime"/> equivalent to the ISO-8601 formatted date time. 
        /// </returns>
        public static DateTime FromIso8601FormattedDateTime(string iso8601DateTime)
        {
            Contract.Requires(!string.IsNullOrEmpty(iso8601DateTime));
            return DateTime.ParseExact(iso8601DateTime, "o", System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        #region Dictionary Utils

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
                        var encodedValue = UrlEncode(jsonValue);
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

#if !SILVERLIGHT

        /// <summary>
        /// Converts the NameValueCollection to a json formatted query string.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>A Json formatted querystring.</returns>
        internal static string ToJsonQueryString(System.Collections.Specialized.NameValueCollection collection)
        {
            Contract.Requires(collection != null);
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.EndContractBlock();

            var dictionary = new Dictionary<string, string>();
            collection.AllKeys.ToList().ForEach((key) =>
            {
                if (key != null)
                {
                    dictionary.Add(key, collection[key]);
                }
            });
            return ToJsonQueryString(dictionary);
        }
#endif

        #endregion

        #region String Utils

        internal static string ConvertToString(HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    return "GET";
                case HttpMethod.Post:
                    return "POST";
                case HttpMethod.Delete:
                    return "DELETE";
            }
            throw new InvalidOperationException();
        }

        #endregion

        #region Html Decoding and Encoding

        internal static string HtmlDecode(string input)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.HtmlDecode(input);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.HtmlDecode(input);
#else
            return External.HttpUtility.HtmlDecode(input);
#endif
        }

        internal static string HtmlEncode(string input)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.HtmlEncode(input);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.HtmlEncode(input);
#else
            return External.HttpUtility.HtmlEncode(input);
#endif
        }

        #endregion

        #region Url Decoding, Encoding and other helper methods

        internal static string UrlDecode(string input)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.UrlDecode(input);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.UrlDecode(input);
#else
            return External.HttpUtility.UrlDecode(input);
#endif
        }

        internal static string UrlEncode(string input)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.UrlEncode(input);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.UrlEncode(input);
#else
            return External.HttpUtility.UrlEncode(input);
#endif
        }

        internal static string RemoveTrailingSlash(string url)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return url;
        }

        internal static Uri RemoveTrailingSlash(Uri url)
        {
            Contract.Requires(url != null);
            Contract.Ensures(Contract.Result<Uri>() != null);
            var urlString = RemoveTrailingSlash(url.ToString());
            return new Uri(urlString);
        }

        #endregion

        #region Base64 Url Decoding and Encoding

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

        #endregion

        #region QueryString Utils

        /// <summary>
        /// Parse a URL query and fragment parameters into a key-value bundle.
        /// </summary>
        /// <param name="query">
        /// The URL query to parse.
        /// </param>
        /// <returns>
        /// Returns a dictionary of keys and values for the querystring.
        /// </returns>
        internal static IDictionary<string, object> ParseUrlQueryString(string query)
        {
            Contract.Ensures(Contract.Result<IDictionary<string, object>>() != null);

            var result = new Dictionary<string, object>();

            // if string is null, empty or whitespace
            if (string.IsNullOrEmpty(query) || query.Trim().Length == 0)
            {
                return result;
            }

            string decoded = HtmlDecode(query);
            int decodedLength = decoded.Length;
            int namePos = 0;
            bool first = true;

            while (namePos <= decodedLength)
            {
                int valuePos = -1, valueEnd = -1;
                for (int q = namePos; q < decodedLength; q++)
                {
                    if (valuePos == -1 && decoded[q] == '=')
                    {
                        valuePos = q + 1;
                    }
                    else if (decoded[q] == '&')
                    {
                        valueEnd = q;
                        break;
                    }
                }

                if (first)
                {
                    first = false;
                    if (decoded[namePos] == '?')
                    {
                        namePos++;
                    }
                }

                string name, value;
                if (valuePos == -1)
                {
                    name = null;
                    valuePos = namePos;
                }
                else
                {
                    name = UrlDecode(decoded.Substring(namePos, valuePos - namePos - 1));
                }

                if (valueEnd < 0)
                {
                    namePos = -1;
                    valueEnd = decoded.Length;
                }
                else
                {
                    namePos = valueEnd + 1;
                }

                value = UrlDecode(decoded.Substring(valuePos, valueEnd - valuePos));

                if (!string.IsNullOrEmpty(name))
                {
                    result[name] = value;
                }

                if (namePos == -1)
                {
                    break;
                }
            }

            return result;
        }

        #endregion

#if !SILVERLIGHT

        #region Encryption Decryption Helper methods

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

        #endregion

#endif
    }

}