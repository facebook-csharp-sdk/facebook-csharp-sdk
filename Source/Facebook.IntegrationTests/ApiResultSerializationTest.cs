namespace Facebook.Tests.Integration
{
    using System;
    using Xunit;

    public class ApiResultSerializationTest
    {
        private FacebookClient app;
        public ApiResultSerializationTest()
        {
            app = new FacebookClient();
            //app.Session = new FacebookSession
            //{
            //    AccessToken = ConfigurationManager.AppSettings["AccessToken"],
            //};
        }

        [Fact]
        public void Updated_Time_Returns_As_DateTime()
        {
            dynamic result = app.Get("/331218348435");
            var resultType = result.updated_time.GetType();
            Assert.Equal(typeof(DateTime), resultType);
        }

        [Fact]
        public void Url_Formats_Return_Same_Result()
        {
            dynamic pageResult1 = app.Get("/http://www.underarmour.com/shop/us/en/pid1212701%3Fcid%3DSM|Facebook|Like|1212701");
            dynamic pageResult2 = app.Get("/http://www.underarmour.com/shop/us/en/pid1212701?cid=SM|Facebook|Like|1212701");
            dynamic pageResult3 = app.Get("http://www.underarmour.com/shop/us/en/pid1212701%3Fcid%3DSM|Facebook|Like|1212701");
            dynamic pageResult4 = app.Get("http://www.underarmour.com/shop/us/en/pid1212701?cid=SM|Facebook|Like|1212701");

            Assert.True(pageResult1.shares > 0);
            Assert.Equal(pageResult1.shares, pageResult2.shares);
            Assert.Equal(pageResult2.shares, pageResult3.shares);
            Assert.Equal(pageResult3.shares, pageResult4.shares);
        }

        [Fact]
        // [TestCategory("RequiresOAuth")]
        public void Bad_Property_Returns_Null()
        {
            dynamic result = app.Get("/331218348435");
            Assert.NotEqual(null, result.venue);
            Assert.Equal(null, result.venue.badname);
        }
    }
}
