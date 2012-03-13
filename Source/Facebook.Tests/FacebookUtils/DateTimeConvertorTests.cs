//-----------------------------------------------------------------------
// <copyright file="<file>.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

namespace Facebook.Tests.FacebookUtils
{
    using Facebook;
    using Xunit;

    /// <summary>
    ///This is a test class for DateTimeConvertorTest and is intended
    ///to contain all DateTimeConvertorTest Unit Tests
    ///</summary>
    public class DateTimeConvertorTests
    {
        [Fact(DisplayName = "Convert to and from unix time (string)")]
        public void ConvertToAndFromUnixTime_String()
        {
            var unixTimeInString = "1213513200";

            var fbUnix = DateTimeConvertor.FromUnixTime(unixTimeInString);
            var unixTime = DateTimeConvertor.ToUnixTime(fbUnix);

            Assert.Equal(unixTimeInString, unixTime.ToString());
        }

        [Fact(DisplayName = "Convert to and from unix time (double)")]
        public void ConvertToAndFromUnixTime_Double()
        {
            var unixTimeInDouble = 1213513200;

            var fbUnix = DateTimeConvertor.FromUnixTime(unixTimeInDouble);
            var unixTime = DateTimeConvertor.ToUnixTime(fbUnix);

            Assert.Equal(unixTimeInDouble, unixTime);
        }

        [Fact(DisplayName = "Convert from ISO8601 formatted date")]
        public void FromIso8601FormattedDateTime()
        {
            var result = DateTimeConvertor.FromIso8601FormattedDateTime("2011-08-04T07:00:00+0000");

            Assert.Equal(result.Year, 2011);
            Assert.Equal(result.Month, 8);
            Assert.Equal(result.Day, 4);

            Assert.Equal(result.Hour, 7);
            Assert.Equal(result.Minute, 0);
            Assert.Equal(result.Second, 0);
        }

        [Fact(DisplayName = "Convert to ISO8601 formatted date")]
        public void ToIso8601FormattedDateTime()
        {
            var dateTime = DateTimeConvertor.FromIso8601FormattedDateTime("2011-08-04T07:00:00+0000");

            var result = DateTimeConvertor.ToIso8601FormattedDateTime(dateTime);

            Assert.Equal(result, "2011-08-04T07:00:00Z");
        }
    }
}
