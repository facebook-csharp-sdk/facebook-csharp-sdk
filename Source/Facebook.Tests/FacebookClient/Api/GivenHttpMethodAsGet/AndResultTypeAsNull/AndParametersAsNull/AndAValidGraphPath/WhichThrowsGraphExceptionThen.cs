namespace Facebook.Tests.FacebookClient.Api.GivenHttpMethodAsGet.AndResultTypeAsNull.AndParametersAsNull.AndAValidGraphPath
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhichThrowsGraphExceptionThen
    {
        private FacebookClient facebookClient;

        private string path = "/me";
        private IDictionary<string, object> parameters = null;
        private HttpMethod httpMethod = HttpMethod.Get;
        private Type resultType = null;

        private string requestUrl = "https://graph.facebook.com/me";
        private string jsonResult = "{\"error\":{\"type\":\"OAuthException\",\"message\":\"An active access token must be used to query information about the current user.\"}}";

        public WhichThrowsGraphExceptionThen()
        {
            this.facebookClient = new FacebookClient
                                      {
                                          WebClient = WebClientFakes.DownloadDataThrowsGraphException(requestUrl, jsonResult)
                                      };
        }

        [Fact]
        public void ThrowsFacebookApiException()
        {
            bool threwFacebookApiException = false;
            try
            {
                ExecuteMethodToTest();
            }
            catch (FacebookApiException)
            {
                threwFacebookApiException = true;
            }

            Assert.True(threwFacebookApiException);
        }

        [Fact]
        public void GetCompletedEventShouldNotBeFired()
        {
            bool executed = false;
            this.facebookClient.GetCompleted += (o, e) => executed = true;

            try
            {
                this.ExecuteMethodToTest();
            }
            catch
            {
            }

            Assert.False(executed);
        }

        [Fact]
        public void DeleteCompletedEventShouldNotBeFired()
        {
            bool executed = false;
            this.facebookClient.DeleteCompleted += (o, e) => executed = true;

            try
            {
                this.ExecuteMethodToTest();
            }
            catch
            {
            }

            Assert.False(executed);
        }

        [Fact]
        public void PostCompletedEventShouldNotBeFired()
        {
            bool executed = false;
            this.facebookClient.PostCompleted += (o, e) => executed = true;

            try
            {
                this.ExecuteMethodToTest();
            }
            catch
            {
            }

            Assert.False(executed);
        }

        private object ExecuteMethodToTest()
        {
            return this.facebookClient.Api(this.path, this.parameters, this.httpMethod, this.resultType);
        }
    }
}