using System;

namespace Facebook.Utilities
{
    internal static class HttpMethodHelper
    {
        public static string ConvertToString(HttpMethod httpMethod)
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
