using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using System.Configuration;
using Facebook.Graph;

namespace Facebook.Extensions.Tests.Graph
{
    [TestClass]
    public class InsightTest
    {
        [TestMethod]
        public void Get_Insights_By_Facebook_Ids()
        {

            dynamic parameters = new ExpandoObject();
            parameters.ids = String.Join(",", 136963329653478, 113767478670024);
            parameters.period = (int)TimeSpan.FromDays(1).TotalSeconds;
            parameters.endtime = (int)DateTime.UtcNow.Date.ToUnixTime();

            var app = new FacebookApp(ConfigurationManager.AppSettings["AccessToken"]);
            var result = app.Api<List<KeyValuePair<string, InsightCollectionItem>>>("/insights", (IDictionary<string, object>)parameters);

            Assert.AreEqual(2, result.Count);

        }
    }
}
