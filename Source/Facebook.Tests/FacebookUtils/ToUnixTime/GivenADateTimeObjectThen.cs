namespace Facebook.Tests.FacebookUtils.ToUnixTime
{
    using System;
    using Facebook;
    using Xunit;

    public class GivenADateTimeObjectThen
    {
        [Fact]
        public void ReturnsUnixTimeEquivalent()
        {
            DateTimeOffset dateTime = new DateTimeOffset(2010, 9, 16, 0, 0, 0, TimeSpan.FromHours(-7));
            var expected = 1284620400;

            var actual = DateTimeConvertor.ToUnixTime(dateTime);

            Assert.Equal(expected, actual);
        }
    }
}