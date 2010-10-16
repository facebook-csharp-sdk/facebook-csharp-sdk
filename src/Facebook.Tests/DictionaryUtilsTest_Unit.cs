using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using Facebook;
using System.Collections.Specialized;

namespace Facebook
{
    public partial class DictionaryExtensionsTest
    {

        [TestMethod]
        public void Merge_Only_First_Provided()
        {
            var first = new Dictionary<string, object>();
            first["prop1"] = "value1";
            first["prop2"] = "value2";
            var result = DictionaryUtils.Merge(first, null);
            Assert.AreEqual(first["prop1"], result["prop1"]);
            Assert.AreEqual(first["prop2"], result["prop2"]);
        }

        [TestMethod]
        public void Merge_Only_Second_Provided()
        {
            var second = new Dictionary<string, object>();
            second["prop1"] = "value1";
            second["prop2"] = "value2";
            var result = DictionaryUtils.Merge(null, second);
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
            var result = DictionaryUtils.Merge(first, second);
            Assert.AreEqual(second["prop1"], result["prop1"]);
            Assert.AreEqual(first["prop2"], result["prop2"]);
            Assert.AreEqual(second["prop3"], result["prop3"]);
        }

        [TestMethod]
        public void Merge_Both_Null()
        {
            var result = DictionaryUtils.Merge(null, null);
            Assert.AreEqual(0, ((IDictionary<string, object>)result).Count);
        }

        [TestMethod]
        public void Merge_Simple()
        {
            var first = new Dictionary<string, object>();
            first["prop1"] = "value1";
            var second = new Dictionary<string, object> ();
            second["prop2"] = "value2";
            var result = DictionaryUtils.Merge(first, second);
            Assert.AreEqual(first["prop1"], result["prop1"]);
            Assert.AreEqual(second["prop2"], result["prop2"]);
        }

        [TestMethod]
        public void Dictionary_To_Querystring()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("key1", "value1");
            dict.Add("key2", "value2");
            string actual = DictionaryUtils.ToJsonQueryString(dict);
            string expected = "key1=value1&key2=value2";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Dictionary2_To_Querystring()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("key1", "value1");
            dict.Add("key2", "value2");
            string actual = DictionaryUtils.ToJsonQueryString(dict);
            string expected = "key1=value1&key2=value2";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NameValueCollection_To_Querystring()
        {
            var nvc = new NameValueCollection();
            nvc.Add("key1", "value1");
            nvc.Add("key2", "value2");
            string actual = DictionaryUtils.ToJsonQueryString(nvc);
            string expected = "key1=value1&key2=value2";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Dictionary_With_Object_To_Querystring()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("key1", "value1");
            dict.Add("key2", "value2");
            var list = new List<string>();
            list.Add("list1");
            list.Add("list2");
            dict.Add("key3", list);
            string actual = DictionaryUtils.ToJsonQueryString(dict);
            string expected = "key1=value1&key2=value2&key3=%5B%0D%0A%20%20%22list1%22%2C%20%22list2%22%0D%0A%5D";
            Assert.AreEqual(expected, actual);
        }

    }
}
