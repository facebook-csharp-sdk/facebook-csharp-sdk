namespace Facebook.Tests.FacebookUtils.FromUnixTime
{
    using System;
    using Facebook;
    using Xunit;

    public class GivenAUnixTimeInStringThen
    {
        [Fact]
        public void ReturnsDateTimeEquivalent()
        {
            var unixTimeInString = "1284620400";
            var expected = new DateTimeOffset(2010, 9, 16, 0, 0, 0, TimeSpan.FromHours(-7));

            var actual = DateTimeConvertor.FromUnixTime(unixTimeInString);

            Assert.Equal(expected, actual);
        }
    }
}