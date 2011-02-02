namespace Facebook.Tests.Fql
{
    using System.Collections.Generic;
    using System.Configuration;
    using Xunit;

    public class FqlReadTests
    {
        private FacebookClient app;
        public FqlReadTests()
        {
            app = new FacebookClient();
            app.AccessToken = ConfigurationManager.AppSettings["AccessToken"];
        }

        [Fact]
        // [TestCategory("RequiresOAuth")]
        public void Read_Friends()
        {
            var query = "SELECT uid, name FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1 = me())";
            dynamic results = app.Query(query);

            Assert.NotNull(results);
            foreach (var item in results)
            {
                Assert.NotEqual(null, item.uid);
                long id;
                long.TryParse(item.uid, out id);
                Assert.True(id > 0);
            }
        }

        [Fact]
        // [TestCategory("RequiresOAuth")]
        public void Read_Permissions()
        {
            string appId = "";
            string appSecret = "";
            var query = string.Format("SELECT {0} FROM permissions WHERE uid == '{1}'", "email", "120625701301347");
            var parameters = new Dictionary<string, object>();
            parameters["query"] = query;
            parameters["method"] = "fql.query";
            parameters["access_token"] = string.Concat(appId, "|", appSecret);
            dynamic result = app.Get(parameters);
            Assert.NotNull(result);
        }
    }
}
