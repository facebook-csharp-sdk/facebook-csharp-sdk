// --------------------------------
// <copyright file="SessionTests.cs" company="Thuzi, LLC">
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

namespace Facebook.Tests
{
    [TestClass]
    public class SessionTests
    {
        [TestMethod]
        public void Test_Session_QueryString()
        {
            var value = "{\"session_key\":\"2.2vJfEsqRQNKuc9R_kksyOA__.3600.1281474000-14812017\",\"uid\":\"14812017\",\"expires\":1281474000,\"secret\":\"qHP_hdDFtvcj7TDJt8ThrA__\",\"base_domain\":\"cloudapp.net\",\"access_token\":\"145132072179505|2.2vJfEsqRQNKuc9R_kksyOA__.3600.1281474000-14812017|0h7u9pPq8VUbrF6gskSGtxtsmDs.\",\"sig\":\"56319ec02ae84669a286d2466c88cba7\"}";
            FacebookApp app = new FacebookApp();
            var session = app.ParseFromQuerystring(value);
            var validated = app.ValidateSessionObject(session);
        }

    }
}
