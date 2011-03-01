namespace Facebook.Tests.FacebookUtils.ParseQueryParametersToDictionary
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAPathStartingWithForwardSlashWithQuerystringAndEmptyParametersThen
    {
        [Fact]
        public void ReturnPathEqualsThePathWithoutForwardSlashAndQuerystring()
        {
            string originalPathWithQueryString = "/me/likes?limit=3&offset=2";
            string pathWithoutForwardSlashAndQueryString = "me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookUtils.ParseQueryParametersToDictionary(originalPathWithQueryString, parameters);

            Assert.Equal(pathWithoutForwardSlashAndQueryString, path);
        }

        [Fact]
        public void ReturnPathDoesNotStartWithForwardSlash()
        {
            string originalPathWithQueryString = "/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            var path = FacebookUtils.ParseQueryParametersToDictionary(originalPathWithQueryString, parameters);

            Assert.NotEqual('/', path[0]);
        }
    }
}