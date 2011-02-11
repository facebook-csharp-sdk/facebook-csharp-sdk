namespace Facebook.Tests.FacebookUtils.ToJsonQueryString
{
    using System.Collections.Specialized;
    using Facebook;
    using Xunit;

    public class WhenNameValueCollectionHasValuesThen
    {
        [Fact]
        public void ResultShouldNotBeEmptyStringOrNull()
        {
            var nvc = new NameValueCollection();
            nvc.Add("key1", "value1");
            nvc.Add("key2", "value2");

            var result = FacebookUtils.ToJsonQueryString(nvc);

            Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact]
        public void ResultShouldBeQuerystringFormatted()
        {
            var nvc = new NameValueCollection();
            nvc.Add("key1", "value1");
            nvc.Add("key2", "value2");
            var expected = "key1=value1&key2=value2";

            var result = FacebookUtils.ToJsonQueryString(nvc);

            // TODO: more tests for encoded values.
            Assert.Equal(expected, result);
        }
    }
}