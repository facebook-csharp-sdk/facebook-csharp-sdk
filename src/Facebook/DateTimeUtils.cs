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

namespace Facebook
{
    public static class DateTimeUtils
    {
        public static string ToUnixTime(DateTime dateTime)
        {
            Contract.Requires(dateTime >= new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var range = dateTime - epoch;
            return Math.Floor(range.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "unixTime")]
        internal static DateTime FromUnixTime(string unixTime)
        {
            Contract.Requires(!String.IsNullOrEmpty(unixTime));

            long seconds;
            if (!long.TryParse(unixTime, out seconds) || seconds < 0)
            {
                throw new FormatException("The unix time provided was not in the correct format.");
            }
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(seconds);
        }
    }
}
