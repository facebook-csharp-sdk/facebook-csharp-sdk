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
        [Fact(DisplayName = "Merge: When first input contains values and second is null The the result should not be null")]
        public void Merge_WhenFirstInputContainsValuesAndSecondIsNull_TheTheResultShouldNotBeNull()
        {
            IDictionary<string, object> first = new Dictionary<string, object>
                                                    {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };
            IDictionary<string, object> second = null;

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: When first input contains values and second is null The the count of result should be equal to the count of first input")]
        public void Merge_WhenFirstInputContainsValuesAndSecondIsNull_TheTheCountOfResultShouldBeEqualToTheCountOfFirstInput()
        {
            IDictionary<string, object> first = new Dictionary<string, object>
                                                    {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };
            IDictionary<string, object> second = null;

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.Equal(first.Count, result.Count);
        }

        [Fact(DisplayName = "Merge: When first input contains values and second is null The values of result should be same as values of first input")]
        public void Merge_WhenFirstInputContainsValuesAndSecondIsNull_TheValuesOfResultShouldBeSameAsValuesOfFirstInput()
        {
            IDictionary<string, object> first = new Dictionary<string, object>
                                                    {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };
            IDictionary<string, object> second = null;

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.Equal(first["prop1"], result["prop1"]);
            Xunit.Assert.Equal(first["prop2"], result["prop2"]);
        }

        [Fact(DisplayName = "Merge: When first input is null and second contains values Then the result should not be null")]
        public void Merge_WhenFirstInputIsNullAndSecondContainsValues_ThenTheResultShouldNotBeNull()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = new Dictionary<string, object>
                                                     {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };


            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: When first input is null and second contains values Then the count of the result should be equal to count of second input")]
        public void Merge_WhenFirstInputIsNullAndSecondContainsValues_ThenTheCountOfTheResultShouldBeEqualToCountOfSecondInput()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = new Dictionary<string, object>
                                                     {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };


            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.Equal(second.Count, result.Count);
        }

        [Fact(DisplayName = "Merge: When first input is null and second contains values Then the values of result should be same as values of second input")]
        public void Merge_WhenFirstInputIsNullAndSecondContainsValues_ThenTheValuesOfResultShouldBeSameAsValuesOfSecondInput()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = new Dictionary<string, object>
                                                    {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.Equal(second["prop1"], result["prop1"]);
            Xunit.Assert.Equal(second["prop2"], result["prop2"]);
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
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = null;

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: When both inputs are null, the count of result should be 0")]
        public void Merge_WhenBothInputsAreNull_TheCountOfResultShouldBe0()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = null;

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "Merge: when both inputs are empty and not null, the result should not be null")]
        public void Merge_WhenBothInputsAreEmptyAndNotNull_TheResultShouldNotBeNull()
        {
            var first = new Dictionary<string, object>();
            var second = new Dictionary<string, object>();

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: when both inputs are empty and not null, the count of result should be 0")]
        public void Merge_WhenBothInputsAreEmptyAndNotNull_TheCountOfResultShouldBe0()
        {
            var first = new Dictionary<string, object>();
            var second = new Dictionary<string, object>();

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "Merge: When first input is null and second is empty and not null, the result should not be null")]
        public void Merge_WhenFirstInputIsNullAndSecondIsEmptyAndNotNull_TheResultShouldNotBeNull()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = new Dictionary<string, object>();

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: When first input is null and second is empty and not null, the count of result should be 0")]
        public void Merge_WhenFirstInputIsNullAndSecondIsEmptyAndNotNull_TheCountOfResultShouldBe0()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = new Dictionary<string, object>();

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "Merge: When first input is empty and not null and second input is null Then the result should not be null")]
        public void Merge_WhenFirstInputIsEmptyAndNotNullAndSecondInputIsNull_ThenTheResultShouldNotBeNull()
        {
            IDictionary<string, object> first = new Dictionary<string, object>();
            IDictionary<string, object> second = null;

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: When first input is empty and not null and second input is null Then the count of the result should be 0")]
        public void Merge_WhenFirstInputIsEmptyAndNotNullAndSecondInputIsNull_ThenTheCountOfTheResultShouldBe0()
        {
            IDictionary<string, object> first = new Dictionary<string, object>();
            IDictionary<string, object> second = null;

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "Merge: When both the combination of first input and second input contains unique keys Then the result should not be null")]
        public void Merge_WhenBothTheCombinationOfFirstInputAndSecondInputContainsUniqueKeys_ThenTheResultShouldNotBeNull()
        {
            var first = new Dictionary<string, object> { { "prop1", "value1" } };
            var second = new Dictionary<string, object> { { "prop2", "value2" } };

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.NotNull(result);
        }

        [Fact(DisplayName = "Merge: When both the combination of first input and second input contains unique keys Then the count of result should be equal to count of first and second inputs")]
        public void Merge_WhenBothTheCombinationOfFirstInputAndSecondInputContainsUniqueKeys_ThenTheCountOfResultShouldBeEqualToCountOfFirstAndSecondInputs()
        {
            var first = new Dictionary<string, object> { { "prop1", "value1" } };
            var second = new Dictionary<string, object> { { "prop2", "value2" } };

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.Equal(2, result.Count);
        }

        [Fact(DisplayName = "Merge: When both the combination of first input and second input contains unique keys Then the values should be equal to the one inserted from first or second")]
        public void Merge_WhenBothTheCombinationOfFirstInputAndSecondInputContainsUniqueKeys_ThenTheValuesShouldBeEqualToTheOneInsertedFromFirstOrSecond()
        {
            var first = new Dictionary<string, object> { { "prop1", "value1" } };
            var second = new Dictionary<string, object> { { "prop2", "value2" } };

            var result = DictionaryUtilities.Merge(first, second);

            Xunit.Assert.Equal(first["prop1"], result["prop1"]);
            Xunit.Assert.Equal(second["prop2"], result["prop2"]);
        }

        [Fact(DisplayName = "ToJsonQueryString: When string dictionary is not null and empty Then the result should be empty string")]
        public void ToJsonQueryString_WhenStringDictionaryIsNotNullAndEmpty_ThenTheResultShouldBeEmptyString()
        {
            var dict = new Dictionary<string, string>();

            var result = DictionaryUtilities.ToJsonQueryString(dict);

            Xunit.Assert.Equal(string.Empty, result);
        }

        [Fact(DisplayName = "ToJsonQueryString: When string dictionary has values Then the result  should not be empty string or null")]
        public void ToJsonQueryString_WhenStringDictionaryHasValues_ThenTheResultShouldNotBeEmptyStringOrNull()
        {
            var dict = new Dictionary<string, string>
                           {
                               {"key1", "value1"},
                               {"key2", "value2"}
                           };

            var result = DictionaryUtilities.ToJsonQueryString(dict);

            Xunit.Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact(DisplayName = "ToJsonQueryString: When string dictionary has values Then the result should be querstring formatted")]
        public void ToJsonQueryString_WhenStringDictionaryHasValues_ThenTheResultShouldBeQuerstringFormatted()
        {
            var dict = new Dictionary<string, string>
                           {
                               {"key1", "value1"},
                               {"key2", "value2"}
                           };
            var exepected = "key1=value1&key2=value2";

            var result = DictionaryUtilities.ToJsonQueryString(dict);

            // TODO: check key and values that needs to be encoded too
            Xunit.Assert.Equal(exepected, result);
        }

        [Fact(DisplayName = "ToJsonQueryString: When object dictionary is not null and empty Then the result should be empty string")]
        public void ToJsonQueryString_WhenObjectDictionaryIsNotNullAndEmpty_ThenTheResultShouldBeEmptyString()
        {
            var dict = new Dictionary<string, object>();

            var result = DictionaryUtilities.ToJsonQueryString(dict);

            Xunit.Assert.Equal(string.Empty, result);
        }

        [Fact(DisplayName = "ToJsonQueryString: When object dictionary has values Then the result should not be empty string or null")]
        public void ToJsonQueryString_WhenObjectDictionaryHasValues_ThenTheResultShouldNotBeEmptyStringOrNull()
        {
            var dict = new Dictionary<string, object>
                           {
                               {"key1", "value1"},
                               {"key2", "value2"}
                           };

            var result = DictionaryUtilities.ToJsonQueryString(dict);

            Xunit.Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact(DisplayName = "ToJsonQueryString: When object dictionary has values Then the result should be querystring formatted")]
        public void ToJsonQueryString_WhenObjectDictionaryHasValues_ThenTheResultShouldBeQuerystringFormatted()
        {
            var dict = new Dictionary<string, object>
                           {
                               {"key1", "value1"},
                               {"key2", "value2"}
                           };
            var exepected = "key1=value1&key2=value2";

            var result = DictionaryUtilities.ToJsonQueryString(dict);

            // TODO: check key and values that needs to be encoded too
            Xunit.Assert.Equal(exepected, result);
        }

        [Fact(DisplayName = "ToJsonQueryString: When NameValueCollection is not null and empty Then the result should be empty string")]
        public void ToJsonQueryString_WhenNameValueCollectionIsNotNullAndEmpty_ThenTheResultShouldBeEmptyString()
        {
            var nvc = new NameValueCollection();

            string result = DictionaryUtilities.ToJsonQueryString(nvc);

            Xunit.Assert.Equal(string.Empty, result);
        }

        [Fact(DisplayName = "ToJsonQueryString: When NameValueCollection has values Then the result should not be empty string or null")]
        public void ToJsonQueryString_WhenNameValueCollectionHasValues_ThenTheResultShouldNotBeEmptyStringOrNull()
        {
            var nvc = new NameValueCollection();
            nvc.Add("key1", "value1");
            nvc.Add("key2", "value2");

            var result = DictionaryUtilities.ToJsonQueryString(nvc);

            Xunit.Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact(DisplayName = "ToJsonQueryString: When NameValueCollection has values Then the result should be querystring formatted")]
        public void ToJsonQueryString_WhenNameValueCollectionHasValues_ThenTheResultShouldBeQuerystringFormatted()
        {
            var nvc = new NameValueCollection();
            nvc.Add("key1", "value1");
            nvc.Add("key2", "value2");
            var expected = "key1=value1&key2=value2";

            var result = DictionaryUtilities.ToJsonQueryString(nvc);

            // TODO: more tests for encoded values.
            Xunit.Assert.Equal(expected, result);
        }

        [Fact(DisplayName = "ToJsonQueryString: When Object dictionary contains object as list of string Then it should be decoded with square brackets")]
        public void ToJsonQueryString_WhenObjectDictionaryContainsObjectAsListOfString_ThenItShouldBeDecodedWithSquareBrackets()
        {
            var dict = new Dictionary<string, object>
                           {
                               {"key1", "value1"},
                               {"key2", "value2"},
                               {"key3", new List<string> {"list_item1", "list_item2"}}
                           };

            // key1=value1&key2=value2&key3=["list_item1","list_item2"]
            var expected = "key1=value1&key2=value2&key3=%5B%22list_item1%22%2C%22list_item2%22%5D";

            var result = DictionaryUtilities.ToJsonQueryString(dict);

            Xunit.Assert.Equal(expected, result);
        }
    }
}