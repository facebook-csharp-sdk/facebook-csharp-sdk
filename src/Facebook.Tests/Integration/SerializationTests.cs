using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Facebook.Utilities;

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
