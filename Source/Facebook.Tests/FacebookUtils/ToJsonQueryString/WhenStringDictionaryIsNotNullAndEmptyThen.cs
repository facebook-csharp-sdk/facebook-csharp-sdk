namespace Facebook.Tests.FacebookUtils.ToJsonQueryString
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenStringDictionaryIsNotNullAndEmptyThen
    {
        [Fact]
        public void ResultShouldBeEmptyString()
        {
            var dict = new Dictionary<string, object>();

            var result = FacebookUtils.ToJsonQueryString(dict);

            Assert.Equal(string.Empty, result);
        }
    }
}