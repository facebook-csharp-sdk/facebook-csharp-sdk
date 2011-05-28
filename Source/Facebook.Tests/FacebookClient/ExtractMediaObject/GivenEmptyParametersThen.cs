using System.Collections.Generic;
using Xunit;

namespace Facebook.Tests.FacebookClient.ExtractMediaObject
{
    using Facebook;

    public class GivenEmptyParametersThen
    {
        private IDictionary<string, object> _parameters;

        public GivenEmptyParametersThen()
        {
            _parameters = new Dictionary<string, object>();
        }

        [Fact]
        public void ReturnValueIsNotNull()
        {
            var result = FacebookClient.ExtractMediaObjects(_parameters);

            Assert.NotNull(result);
        }

        [Fact]
        public void CountIs0()
        {
            var result = FacebookClient.ExtractMediaObjects(_parameters);

            Assert.Equal(0, result.Count);
        }
    }
}