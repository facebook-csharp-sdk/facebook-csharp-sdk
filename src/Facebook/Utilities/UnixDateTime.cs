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

namespace Facebook.Utilities
{
    internal static class UnixDateTime
    {

        public static string ToUnixDateTime(this DateTime dateTime)
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
            Contract.EndContractBlock();

            return UnixDateTime.ToUnixTime(dateTime);
        }

        public static string ToUnixTime(DateTime dateTime)
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
            Contract.EndContractBlock();

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var range = dateTime - epoch;
            return Math.Floor(range.TotalSeconds).ToString();
        }

        public static DateTime FromUnixTime(string unixTime)
        {
            if (string.IsNullOrEmpty(unixTime))
            {
                throw new ArgumentNullException("unixTime");
            }
            Contract.EndContractBlock();

            long seconds;
            if (!long.TryParse(unixTime, out seconds))
            {
                throw new FormatException("Input unixTime was not in the correct format.");
            }
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(seconds);
        }
    }
}
