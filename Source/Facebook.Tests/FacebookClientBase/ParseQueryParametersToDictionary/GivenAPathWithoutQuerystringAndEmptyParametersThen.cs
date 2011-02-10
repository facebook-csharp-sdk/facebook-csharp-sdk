namespace Facebook.Tests.FacebookClientBase.ParseQueryParametersToDictionary
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAPathWithoutQuerystringAndEmptyParametersThen
    {
        [Fact]
        public void CountOfParametersEquals0()
        {
            var path = "/me/likes";
            var parameters = new Dictionary<string, object>();

            FacebookClientBase.ParseQueryParametersToDictionary(path, parameters);

            Assert.Equal(0, parameters.Count);
        }
    }
}