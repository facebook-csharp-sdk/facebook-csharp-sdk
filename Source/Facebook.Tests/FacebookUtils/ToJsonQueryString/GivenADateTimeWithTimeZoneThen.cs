
namespace Facebook.Tests.FacebookUtils.ToJsonQueryString
{
    using System;
    using Facebook;
    using Xunit;

    public class GivenADateTimeWithTimeZoneThen
    {
#if !SILVERLIGHT

        [Fact(Skip = "in bangkok, thailand the result is +07:00 and thus fails")]
        public void ShouldSerializeItCorrectlyToISO8601DateTimeFormat()
        {
            // We create the datetime this way so that this test passes in all time zones
            var dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(2010, 6, 15, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Utc.Id, "Central Standard Time");
            var result = JsonSerializer.Current.SerializeObject(dateTime);

            // TODO: fix - in bangkok, thailand the result is +07:00 and thus fails
            Assert.Equal("\"2010-06-14T19:00:00-04:00\"", result);
        }
#endif
    }
}