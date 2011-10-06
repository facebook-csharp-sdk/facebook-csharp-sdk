namespace Facebook.Tests.FacebookUtils.ToJsonQueryString
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenObjectDictionaryContainsObjectAsListOfStringThen
    {
        [Fact]
        public void ItShouldBeDecodedWithSquareBrackets()
        {
            var dict = new Dictionary<string, object>
                           {
                               {"key1", "value1"},
                               {"key2", "value2"},
                               {"key3", new List<string> {"list_item1", "list_item2"}}
                           };

            // key1=value1&key2=value2&key3=["list_item1","list_item2"]
            var expected = "key1=value1&key2=value2&key3=%5B%22list_item1%22%2C%22list_item2%22%5D";

            var result = FacebookUtils.ToJsonQueryString(dict);

            Assert.Equal(expected, result);
        }
    }
}