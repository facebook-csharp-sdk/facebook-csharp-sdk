// --------------------------------
// <copyright file="RestReadTests.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;

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
