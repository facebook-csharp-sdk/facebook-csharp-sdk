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
    /// <summary>
    /// Utilities to convert dates to and from unix time.
    /// </summary>
    public static class DateTimeConvertor
    {
        /// <summary>
        /// Converts a DateTime object to unix time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static double ToUnixTime(this DateTime dateTime)
        {
            Contract.Requires(dateTime >= new DateTime(1970, 1, 1, 0, 0, 0));

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            // For some reason when we use utc time and convert it to 'facebook'
            // time it apears to be in UTC+7. This doesn't seem right though...
            var range = dateTime.AddHours(7) - epoch; 
            return Math.Floor(range.TotalSeconds);
        }

        /// <summary>
        /// Converts a unix time string to a DateTime object.
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(string unixTime)
        {
            double d;
            if (!double.TryParse(unixTime, out d) || d <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid unix time specified.");
            }
            return FromUnixTime(d);
        }

        /// <summary>
        /// Converts a unix time string to a DateTime object.
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(double unixTime)
        {
            Contract.Requires(unixTime > 0);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}
