using System.Collections.Generic;
using Xunit;

namespace Facebook.Tests.FacebookClient.ExtractMediaObject
{
    using Facebook;

    public class GiveOneMediaObjectOnlyThen
    {
        private IDictionary<string, object> _parameters;

        public GiveOneMediaObjectOnlyThen()
        {
            _parameters = new Dictionary<string, object>();
            _parameters["source"] = new FacebookMediaObject();
        }

        [Fact]
        public void ResultIsNotNull()
        {
            var result = FacebookClient.ExtractMediaObjects(_parameters);

            Assert.NotNull(result);
        }

        [Fact]
        public void ResultCountIs1()
        {
            var result = FacebookClient.ExtractMediaObjects(_parameters);

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void ParametersCountIs0()
        {
            var result = FacebookClient.ExtractMediaObjects(_parameters);

            Assert.Equal(0, _parameters.Count);
        }
    }
}