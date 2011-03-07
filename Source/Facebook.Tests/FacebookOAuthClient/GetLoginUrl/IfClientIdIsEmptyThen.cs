namespace Facebook.Tests.FacebookOAuthClient.GetLoginUrl
{
    using System;
    using Facebook;
    using Xunit;

    public class IfClientIdIsEmptyThen
    {
        [Fact]
        public void ItShouldThrowInvalidOperationException()
        {
            var oauth = new FacebookOAuthClient();
            Assert.Throws<ArgumentException>(() => oauth.GetLoginUrl(null));
        }

        [Fact]
        public void ItExceptionMessageShouldBeClientIdRequired()
        {
            var oauth = new FacebookOAuthClient();
            Exception exception = null;

            try
            {
                oauth.GetLoginUrl(null);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.Equal("client_id required.", exception.Message);
        }
    }
}