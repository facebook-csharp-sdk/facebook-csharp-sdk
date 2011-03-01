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

    }
}
