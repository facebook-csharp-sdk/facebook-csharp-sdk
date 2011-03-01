namespace Facebook.Tests.FacebookUtils.ParseQueryParametersToDictionary
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAPathAsEmptyStringAndEmptyParametersThen
    {
        [Fact]
        public void CountOfParametersEquals0()
        {
            string path = string.Empty;
            var parameters = new Dictionary<string, object>();

            FacebookUtils.ParseQueryParametersToDictionary(path, parameters);

            Assert.Equal(0, parameters.Count);
        }
    }
}