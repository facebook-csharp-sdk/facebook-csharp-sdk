using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Tests.Fql
{
    [TestClass]
    public class FqlReadTests
    {

        private FacebookApp app;
        public FqlReadTests()
        {
            app = new FacebookApp();
            app.Session = new FacebookSession
            {
                AccessToken = ConfigurationManager.AppSettings["AccessToken"],
            };
        }

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void Read_Friends()
        {
            var query = "SELECT uid, name FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1 = me())";
            dynamic results = app.Fql(query);

            Assert.IsNotNull(results);
            foreach (var item in results)
            {
                Assert.AreNotEqual(null, item.uid);
                long id;
                long.TryParse(item.uid, out id);
                Assert.IsTrue(id > 0);
            }
        }

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void Read_Permissions()
        {
            var query = string.Format("SELECT {0} FROM permissions WHERE uid == '{1}'", "email", "120625701301347");
            var parameters = new Dictionary<string, object>();
            parameters["query"] = query;
            parameters["method"] = "fql.query";
            parameters["access_token"] = string.Concat(app.AppId, "|", app.ApiSecret);
            dynamic result = app.Api(parameters);
            Assert.IsNotNull(result);
        }
    }
}
