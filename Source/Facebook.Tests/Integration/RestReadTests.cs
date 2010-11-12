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

namespace Facebook.Tests.Rest {
    [TestClass]
    public class RestReadTests {

        [TestMethod]
        public void user_getInfo_rest_should_throw_oauth()
        {
            try
            {
                FacebookApp app = new FacebookApp();
                
                dynamic parameters = new ExpandoObject();
                parameters.method = "user.getInfo";
                parameters.uids = "14812017";
                parameters.fields = "first_name,last_name";
                parameters.access_token = "invalidtoken";

                var result = app.Api(parameters);

                var firstName = result[0].first_name;
                Assert.Fail(); // Should have thown by now
            }
            catch (FacebookOAuthException)
            {
                // Correct exception
            }
            catch
            {
                Assert.Fail();
            }

        }
        

    }
}
