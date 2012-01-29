namespace Facebook.Tests
{
    using System.Diagnostics;
    using Facebook;
    using Xunit;
    using System;

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