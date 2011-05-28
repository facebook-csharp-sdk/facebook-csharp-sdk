using System.Collections.Generic;
using Xunit;

namespace Facebook.Tests.FacebookClient.ExtractMediaObject
{
    public class GivenMoreThanOneMediaObjectsOnlyThen
    {
        private IDictionary<string, object> _parameters;

        public GivenMoreThanOneMediaObjectsOnlyThen()
        {
            _parameters = new Dictionary<string, object>();
            _parameters["file1"] = new FacebookMediaObject();
            _parameters["file2"] = new FacebookMediaObject();
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

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void ParametersCountIs1()
        {
            var result = Facebook.FacebookClient.ExtractMediaObjects(_parameters);

            Assert.Equal(0, _parameters.Count);
        }
    }
}