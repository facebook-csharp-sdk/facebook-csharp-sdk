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
    public partial class DictionaryExtensionsTest
    {

        [TestMethod]
        public void Merge_Only_First_Provided()
        {
            var first = new Dictionary<string, object>();
            first["prop1"] = "value1";
            first["prop2"] = "value2";
            var result = DictionaryExtensions.Merge(first, null);
            Assert.AreEqual(first["prop1"], result["prop1"]);
            Assert.AreEqual(first["prop2"], result["prop2"]);
        }

        [TestMethod]
        public void Merge_Only_Second_Provided()
        {
            var second = new Dictionary<string, object>();
            second["prop1"] = "value1";
            second["prop2"] = "value2";
            var result = DictionaryExtensions.Merge(null, second);
            Assert.AreEqual(second["prop1"], result["prop1"]);
            Assert.AreEqual(second["prop2"], result["prop2"] );
        }

        [TestMethod]
        public void Merge_Both_Provided()
        {
            var first = new Dictionary<string, object>();
            first["prop1"] = "value1-first";
            first["prop2"] = "value2";
            var second = new Dictionary<string, object>();
            second["prop1"] = "value1-second";
            second["prop3"] = "value3";
            var result = DictionaryExtensions.Merge(first, second);
            Assert.AreEqual(second["prop1"], result["prop1"]);
            Assert.AreEqual(first["prop2"], result["prop2"]);
            Assert.AreEqual(second["prop3"], result["prop3"]);
        }

        [TestMethod]
        public void Merge_Both_Null()
        {
            var result = DictionaryExtensions.Merge(null, null);
            Assert.AreEqual(0, ((IDictionary<string, object>)result).Count);
        }

        [TestMethod]
        public void Merge_Simple()
        {
            var first = new Dictionary<string, object>();
            first["prop1"] = "value1";
            var second = new Dictionary<string, object> ();
            second["prop2"] = "value2";
            var result = DictionaryExtensions.Merge(first, second);
            Assert.AreEqual(first["prop1"], result["prop1"]);
            Assert.AreEqual(second["prop2"], result["prop2"]);
        }

    }
}
