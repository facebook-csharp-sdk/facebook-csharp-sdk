using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Facebook.Tests.Integration
{
    [TestClass]
    public class ApiResultSerializationTest
    {
        private FacebookApp app;
        public ApiResultSerializationTest()
        {
            app = new FacebookApp();
            //app.Session = new FacebookSession
            //{
            //    AccessToken = ConfigurationManager.AppSettings["AccessToken"],
            //};
        }

        [TestMethod]
        public void Updated_Time_Returns_As_DateTime()
        {
            dynamic result = app.Get("/331218348435");
            var resultType = result.updated_time.GetType();
            Assert.AreEqual(typeof(DateTime), resultType);
        }

        [TestMethod]
        public void Url_Formats_Return_Same_Result()
        {
            dynamic pageResult1 = app.Get("/http://www.underarmour.com/shop/us/en/pid1212701%3Fcid%3DSM|Facebook|Like|1212701");
            dynamic pageResult2 = app.Get("/http://www.underarmour.com/shop/us/en/pid1212701?cid=SM|Facebook|Like|1212701");
            dynamic pageResult3 = app.Get("http://www.underarmour.com/shop/us/en/pid1212701%3Fcid%3DSM|Facebook|Like|1212701");
            dynamic pageResult4 = app.Get("http://www.underarmour.com/shop/us/en/pid1212701?cid=SM|Facebook|Like|1212701");

            Assert.IsTrue(pageResult1.shares > 0);
            Assert.AreEqual(pageResult1.shares, pageResult2.shares);
            Assert.AreEqual(pageResult2.shares, pageResult3.shares);
            Assert.AreEqual(pageResult3.shares, pageResult4.shares);
        }

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void Bad_Property_Returns_Null()
        {
            dynamic result = app.Get("/331218348435");
            Assert.AreNotEqual(null, result.venue);
            Assert.AreEqual(null, result.venue.badname);
        }
    }
}
