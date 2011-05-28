using System.Collections.Generic;
using Xunit;

namespace Facebook.Tests.FacebookClient.ExtractMediaObject
{
    public class GivenOneMediaObjectThen
    {
        private IDictionary<string, object> _parameters;

        public GivenOneMediaObjectThen()
        {
            _parameters = new Dictionary<string, object>();
            _parameters["source"] = new FacebookMediaObject();
            _parameters["message"] = "hello world";
        }

        [Fact]
        public void ResultIsNotNull()
        {
            var result = Facebook.FacebookClient.ExtractMediaObjects(_parameters);

            Assert.NotNull(result);
        }

        [Fact]
        public void ResultCountIs1()
        {
            var result = Facebook.FacebookClient.ExtractMediaObjects(_parameters);

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void ParametersCountIs1()
        {
            var result = Facebook.FacebookClient.ExtractMediaObjects(_parameters);

            Assert.Equal(1, _parameters.Count);
        }
    }
}