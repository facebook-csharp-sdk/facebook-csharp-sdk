using System.Collections.Generic;
using Xunit;

namespace Facebook.Tests.FacebookClient.ExtractMediaObject
{
    using Facebook;

    public class GivenNullParametersThen
    {
        private IDictionary<string, object> _parameters;

        public GivenNullParametersThen()
        {
            _parameters = null;
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