namespace Facebook.Tests.FacebookUtils.ParseQueryParametersToDictionary
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAUrlHostIsFacebookGraphWithoutQuerystringAndParameterIsEmptyThen
    {
        [Fact]
        public void CountOfParameterIs0()
        {
            string url = "http://graph.facebook.com/me/likes";
            var parameters = new Dictionary<string, object>();

            FacebookUtils.ParseQueryParametersToDictionary(url, parameters);

            Assert.Equal(0, parameters.Count);
        }

        [Fact]
        public void ReturnPathEqualsPathWithoutUriHostAndDoesntStartWithForwardSlash()
        {
            string url = "http://graph.facebook.com/me/likes";
            string originalPathWithoutForwardSlash = "me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookUtils.ParseQueryParametersToDictionary(url, parameters);

            Assert.Equal(originalPathWithoutForwardSlash, path);
        }

        [Fact]
        public void ReturnPathDoesNotStartWithForwardSlash()
        {
            string url = "http://graph.facebook.com/me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookUtils.ParseQueryParametersToDictionary(url, parameters);

            Assert.NotEqual('/', path[0]);
        }
    }
}