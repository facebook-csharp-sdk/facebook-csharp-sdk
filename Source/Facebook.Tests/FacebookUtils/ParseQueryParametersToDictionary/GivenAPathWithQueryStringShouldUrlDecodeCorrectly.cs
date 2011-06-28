namespace Facebook.Tests.FacebookUtils.ParseQueryParametersToDictionary
{
    using System.Collections.Generic;
    using Xunit;

    public class GivenAPathWithQueryStringShouldUrlDecodeCorrectly
    {
        [Fact]
        public void UrlDecodesCorrectly()
        {
            string pathWithQuerystring = "/me?access_token=124973702%7cc6d91d1492d6d1a.1-6306%7cpLl4mEfII18sA";
            var parameters = new Dictionary<string, object>();

            var path = Facebook.FacebookUtils.ParseQueryParametersToDictionary(pathWithQuerystring, parameters);

            Assert.Equal(path, "me");
            Assert.NotNull(parameters);
            Assert.Equal(1, parameters.Count);
            Assert.Equal("124973702|c6d91d1492d6d1a.1-6306|pLl4mEfII18sA", parameters["access_token"]);
        }
    }
}