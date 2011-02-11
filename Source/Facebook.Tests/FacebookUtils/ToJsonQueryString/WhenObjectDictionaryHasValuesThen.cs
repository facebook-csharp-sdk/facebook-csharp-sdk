namespace Facebook.Tests.FacebookUtils.ToJsonQueryString
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenObjectDictionaryHasValuesThen
    {
        [Fact]
        public void ResultShouldNotBeEmptyStringOrNull()
        {
            var dict = new Dictionary<string, object>
                           {
                               {"key1", "value1"},
                               {"key2", "value2"}
                           };

            var result = FacebookUtils.ToJsonQueryString(dict);

            Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact]
        public void ResultShouldBeQuerystringFormatted()
        {
            var dict = new Dictionary<string, object>
                           {
                               {"key1", "value1"},
                               {"key2", "value2"}
                           };
            var exepected = "key1=value1&key2=value2";

            var result = FacebookUtils.ToJsonQueryString(dict);

            // TODO: check key and values that needs to be encoded too
            Assert.Equal(exepected, result);
        }
    }
}