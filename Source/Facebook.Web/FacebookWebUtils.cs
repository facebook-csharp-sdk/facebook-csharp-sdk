

namespace Facebook.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security.Cryptography;

    internal class FacebookWebUtils
    {
        #region Base64 Url Decoding/Encoding

        /// <summary>
        /// Base64 Url decode.
        /// </summary>
        /// <param name="base64UrlSafeString">
        /// The base 64 url safe string.
        /// </param>
        /// <returns>
        /// The base 64 url decoded string.
        /// </returns>
        public static byte[] Base64UrlDecode(string base64UrlSafeString)
        {
            if (string.IsNullOrEmpty(base64UrlSafeString))
                throw new ArgumentNullException("base64UrlSafeString");

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
        public static string Base64UrlEncode(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            return Convert.ToBase64String(input).Replace("=", String.Empty).Replace('+', '-').Replace('/', '_');
        }

        #endregion

        #region Encryption/Decryption

        /// <summary>
        /// Computes the Md5 Hash.
        /// </summary>
        /// <param name="data">
        /// The input data.
        /// </param>
        /// <returns>
        /// The md5 hash.
        /// </returns>
        public static byte[] ComputerMd5Hash(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

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
        public static byte[] ComputeHmacSha256Hash(byte[] data, byte[] key)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (key == null)
                throw new ArgumentNullException("key");

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
            if (encryptedData == null)
                throw new ArgumentNullException("encryptedData");
            if (key == null)
                throw new ArgumentNullException("key");
            if (iv == null)
                throw new ArgumentNullException("iv");

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

        #region Url Helpers

        public static string RemoveStartingSlash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // if not null or empty
            if (input[0] == '/')
            {
                // if starts with / remove it
                return input.Length > 1 ? input.Substring(1) : string.Empty;
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
            if (url == null)
                throw new ArgumentNullException("url");

            var urlString = RemoveTrailingSlash(url.ToString());
            return new Uri(urlString);
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

        #endregion
    }
}