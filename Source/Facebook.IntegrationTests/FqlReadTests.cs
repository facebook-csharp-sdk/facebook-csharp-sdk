//-----------------------------------------------------------------------
// <copyright file="<file>.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

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
