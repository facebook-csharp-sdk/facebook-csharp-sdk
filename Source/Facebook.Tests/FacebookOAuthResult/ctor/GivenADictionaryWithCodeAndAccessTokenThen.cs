namespace Facebook.Tests.FacebookOAuthResult.ctor
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenADictionaryWithCodeAndAccessTokenThen
    {
        [Fact]
        public void CodeShouldBeTheOneSpecifiedInDictionary()
        {
            var parameters = new Dictionary<string, object>
                                {
                                    { "code", "code" },
                                    { "access_token", "accesstoken" }
                                };

            var result = new FacebookOAuthResult(parameters);

            Assert.Equal("code", result.Code);
        }
    }
}