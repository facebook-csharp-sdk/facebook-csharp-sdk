namespace Facebook.Tests.FacebookUtils.IsUsingRestApi.GivenDomainMapsAsNull
{
    using System;
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class AndRequestUriIsApiUrlThen
    {
        [Theory]
        [InlineData("https://api.facebook.com")]
        public void ResultIsTrue(string url)
        {
            var uri = new Uri(url);

            var result = FacebookUtils.IsUsingRestApi(null, uri);

            Assert.True(result);
        }
    }
}