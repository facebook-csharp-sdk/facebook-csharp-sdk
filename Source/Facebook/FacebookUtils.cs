// --------------------------------
// <copyright file="FacebookUtils.cs" company="Facebook C# SDK">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Helper methods for Facebook.
    /// </summary>
    public static class FacebookUtils
    {
        #region Dictionary Utils

        /// <summary>
        /// Merges two dictionaries.
        /// </summary>
        /// <param name="first">Default values, only used if second does not contain a value.</param>
        /// <param name="second">Every value of the merged object is used.</param>
        /// <returns>The merged dictionary</returns>
        internal static IDictionary<string, object> Merge(IDictionary<string, object> first,
                                                          IDictionary<string, object> second)
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
        /// Convert the object to dictionary.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the dictionary equivalent of the specified object.
        /// </returns>
        internal static IDictionary<string, object> ToDictionary(object parameters)
        {
            Contract.Requires(parameters != null);
            Contract.Ensures(Contract.Result<IDictionary<string, object>>() != null);

            var json = JsonSerializer.Current.SerializeObject(parameters);
            return (IDictionary<string, object>)JsonSerializer.Current.DeserializeObject(json);
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
                    // Format Object As Json And Remove leading and trailing parenthesis
                    string jsonValue = JsonSerializer.Current.SerializeObject(dictionary[key]);
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

            // TODO (review): most likely we an remove this method.

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

        /// <summary>
        /// Gets the string representation of the specified http method.
        /// </summary>
        /// <param name="httpMethod">
        /// The http method.
        /// </param>
        /// <returns>
        /// The string representation of the http method.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throws error if the http method is not Get,Post or Delete.
        /// </exception>
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

        /// <summary>
        /// Html decode the input string.
        /// </summary>
        /// <param name="input">
        /// The string to decode.
        /// </param>
        /// <returns>
        /// The html decoded string.
        /// </returns>
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

        /// <summary>
        /// Html encode the input string.
        /// </summary>
        /// <param name="input">
        /// The string to encode.
        /// </param>
        /// <returns>
        /// The html encoded string.
        /// </returns>
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

        /// <summary>
        /// Url decode the input string.
        /// </summary>
        /// <param name="input">
        /// The string to url decode.
        /// </param>
        /// <returns>
        /// The url decoded string.
        /// </returns>
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

        /// <summary>
        /// Url encode the input string.
        /// </summary>
        /// <param name="input">
        /// The string to url encode.
        /// </param>
        /// <returns>
        /// The url encoded string.
        /// </returns>
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

        /// <summary>
        /// Removes the trailing slash.
        /// </summary>
        /// <param name="input">
        /// The input string to remove the trailing slash from.
        /// </param>
        /// <returns>
        /// The string with trailing slash removed.
        /// </returns>
        internal static string RemoveTrailingSlash(string input)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            if (input.EndsWith("/"))
            {
                input = input.Substring(0, input.Length - 1);
            }

            return input;
        }

        /// <summary>
        /// Removes the trailing slash from the uri.
        /// </summary>
        /// <param name="url">
        /// The url to remove the trailing slash from.
        /// </param>
        /// <returns>
        /// The uri with trailing slash removed.
        /// </returns>
        internal static Uri RemoveTrailingSlash(Uri url)
        {
            Contract.Requires(url != null);
            Contract.Ensures(Contract.Result<Uri>() != null);
            var urlString = RemoveTrailingSlash(url.ToString());
            return new Uri(urlString);
        }

        #endregion

        #region Base64 Url Decoding and Encoding

        /// <summary>
        /// Base64 Url decode.
        /// </summary>
        /// <param name="base64UrlSafeString">
        /// The base 64 url safe string.
        /// </param>
        /// <returns>
        /// The base 64 url decoded string.
        /// </returns>
        internal static byte[] Base64UrlDecode(string base64UrlSafeString)
        {
            Contract.Requires(!string.IsNullOrEmpty(base64UrlSafeString));
            Contract.Ensures(Contract.Result<byte[]>() != null);

            base64UrlSafeString =
                base64UrlSafeString.PadRight(base64UrlSafeString.Length + (4 - base64UrlSafeString.Length % 4) % 4, '=');
            base64UrlSafeString = base64UrlSafeString.Replace('-', '+').Replace('_', '/');
            return Convert.FromBase64String(base64UrlSafeString);
        }

        /// <summary>
        /// Base64 url encode.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// Base64 url encoded string.
        /// </returns>
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

        /// <summary>
        /// Computes the Md5 Hash.
        /// </summary>
        /// <param name="data">
        /// The input data.
        /// </param>
        /// <returns>
        /// The md5 hash.
        /// </returns>
        internal static byte[] ComputerMd5Hash(byte[] data)
        {
            Contract.Requires(data != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            using (var md5 = new MD5CryptoServiceProvider())
            {
                return md5.ComputeHash(data);
            }
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

        /// <summary>
        /// Decrypt encrypted data using the Aes256 CBC no padding algorithm.
        /// </summary>
        /// <param name="encryptedData">
        /// The encrypted data.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="iv">
        /// The iv.
        /// </param>
        /// <returns>
        /// The decrypted string.
        /// </returns>
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules",
            "SA1632:DocumentationTextMustMeetMinimumCharacterLength",
            Justification = "Reviewed. Suppression is OK here.")]
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