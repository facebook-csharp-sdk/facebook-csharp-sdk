namespace Facebook.Tests.FacebookOAuthClient.GetLoginUrl
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class IfRedirectUriIsEmptyThen
    {
        [Fact]
        public void ItShouldThrowInvalidOperationException()
        {
            var oauth = new FacebookOAuthClient();

            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = "dummy client id";
            parameters["redirect_uri"] = null;

            Assert.Throws<ArgumentException>(() => oauth.GetLoginUrl(parameters));
        }

        [Fact]
        public void GetLoginUri_IfRedirectUriIsEmpty_ThenItExceptionMessageShouldBeRedirectUriRequired()
        {
            var oauth = new FacebookOAuthClient();

            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = "dummy client id";
            parameters["redirect_uri"] = null;

            Exception exception = null;

            try
            {
                oauth.GetLoginUrl(parameters);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.Equal("redirect_uri required.", exception.Message);
        }
    }
}