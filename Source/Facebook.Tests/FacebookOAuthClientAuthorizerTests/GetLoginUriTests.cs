
namespace Facebook.Tests.FacebookOAuthClientAuthorizerTests
{
    using System;
    using System.Collections.Generic;
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
            Assert.Throws<InvalidOperationException>(() => this.oauth.GetLoginUrl(null));
        }

        [Fact(DisplayName = "GetLoginUri: If client id is empty Then it exception message should be \"client_id required.\"")]
        public void GetLoginUri_IfClientIdIsEmpty_ThenItExceptionMessageShouldBeClientIdRequired()
        {
            Exception exception = null;

            try
            {
                this.oauth.GetLoginUrl(null);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.Equal("client_id required.", exception.Message);
        }

        [Fact(DisplayName = "GetLoginUri: If redirect_uri is empty Then it should throw InvalidOperationException")]
        public void GetLoginUri_IfRedirectUriIsEmpty_ThenItShouldThrowInvalidOperationException()
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = "dummy client id";
            parameters["redirect_uri"] = null;

            Assert.Throws<InvalidOperationException>(() => this.oauth.GetLoginUrl(parameters));
        }

        [Fact(DisplayName = "GetLoginUri: If redirect_uri is empty Then it exception message should be \"redirect_uri required.\"")]
        public void GetLoginUri_IfRedirectUriIsEmpty_ThenItExceptionMessageShouldBeRedirectUriRequired()
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = "dummy client id";
            parameters["redirect_uri"] = null;

            Exception exception = null;

            try
            {
                this.oauth.GetLoginUrl(parameters);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.Equal("redirect_uri required.", exception.Message);
        }
    }
}