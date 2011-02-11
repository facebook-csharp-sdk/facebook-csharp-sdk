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

    }
}
