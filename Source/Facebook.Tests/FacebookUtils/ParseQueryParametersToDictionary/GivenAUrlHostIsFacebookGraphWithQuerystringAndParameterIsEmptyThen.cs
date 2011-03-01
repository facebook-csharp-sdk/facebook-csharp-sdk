namespace Facebook.Tests.FacebookUtils.ParseQueryParametersToDictionary
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAUrlHostIsFacebookGraphWithQuerystringAndParameterIsEmptyThen
    {
        [Fact]
        public void TheParameterValuesAreEqualToTheQuerystrings()
        {
            string urlWithQueryString = "http://graph.facebook.com/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            FacebookUtils.ParseQueryParametersToDictionary(urlWithQueryString, parameters);

            Assert.Equal("3", parameters["limit"]);
            Assert.Equal("2", parameters["offset"]);
        }

        [Fact]
        public void TheCountOfParameterIsEqualToTheCountOfQuerystring()
        {
            string urlWithQueryString = "http://graph.facebook.com/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            FacebookUtils.ParseQueryParametersToDictionary(urlWithQueryString, parameters);

            Assert.Equal(2, parameters.Count);
        }

        [Fact]
        public void TheReturnPathEqualsPathWithoutUriHostAndDoesNotStartWithForwardSlash()
        {
            string urlWithQueryString = "http://graph.facebook.com/me/likes?limit=3&offset=2";
            string originalPathWithoutForwardSlashAndWithoutQueryString = "me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookUtils.ParseQueryParametersToDictionary(urlWithQueryString, parameters);

            Assert.Equal(originalPathWithoutForwardSlashAndWithoutQueryString, path);
        }

        [Fact]
        public void TheReturnPathDoesNotStartWithForwardSlash()
        {
            string urlWithQueryString = "http://graph.facebook.com/me/likes?limit=3&offset=2";

            var parameters = new Dictionary<string, object>();

            var path = FacebookUtils.ParseQueryParametersToDictionary(urlWithQueryString, parameters);

            Assert.NotEqual('/', path[0]);
        }

    }
}