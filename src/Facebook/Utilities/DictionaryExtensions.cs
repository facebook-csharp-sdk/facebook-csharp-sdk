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

namespace Facebook.Utilities
{
    internal static class DictionaryExtensions
    {
        /// <summary>
        /// Merges two dictionaries.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IDictionary<string, object> Merge(this IDictionary<string, object> first, IDictionary<string, object> second)
        {
            first = first ?? new Dictionary<string, object>();
            second = second ?? new Dictionary<string, object>();
            return first.Union(second).ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}
