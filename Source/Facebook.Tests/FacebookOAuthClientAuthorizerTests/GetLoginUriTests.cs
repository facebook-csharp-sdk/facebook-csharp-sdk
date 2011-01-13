
namespace Facebook.Tests.FacebookOAuthClientAuthorizerTests
{
    using System;
    using Xunit;

    public class GetLoginUriTests
    {
        private FacebookOAuthClientAuthorizer oauth;

        public GetLoginUriTests()
        {
            oauth = new FacebookOAuthClientAuthorizer();
        }

        [Fact(DisplayName = "GetLoginUri: If client id is empty Then it should throw InvalidOperationException")]
        public void GetLoginUri_IfClientIdIsEmpty_ThenItShouldThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => this.oauth.GetLoginUri(null));
        }

        [Fact(DisplayName = "GetLoginUri: If client id is empty Then it exception message should be \"client_id required.\"")]
        public void GetLoginUri_IfClientIdIsEmpty_ThenItExceptionMessageShouldBeClientIdRequired()
        {
            Exception exception = null;

            try
            {
                this.oauth.GetLoginUri(null);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.Equal("client_id required.", exception.Message);
        }
    }
}