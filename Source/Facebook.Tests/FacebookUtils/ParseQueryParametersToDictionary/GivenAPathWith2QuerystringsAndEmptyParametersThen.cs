namespace Facebook.Tests.FacebookUtils.ParseQueryParametersToDictionary
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAPathWith2QuerystringsAndEmptyParametersThen
    {
        [Fact]
        public void ReturnPathEqualsThePathWithoutQuerystring()
        {
            string originalPath = "/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            var path = FacebookUtils.ParseQueryParametersToDictionary(originalPath, parameters);

            Assert.Equal(path, "me/likes");
        }

        [Fact]
        public void ParameterValuesEqualToTheQuerystrings()
        {
            string path = "/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            FacebookUtils.ParseQueryParametersToDictionary(path, parameters);

            Assert.Equal("3", parameters["limit"]);
            Assert.Equal("2", parameters["offset"]);
        }

        [Fact]
        public void CountOfParametersEquals2()
        {
            string path = "/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            FacebookUtils.ParseQueryParametersToDictionary(path, parameters);

            Assert.Equal(2, parameters.Count);
        }
    }
}