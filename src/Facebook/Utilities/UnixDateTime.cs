// --------------------------------
// <copyright file="UnixDateTime.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Facebook.Utilities
{
    internal static class UnixDateTime
    {
        public static string ToUnixDateTime(this DateTime dateTime)
        {
            if (dateTime < new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
            {
                throw new ArgumentException("Date time cannot be less than 01/01/1970.");
            }
            Contract.EndContractBlock();

            return UnixDateTime.ToUnixTime(dateTime);
        }

        public static string ToUnixTime(DateTime dateTime)
        {
            if (dateTime < new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
            {
                throw new ArgumentException("Date time cannot be less than 01/01/1970.");
            }
            Contract.EndContractBlock();
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var range = dateTime - epoch;
            return Math.Floor(range.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "unixTime")]
        public static DateTime FromUnixTime(string unixTime)
        {
            if (string.IsNullOrEmpty(unixTime))
            {
                throw new ArgumentNullException("unixTime");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(unixTime, @"[0-9]+"))
            {
                throw new FormatException("Input unixTime was not in the correct format.");
            }
            Contract.EndContractBlock();

            long seconds = long.Parse(unixTime, CultureInfo.InvariantCulture);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(seconds);
        }
    }
}
