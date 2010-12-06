namespace Facebook.Extensions.Tests.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Dynamic;
    using Facebook.Graph;
    using Xunit;

    public class InsightTest
    {
        [Fact]
        public void Get_Insights_By_Facebook_Ids()
        {
            dynamic parameters = new ExpandoObject();
            parameters.ids = String.Join(",", 136963329653478, 113767478670024);
            parameters.period = (int)TimeSpan.FromDays(1).TotalSeconds;
            parameters.endtime = (int)DateTime.UtcNow.Date.ToUnixTime();

            var app = new FacebookApp(ConfigurationManager.AppSettings["AccessToken"]);
            var result = app.Get<List<KeyValuePair<string, InsightCollectionItem>>>("/insights", (IDictionary<string, object>)parameters);

            Assert.Equal(2, result.Count);
        }
    }
}
