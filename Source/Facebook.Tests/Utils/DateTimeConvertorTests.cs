namespace Facebook.Utils.Tests
{
    using System;
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

            var fbUnix = FacebookUtils.FromUnixTime(unixTimeInString);
            var unixTime = FacebookUtils.ToUnixTime(fbUnix);

            Assert.Equal(unixTimeInString, unixTime.ToString());
        }

        [Fact(DisplayName = "Convert to and from unix time (double)")]
        public void ConvertToAndFromUnixTime_Double()
        {
            var unixTimeInDouble = 1213513200;

            var fbUnix = FacebookUtils.FromUnixTime(unixTimeInDouble);
            var unixTime = FacebookUtils.ToUnixTime(fbUnix);

            Assert.Equal(unixTimeInDouble, unixTime);
        }

        [Fact(DisplayName = "FromUnixTime: Given a unix time in double Returns DateTime equivalent")]
        public void FromUnixTime_GivenAUnixTimeInDouble_ReturnsDateTimeEquivalent()
        {
            var unixTimeinDouble = 1284620400;
            var expected = new DateTimeOffset(2010, 9, 16, 0, 0, 0, TimeSpan.FromHours(-7));

            var actual = FacebookUtils.FromUnixTime(unixTimeinDouble);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "FromUnixTime: Given a unix time in string Returns DateTime equivalent")]
        public void FromUnixTime_GivenAUnixTimeInString_ReturnsDateTimeEquivalent()
        {
            var unixTimeInString = "1284620400";
            var expected = new DateTimeOffset(2010, 9, 16, 0, 0, 0, TimeSpan.FromHours(-7));

            var actual = FacebookUtils.FromUnixTime(unixTimeInString);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "ToUnixTime: Given a DateTime object Returns unix time equivalent")]
        public void ToUnixTime_GivenADateTimeObject_ReturnsUnixTimeEquivalent()
        {
            DateTimeOffset dateTime = new DateTimeOffset(2010, 9, 16, 0, 0, 0, TimeSpan.FromHours(-7));
            var expected = 1284620400;

            var actual = FacebookUtils.ToUnixTime(dateTime);

            Assert.Equal(expected, actual);
        }

    }
}
