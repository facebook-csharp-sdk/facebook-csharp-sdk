namespace Facebook.Tests.FacebookUtils.IsUsingRestApi.GivenDomainMapsAsNull
{
    using System;
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class AndRequestUriIsGraphUrlThen
    {
        [Theory]
        [InlineData("https://graph.facebook.com/me")]
        [InlineData("https://graph.beta.facebook.com/me")]
        public void ResultIsFalse(string url)
        {
            var uri = new Uri(url);

            var result = FacebookUtils.IsUsingRestApi(null, uri);

            Assert.False(result);
        }
    }
}