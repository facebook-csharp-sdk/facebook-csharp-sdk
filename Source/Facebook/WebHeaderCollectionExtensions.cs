using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Facebook
{

    internal static class WebHeaderCollectionExtensions
    {

        public static string TryGet(this WebHeaderCollection headers, string value, bool ignoreCase, string defaultValue)
        {
            string desiredValue = ignoreCase ? value.ToLower() : value;
            foreach (String key in headers.AllKeys)
            {
                string curKey = ignoreCase ? key.ToLower() : key;
                if (desiredValue == curKey)
                {
                    return (headers[curKey]);
                }
            }
            return (defaultValue);
        }

    }

}
