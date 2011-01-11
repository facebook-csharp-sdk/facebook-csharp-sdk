namespace Facebook
{
    using System;

    internal class FacebookUtils
    {
        internal static byte[] Base64UrlDecode(string base64UrlSafeString)
        {
            base64UrlSafeString = base64UrlSafeString.PadRight(base64UrlSafeString.Length + (4 - base64UrlSafeString.Length % 4) % 4, '=');
            base64UrlSafeString = base64UrlSafeString.Replace('-', '+').Replace('_', '/');
            return Convert.FromBase64String(base64UrlSafeString);
        }
    }
}