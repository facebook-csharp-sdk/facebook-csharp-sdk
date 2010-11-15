using System;
using System.Configuration;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void Second_Level_Object_Serialization_Querystring()
        {
            dynamic attachment = new ExpandoObject();
            attachment.name = "my attachment";
            attachment.href = "http://apps.facebook.com/canvas";

            dynamic parameters = new ExpandoObject();
            parameters.method = "stream.publish";
            parameters.message = "my message";
            parameters.attachment = attachment;

            string result = DictionaryUtilities.ToJsonQueryString(parameters);
            Assert.AreEqual("method=stream.publish&message=my%20message&attachment=%7B%22name%22%3A%22my%20attachment%22%2C%22href%22%3A%22http%3A%2F%2Fapps.facebook.com%2Fcanvas%22%7D", result);
        }

        [TestMethod]
        public void Fql_Query_Serialization()
        {
            dynamic parameters = new ExpandoObject();
            parameters.method = "stream.publish";
            parameters.query = "SELECT metric, value FROM insights WHERE object_id=111605505520003 AND end_time=1272276000 AND period=86400 AND metric='page_like_adds'";

            string result = DictionaryUtilities.ToJsonQueryString(parameters);
            Assert.AreEqual("method=stream.publish&query=SELECT%20metric%2C%20value%20FROM%20insights%20WHERE%20object_id%3D111605505520003%20AND%20end_time%3D1272276000%20AND%20period%3D86400%20AND%20metric%3D'page_like_adds'", result);
        }

        [TestMethod]
        public void Date_Time_Serialized_Correctly()
        {
            // We create the datetime this way so that this test passes in all time zones
            var dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(2010, 6, 15, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Utc.Id, "Central Standard Time");
            var result = JsonSerializer.SerializeObject(dateTime);
            Assert.AreEqual("\"2010-06-14T19:00:00-04:00\"", result);
        }
    }
}
