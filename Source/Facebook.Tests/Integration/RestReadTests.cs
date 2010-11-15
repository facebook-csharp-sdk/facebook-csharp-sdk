// --------------------------------
// <copyright file="RestReadTests.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Facebook.Tests.Rest
{
    [TestClass]
    public class RestReadTests
    {
        private FacebookApp app;
        public RestReadTests()
        {
            app = new FacebookApp();
            app.MaxRetries = 0;
            app.Session = new FacebookSession
            {
                AccessToken = ConfigurationManager.AppSettings["AccessToken"],
            };
        }

        [TestMethod]
        [ExpectedException(typeof(FacebookOAuthException))]
        public void user_getInfo_rest_should_throw_oauth()
        {
            dynamic parameters = new ExpandoObject();
            parameters.method = "user.getInfo";
            parameters.uids = "14812017";
            parameters.fields = "first_name,last_name";
            parameters.access_token = "invalidtoken";

            var result = app.Api(parameters);

            var firstName = result[0].first_name;
            Assert.Fail(); // Should have thown by now
        }

    }
}
