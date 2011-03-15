namespace Facebook.Web.Tests.FacebookSession.ctor_dictionary
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenEmptyDictionaryThen
    {
        private FacebookSession session;

        public GivenEmptyDictionaryThen()
        {
            session = new FacebookSession(new Dictionary<string, object>());
        }

        [Fact]
        public void DataIsNotNull()
        {
            Assert.NotNull(session.Data);
        }

        [Fact]
        public void DataIsAssignableFromIDictionaryStringObject()
        {
            Assert.IsAssignableFrom<IDictionary<string, object>>(session.Data);
        }

        [Fact]
        public void DataIsTypeOfJsonObject()
        {
            Assert.IsType<JsonObject>(session.Data);
        }

        [Fact]
        public void DataCountIs0()
        {
            var data = (IDictionary<string, object>)session.Data;

            Assert.Equal(0, data.Count);
        }
    }
}