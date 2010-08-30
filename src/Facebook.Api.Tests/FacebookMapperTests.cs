// --------------------------------
// <copyright file="FacebookMapperTests.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Api.Tests
{
    [TestClass]
    public class FacebookMapperTests
    {
        [TestMethod]
        public void Test_Dynamic_Model_Page_Map()
        {
            dynamic page = new DynamicDictionary();
            page.name = "test";
            page.category = "test1";
            Models.Graph.FacebookPage result = FacebookMapper.Map<Schema.Graph.FacebookPage, Models.Graph.FacebookPage>(page);

            Assert.AreEqual(page.name, result.Name);
            Assert.AreEqual(page.category, result.Category);
        }

        [TestMethod]
        public void Test_Dynamic_Schema_Page_Map()
        {
            dynamic page = new DynamicDictionary();
            page.name = "test";
            page.category = "test1";
            Schema.Graph.FacebookPage result = FacebookMapper.Map<Schema.Graph.FacebookPage>(page);

            Assert.AreEqual(page.name, result.name);
            Assert.AreEqual(page.category, result.category);
        }
    }
}
