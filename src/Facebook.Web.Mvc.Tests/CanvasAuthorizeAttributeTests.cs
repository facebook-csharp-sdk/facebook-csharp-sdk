// --------------------------------
// <copyright file="CanvasAuthorizeAttributeTests.cs" company="Thuzi, LLC">
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
using Facebook.Web.Mvc.Canvas;
using System.Configuration;

namespace Facebook.Web.Mvc.Tests
{
    public class FacebookAuthorizeAttributeHelper : CanvasAuthorizeAttribute
    {

        public string[] GetPermissions(string perms)
        {
            var app = new FacebookApp();
            app.Session = new FacebookSession{ 
                AccessToken = ConfigurationManager.AppSettings["AccessToken"],
                UserId = long.Parse(ConfigurationManager.AppSettings["UserId"]),
            };
            return this.GetCurrentPerms(perms);
        }

    }

    [TestClass]
    public class CanvasAuthorizeAttributeTests
    {
        [TestMethod]
        public void Test_Current_Perms()
        {
            var helper = new FacebookAuthorizeAttributeHelper();
            var result = helper.GetPermissions("email,offline_access");
            Assert.IsTrue(result.Contains("email"));
            Assert.IsTrue(result.Contains("offline_access"));
        }
    }
}
