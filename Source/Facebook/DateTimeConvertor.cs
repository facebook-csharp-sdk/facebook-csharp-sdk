// --------------------------------
// <copyright file="DateTimeConvertor.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Utilities to convert dates to and from unix time.
    /// </summary>
    public static class DateTimeConvertor
    {
        private static readonly string[] Iso8601Format = new[]
                                                             {
                                                                 "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
                                                                 @"yyyy-MM-dd\THH:mm:ssK"
                                                             };
        /// <summary>
        /// Gets the epoch time.
        /// </summary>
        /// <value>The epoch time.</value>
        public static DateTime Epoch
        {
            get { return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); }
        }

        /// <summary>
        /// Converts a unix time string to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The unix time.</param>
        /// <returns>The DateTime object.</returns>
        public static DateTime FromUnixTime(double unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Converts a unix time string to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The unix time.</param>
        /// <returns>The DateTime object.</returns>
        [Obsolete("Use double overload instead.")]
        public static DateTime FromUnixTime(long unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Converts a unix time string to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The string representation of the unix time.</param>
        /// <returns>The DateTime object.</returns>
        public static DateTime FromUnixTime(string unixTime)
        {
            double d;
            if (!double.TryParse(unixTime, out d))
            {
                return FromUnixTime(0D);
            }

            return FromUnixTime(d);
        }

        /// <summary>
        /// Converts a DateTime object to unix time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The unix date time.</returns>
        public static double ToUnixTime(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime() - Epoch).TotalSeconds;
        }

        /// <summary>
        /// Converts a DateTimeOffset object to unix time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The unix date time.</returns>
        public static double ToUnixTime(DateTimeOffset dateTime)
        {
            return (dateTime.ToUniversalTime() - Epoch).TotalSeconds;
        }

        /// <summary>
        /// Converts to specified <see cref="DateTime"/> to ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ).
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// Returns the string representation of date time in ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ).
        /// </returns>
        public static string ToIso8601FormattedDateTime(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString(Iso8601Format[0], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ) date time to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="iso8601DateTime">
        /// The ISO-8601 formatted date time.
        /// </param>
        /// <returns>
        /// Returns the <see cref="DateTime"/> equivalent to the ISO-8601 formatted date time. 
        /// </returns>
        public static DateTime FromIso8601FormattedDateTime(string iso8601DateTime)
        {
            if(string.IsNullOrEmpty(iso8601DateTime))
                throw new ArgumentNullException("iso8601DateTime");
            
            return DateTime.ParseExact(iso8601DateTime, Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }
    }
}
