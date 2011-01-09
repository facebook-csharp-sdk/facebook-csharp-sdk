// --------------------------------
// <copyright file="UrlEncoder.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Facebook
{
    /// <summary>
    /// A Facebook Uri encoding utility.
    /// </summary>
    internal static class UriEncoder
    {
        /// <summary>
        /// Converts a string to its escaped representation.
        /// </summary>
        /// <param name="stringToEscape">The string to escape.</param>
        /// <returns></returns>
        public static string EscapeDataString(string stringToEscape)
        {
            Contract.Requires(!String.IsNullOrEmpty(stringToEscape));

            // Escape the uri string. Also, escap = and ?. Last, the | (pipe) must be double encoded
            string escaped = Uri.EscapeDataString(stringToEscape);
            StringBuilder sb = new StringBuilder(escaped, escaped.Length + (escaped.Length % 10));
            return sb.Replace("%7C", "%257C").ToString();
        }

    }
}
