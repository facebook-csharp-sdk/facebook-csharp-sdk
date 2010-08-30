// --------------------------------
// <copyright file="AutoMapperTests.cs" company="Thuzi, LLC">
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
using AutoMapper;
using Facebook.Schema.Graph;

namespace Facebook.Generated.Tests
{
    [TestClass]
    public class AutoMapperTests
    {
        [TestMethod]
        public void Map_Graph_Page_Schema_To_Model()
        {
            Schema.Graph.FacebookPage source = new Schema.Graph.FacebookPage
            {
                category = "test",
                name = "name"
            };

            var result = source.ToModel();

            Assert.AreEqual(source.name, result.Name);
            Assert.AreEqual(source.category, result.Category);
        }

        [TestMethod]
        public void Map_Graph_Page_Model_To_Schema()
        {
            Models.Graph.FacebookPage source = new Models.Graph.FacebookPage
            {
                Category = "test",
                Name = "name"
            };

            var result = source.ToSchema();

            Assert.AreEqual(source.Name, result.name);
            Assert.AreEqual(source.Category, result.category);
        }
    }
}
