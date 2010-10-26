using System;
using System.Configuration;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Tests.Integration
{
    [TestClass]
    public class SerializationTests
    {
        private FacebookApp app;
        public SerializationTests()
        {
            app = new FacebookApp();
            app.Session = new FacebookSession
            {
                AccessToken = ConfigurationManager.AppSettings["AccessToken"],
            };
        }

        [TestMethod]
        public void Second_Level_Object_Serialization_JsonWriter()
        {
            dynamic attachment = new ExpandoObject();
            attachment.name = "my attachment";
            attachment.href = "http://apps.facebook.com/canvas";

            dynamic parameters = new ExpandoObject();
            parameters.method = "stream.publish";
            parameters.message = "my message";
            parameters.attachment = attachment;

            JsonWriter writer = new JsonWriter();
            writer.WriteValue(parameters);
            var result = writer.Json;
            Assert.AreEqual("{\r\n  \"method\": \"stream.publish\", \"message\": \"my message\", \"attachment\": {\r\n    \"name\": \"my attachment\", \"href\": \"http://apps.facebook.com/canvas\"\r\n  }\r\n}", result);
        }

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
            Assert.AreEqual("method=stream.publish&message=my%20message&attachment=%7B%0D%0A%20%20%22name%22%3A%20%22my%20attachment%22%2C%20%22href%22%3A%20%22http%3A%2F%2Fapps.facebook.com%2Fcanvas%22%0D%0A%7D", result);
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

            var writer = new JsonWriter();
            writer.WriteValue(dateTime);
            var result = writer.Json;
            Assert.AreEqual("\"2010-06-14T19:00:00-04:00\"", result);
        }

        [TestMethod]
        public void Updated_Time_Returns_As_DateTime()
        {
            dynamic result = app.Api("/331218348435");
            var resultType = result.updated_time.GetType();
            Assert.AreEqual(typeof(DateTime), resultType);
        }

        [TestMethod]
        public void Test_Url_Formats_Return_Same_Result()
        {
            dynamic pageResult1 = app.Api("/http://www.underarmour.com/shop/us/en/pid1212701%3Fcid%3DSM|Facebook|Like|1212701");
            dynamic pageResult2 = app.Api("/http://www.underarmour.com/shop/us/en/pid1212701?cid=SM|Facebook|Like|1212701");
            dynamic pageResult3 = app.Api("http://www.underarmour.com/shop/us/en/pid1212701%3Fcid%3DSM|Facebook|Like|1212701");
            dynamic pageResult4 = app.Api("http://www.underarmour.com/shop/us/en/pid1212701?cid=SM|Facebook|Like|1212701");

            Assert.IsTrue(pageResult1.shares > 0);
            Assert.AreEqual(pageResult1.shares, pageResult2.shares);
            Assert.AreEqual(pageResult2.shares, pageResult3.shares);
            Assert.AreEqual(pageResult3.shares, pageResult4.shares);
        }

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void Bad_Property_Returns_Null()
        {
            dynamic result = app.Api("/331218348435");
            Assert.AreNotEqual(null, result.venue);
            Assert.AreEqual(null, result.venue.badname);
        }
    }
}
