// --------------------------------
// <copyright file="DynamicUtilities.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace Facebook.Utilities
{
    internal static class DictionaryUtils
    {
        /// <summary>
        /// Merges two dictionaries.
        /// </summary>
        /// <param name="first">Default values, only used if second does not contain a value.</param>
        /// <param name="second">Every value of the merged object is used.</param>
        /// <returns>The merged dictionary</returns>
        internal static IDictionary<string, object> Merge(this IDictionary<string, object> first, IDictionary<string, object> second)
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
        internal static string ToJsonQueryString(this IDictionary<string, object> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
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
                    var encodedValue = Uri.EscapeDataString(jsonValue);
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}={1}", key, encodedValue);
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
        internal static string ToJsonQueryString(this IDictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.EndContractBlock();

            return ToJsonQueryString((IDictionary<string, object>)dictionary);
        }

#if !SILVERLIGHT


        /// <summary>
        /// Converts the NameValueCollection to a json formatted query string.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>A Json formatted querystring.</returns>
        internal static string ToJsonQueryString(this NameValueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
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
    }
}
