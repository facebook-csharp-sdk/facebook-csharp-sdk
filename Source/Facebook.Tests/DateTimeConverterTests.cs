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

namespace Facebook.Tests
{
    using System;
    using System.Globalization;
    using Facebook;
    using Xunit;

    public class DateTimeConverterTests
    {
        public class EpochTests
        {
            [Fact]
            public void ShouldBe_1970_1_1()
            {
                var result = DateTimeConvertor.Epoch;

                Assert.Equal(1970, result.Year);
                Assert.Equal(1, result.Month);
                Assert.Equal(1, result.Day);
            }
        }

        public class FromUnixTime
        {
            public class FromDouble
            {
                [Fact]
                public void ShouldConvertToDateTimeCorrectly()
                {
                    var result = DateTimeConvertor.FromUnixTime(1327774473);

                    Assert.Equal(2012, result.Year);
                    Assert.Equal(1, result.Month);
                    Assert.Equal(28, result.Day);

                    Assert.Equal(18, result.Hour);
                    Assert.Equal(14, result.Minute);
                    Assert.Equal(33, result.Second);
                }
            }

            public class FromString
            {
                [Fact]
                public void ShouldConvertToDateTimeCorrectly()
                {
                    var result = DateTimeConvertor.FromUnixTime("1327774473");

                    Assert.Equal(2012, result.Year);
                    Assert.Equal(1, result.Month);
                    Assert.Equal(28, result.Day);

                    Assert.Equal(18, result.Hour);
                    Assert.Equal(14, result.Minute);
                    Assert.Equal(33, result.Second);
                }
            }
        }

        public class ToUnixTime
        {
            public class FromDateTime
            {
                [Fact]
                public void ShouldReturnCorrectUnixTime()
                {
                    var dateTime = new DateTime(2012, 1, 28, 18, 14, 33, DateTimeKind.Utc);

                    var result = DateTimeConvertor.ToUnixTime(dateTime);

                    Assert.Equal(1327774473, result);
                }
            }

            [Fact]
            public void ShouldNotContainDecimal()
            {
                var dateTime = new DateTime(2012, 1, 28, 18, 14, 33, 18, DateTimeKind.Utc);

                var result = DateTimeConvertor.ToUnixTime(dateTime);

                Assert.DoesNotContain(".", result.ToString(CultureInfo.InvariantCulture));
                Assert.Equal("1327774473", result.ToString(CultureInfo.InvariantCulture));
            }

        }

        public class Iso8601FormattedDateTime
        {
            [Fact]
            public void ShouldConvertCorrectly()
            {
                var dateTime = new DateTime(2012, 1, 28, 18, 14, 33, DateTimeKind.Utc);

                var result = DateTimeConvertor.ToIso8601FormattedDateTime(dateTime);

                Console.WriteLine(result);
            }
        }

        public class FromIso8601FormattedDateTime
        {
            [Fact]
            public void FromZ()
            {
                var result = DateTimeConvertor.FromIso8601FormattedDateTime("2012-01-28T18:14:33Z");

                Assert.Equal(2012, result.Year);
                Assert.Equal(1, result.Month);
                Assert.Equal(28, result.Day);

                Assert.Equal(18, result.Hour);
                Assert.Equal(14, result.Minute);
                Assert.Equal(33, result.Second);
            }
        }
    }
}