namespace Facebook.Tests
{
    using System;
    using System.Dynamic;
    using Xunit;

    public class SerializationTest
    {
        [Fact(DisplayName = "ToJsonQueryString: Given an object with a second level Should serialize it correctly")]
        public void ToJsonQueryString_GivenAnObjectWithASecondLevel_ShouldSerializeItCorrectly()
        {
            dynamic attachment = new ExpandoObject();
            attachment.name = "my attachment";
            attachment.href = "http://apps.facebook.com/canvas";

            dynamic parameters = new ExpandoObject();
            parameters.method = "stream.publish";
            parameters.message = "my message";
            parameters.attachment = attachment;

            string result = FacebookUtils.ToJsonQueryString(parameters);
            Assert.Equal("method=stream.publish&message=my%20message&attachment=%7B%22name%22%3A%22my%20attachment%22%2C%22href%22%3A%22http%3A%2F%2Fapps.facebook.com%2Fcanvas%22%7D", result);
        }

        [Fact(DisplayName = "ToJsonQueryString: Given a FQL query Should serialize it correctly")]
        public void ToJsonQueryString_GivenAFQLQuery_ShouldSerializeItCorrectly()
        {
            dynamic parameters = new ExpandoObject();
            parameters.method = "stream.publish";
            parameters.query = "SELECT metric, value FROM insights WHERE object_id=111605505520003 AND end_time=1272276000 AND period=86400 AND metric='page_like_adds'";

            string result = FacebookUtils.ToJsonQueryString(parameters);
            Assert.Equal("method=stream.publish&query=SELECT%20metric%2C%20value%20FROM%20insights%20WHERE%20object_id%3D111605505520003%20AND%20end_time%3D1272276000%20AND%20period%3D86400%20AND%20metric%3D'page_like_adds'", result);
        }

        [Fact(DisplayName = "SerializeObject: Given a DateTime with time zone Should serialize it correctly to ISO-8601 date time format")]
        public void SerializeObject_GivenADateTimeWithTimeZone_ShouldSerializeItCorrectlyToISO8601DateTimeFormat()
        {
            // We create the datetime this way so that this test passes in all time zones
            var dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(2010, 6, 15, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Utc.Id, "Central Standard Time");
            var result = JsonSerializer.SerializeObject(dateTime);

            // TODO: fix - in bangkok, thailand the result is +07:00 and thus fails
            Assert.Equal("\"2010-06-14T19:00:00-04:00\"", result);
        }
    }
}
