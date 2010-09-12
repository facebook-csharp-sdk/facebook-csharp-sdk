using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Facebook.Utilities;
using System.Dynamic;

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
        public void Test_Second_Level_Object_Serialization()
        {
            dynamic attachment = new ExpandoObject();
            attachment.name = "my attachment";
            attachment.href = "http://apps.facebook.com/canvas";

            dynamic parameters = new ExpandoObject();
            parameters.method = "stream.publish";
            parameters.message = "my message";
            parameters.attachment = attachment;

            var writer = new JsonWriter();
            writer.WriteValue(parameters);
            var result = writer.Json;
            Assert.AreEqual("{\r\n  \"method\": \"stream.publish\", \"message\": \"my message\", \"attachment\": {\r\n    \"name\": \"my attachment\", \"href\": \"http://apps.facebook.com/canvas\"\r\n  }\r\n}", result);
        }

        [TestMethod]
        public void Date_Time_Serialized_Correctly()
        {
            var writer = new JsonWriter();
            var dateTime = new DateTime(2010, 6, 15);

            writer.WriteValue(dateTime);
            var result = writer.Json;
            Assert.AreEqual("\"2010-06-15T00:00:00-04:00\"", result);
        }

        [TestMethod]
        public void Updated_Time_Returns_As_DateTime()
        {
            dynamic result = app.Api("/me");
            var resultType = result.updated_time.GetType();
            Assert.AreEqual(typeof(DateTime), resultType);
        }

    }
}
