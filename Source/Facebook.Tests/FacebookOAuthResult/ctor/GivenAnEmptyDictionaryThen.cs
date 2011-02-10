namespace Facebook.Tests.FacebookOAuthResult.ctor
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAnEmptyDictionaryThen
    {
        [Fact]
        public void AccessTokenShouldBeNull()
        {
            var result = new FacebookOAuthResult(new Dictionary<string, object>());

            Assert.Null(result.AccessToken);
        }

        [Fact]
        public void ErrorReasonShouldBeNull()
        {
            var result = new FacebookOAuthResult(new Dictionary<string, object>());

            Assert.Null(result.ErrorReason);
        }

        [Fact]
        public void ErrorDescriptionShouldBeNull()
        {
            var result = new FacebookOAuthResult(new Dictionary<string, object>());

            Assert.Null(result.ErrorDescription);
        }

        [Fact]
        public void CodeShouldBeNull()
        {
            var result = new FacebookOAuthResult(new Dictionary<string, object>());

            Assert.Null(result.Code);
        }
    }
}