// --------------------------------
// <copyright file="RestReadTests.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Tests.Rest
{
    using System.Configuration;
    using System.Dynamic;
    using Xunit;

    public class RestReadTests
    {
        private FacebookClient app;
        public RestReadTests()
        {
            app = new FacebookClient();
            app.AccessToken = ConfigurationManager.AppSettings["AccessToken"];
        }

        [Fact]
        public void user_getInfo_rest_should_throw_oauth()
        {
            dynamic parameters = new ExpandoObject();
            parameters.method = "user.getInfo";
            parameters.uids = "14812017";
            parameters.fields = "first_name,last_name";
            parameters.access_token = "invalidtoken";

            Assert.Throws<FacebookOAuthException>(
                () =>
                    {
                        var result = app.Get(parameters);
                        // var firstName = result[0].first_name;
                    });
        }

    }
}
