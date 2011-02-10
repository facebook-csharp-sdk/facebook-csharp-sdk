namespace Facebook.Tests.FacebookOAuthResult.ctor
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenADictionaryWithAccessTokenThen
    {
        [Fact]
        public void IsSuccessShouldBeTrue()
        {
            var parameters = new Dictionary<string, object>
                                 {
                                     { "access_token", "dummy_access_token" }
                                 };

            var result = new FacebookOAuthResult(parameters);

            Assert.True(result.IsSuccess);
        }
    }
}