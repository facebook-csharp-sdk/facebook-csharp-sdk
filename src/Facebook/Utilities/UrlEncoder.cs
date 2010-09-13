using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Utilities
{
    internal static class UrlEncoder
    {
        internal static string EncodeDataString(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            return path.Replace("?", "%3F").Replace("=", "%3D");
        }
    }
}
