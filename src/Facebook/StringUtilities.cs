using System;

namespace Facebook
{
    internal static class StringUtilities
    {
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
    }
}
