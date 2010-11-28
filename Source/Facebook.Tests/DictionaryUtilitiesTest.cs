using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Facebook
{
    [TestClass]
    public class DictionaryExtensionsTest
    {
        [TestMethod]
        public void Merge_Only_First_Provided()
        {
            var first = new Dictionary<string, object>();
            first["prop1"] = "value1";
            first["prop2"] = "value2";
            var result = DictionaryUtilities.Merge(first, null);
            Assert.AreEqual(first["prop1"], result["prop1"]);
            Assert.AreEqual(first["prop2"], result["prop2"]);
        }

        [TestMethod]
        public void Merge_Only_Second_Provided()
        {
            var second = new Dictionary<string, object>();
            second["prop1"] = "value1";
            second["prop2"] = "value2";
            var result = DictionaryUtilities.Merge(null, second);
            Assert.AreEqual(second["prop1"], result["prop1"]);
            Assert.AreEqual(second["prop2"], result["prop2"]);
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
            var result = DictionaryUtilities.Merge(first, second);
            Assert.AreEqual(second["prop1"], result["prop1"]);
            Assert.AreEqual(first["prop2"], result["prop2"]);
            Assert.AreEqual(second["prop3"], result["prop3"]);
        }

        [Fact(DisplayName = "Merge: When both inputs are null, the result should not be null")]
        public void Merge_WhenBothInputsAreNull_TheResultShouldNotBeNull()
        {
            IDictionary<string, object> input1 = null;
            IDictionary<string, object> input2 = null;

            var result = DictionaryUtilities.Merge(input1, input2);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: When both inputs are null, the count of result should be 0")]
        public void Merge_WhenBothInputsAreNull_TheCountOfResultShouldBe0()
        {
            IDictionary<string, object> input1 = null;
            IDictionary<string, object> input2 = null;

            var result = DictionaryUtilities.Merge(input1, input2);

            Xunit.Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "Merge: when both inputs are empty and not null, the result should not be null")]
        public void Merge_WhenBothInputsAreEmptyAndNotNull_TheResultShouldNotBeNull()
        {
            var input1 = new Dictionary<string, object>();
            var input2 = new Dictionary<string, object>();

            var result = DictionaryUtilities.Merge(input1, input2);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: when both inputs are empty and not null, the count of result should be 0")]
        public void Merge_WhenBothInputsAreEmptyAndNotNull_TheCountOfResultShouldBe0()
        {
            var input1 = new Dictionary<string, object>();
            var input2 = new Dictionary<string, object>();

            var result = DictionaryUtilities.Merge(input1, input2);

            Xunit.Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "Merge: When first input is null and second is empty and not null, the result should not be null")]
        public void Merge_WhenFirstInputIsNullAndSecondIsEmptyAndNotNull_TheResultShouldNotBeNull()
        {
            IDictionary<string, object> input1 = null;
            IDictionary<string, object> input2 = new Dictionary<string, object>();

            var result = DictionaryUtilities.Merge(input1, input2);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: When first input is null and second is empty and not null, the count of result should be 0")]
        public void Merge_WhenFirstInputIsNullAndSecondIsEmptyAndNotNull_TheCountOfResultShouldBe0()
        {
            IDictionary<string, object> input1 = null;
            IDictionary<string, object> input2 = new Dictionary<string, object>();

            var result = DictionaryUtilities.Merge(input1, input2);

            Xunit.Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "Merge: When first input is empty and not null and second input is null Then the result should not be null")]
        public void Merge_WhenFirstInputIsEmptyAndNotNullAndSecondInputIsNull_ThenTheResultShouldNotBeNull()
        {
            IDictionary<string, object> input1 = new Dictionary<string, object>();
            IDictionary<string, object> input2 = null;

            var result = DictionaryUtilities.Merge(input1, input2);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: When first input is empty and not null and second input is null Then the count of the result should be 0")]
        public void Merge_WhenFirstInputIsEmptyAndNotNullAndSecondInputIsNull_ThenTheCountOfTheResultShouldBe0()
        {
            IDictionary<string, object> input1 = new Dictionary<string, object>();
            IDictionary<string, object> input2 = null;

            var result = DictionaryUtilities.Merge(input1, input2);

            Xunit.Assert.Equal(0, result.Count);
        }

        [TestMethod]
        public void Merge_Simple()
        {
            var first = new Dictionary<string, object>();
            first["prop1"] = "value1";
            var second = new Dictionary<string, object>();
            second["prop2"] = "value2";
            var result = DictionaryUtilities.Merge(first, second);
            Assert.AreEqual(first["prop1"], result["prop1"]);
            Assert.AreEqual(second["prop2"], result["prop2"]);
        }

        [TestMethod]
        public void Dictionary_To_Querystring()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("key1", "value1");
            dict.Add("key2", "value2");
            string actual = DictionaryUtilities.ToJsonQueryString(dict);
            string expected = "key1=value1&key2=value2";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Dictionary2_To_Querystring()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("key1", "value1");
            dict.Add("key2", "value2");
            string actual = DictionaryUtilities.ToJsonQueryString(dict);
            string expected = "key1=value1&key2=value2";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NameValueCollection_To_Querystring()
        {
            var nvc = new NameValueCollection();
            nvc.Add("key1", "value1");
            nvc.Add("key2", "value2");
            string actual = DictionaryUtilities.ToJsonQueryString(nvc);
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
            string actual = DictionaryUtilities.ToJsonQueryString(dict);
            string expected = "key1=value1&key2=value2&key3=%5B%22list1%22%2C%22list2%22%5D";
            Assert.AreEqual(expected, actual);
        }
    }
}