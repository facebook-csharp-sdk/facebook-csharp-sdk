// --------------------------------
// <copyright file="DynamicHelperTests.cs" company="Thuzi, LLC">
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
using Facebook.Utilities;

namespace Facebook.Tests.Utilities
{
    [TestClass]
    public class DynamicHelperTests
    {
        [TestMethod]
        public void Merge_Only_First_Provided()
        {
            dynamic first = new ExpandoObject();
            first.prop1 = "value1";
            first.prop2 = "value2";
            dynamic result = DynamicHelper.Merge(first, null);
            Assert.AreEqual(first.prop1, result.prop1);
            Assert.AreEqual(first.prop2, result.prop2);  
        }

        [TestMethod]
        public void Merge_Only_Second_Provided()
        {
            dynamic second = new ExpandoObject();
            second.prop1 = "value1";
            second.prop2 = "value2";
            dynamic result = DynamicHelper.Merge(null, second);
            Assert.AreEqual(second.prop1, result.prop1);
            Assert.AreEqual(second.prop2, result.prop2);  
        }

        [TestMethod]
        public void Merge_Both_Provided()
        {
            dynamic first = new ExpandoObject();
            first.prop1 = "value1-first";
            first.prop2 = "value2";
            dynamic second = new ExpandoObject();
            second.prop1 = "value1-second";
            second.prop3 = "value3";
            dynamic result = DynamicHelper.Merge(first, second);
            Assert.AreEqual(second.prop1, result.prop1);
            Assert.AreEqual(first.prop2, result.prop2);
            Assert.AreEqual(second.prop3, result.prop3);  
        }

        [TestMethod]
        public void Merge_Both_Null()
        {
            dynamic result = DynamicHelper.Merge(null, null);
            Assert.AreEqual(0, ((IDictionary<string, object>)result).Count);
        }

        [TestMethod]
        public void Merge_Simple()
        {
            dynamic first = new ExpandoObject();
            first.prop1 = "value1";
            dynamic second = new ExpandoObject();
            second.prop2 = "value2";
            dynamic result = DynamicHelper.Merge(first, second);
            Assert.AreEqual(first.prop1, result.prop1);
            Assert.AreEqual(second.prop2, result.prop2);
        }

    }
}
