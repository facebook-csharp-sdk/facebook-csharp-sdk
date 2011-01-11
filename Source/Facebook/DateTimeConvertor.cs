// --------------------------------
// <copyright file="DateTimeConvertor.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Utilities to convert dates to and from unix time.
    /// </summary>
    public static class DateTimeConvertor
    {
        /// <summary>
        /// Converts a DateTimeOffset object to unix time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The unix date time.</returns>
        public static double ToUnixTime(this DateTimeOffset dateTime)
        {
            Contract.Requires(dateTime >= FacebookUtils.Epoch);
            return (double)(dateTime.ToUniversalTime() - FacebookUtils.Epoch).TotalSeconds;
        }
    }
}
