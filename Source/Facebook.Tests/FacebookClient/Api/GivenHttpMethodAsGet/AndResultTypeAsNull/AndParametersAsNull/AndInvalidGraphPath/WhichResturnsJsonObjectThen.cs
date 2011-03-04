namespace Facebook.Tests.FacebookClient.Api.GivenHttpMethodAsGet.AndResultTypeAsNull.AndParametersAsNull.AndInvalidGraphPath
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhichResturnsJsonObjectThen
    {
        private FacebookClient facebookClient;

        private string path = "/path_does_not_exists";
        private IDictionary<string, object> parameters = null;
        private HttpMethod httpMethod = HttpMethod.Get;
        private Type resultType = null;

        // NOTE: seems like there is a bug in the facebook graph api. Facebook seems to be returning 200OK instead of a different exception for this particular case.
        private string requestUrl = "https://graph.facebook.com/path_does_not_exists";
        private string jsonResult = "{\"error\":{\"type\":\"OAuthException\",\"message\":\"(#803) Some of the aliases you requested do not exist: path_does_not_exists\"}}";

        public WhichResturnsJsonObjectThen()
        {
            this.facebookClient = new FacebookClient
            {
                WebClient = WebClientFakes.DownloadData(requestUrl, jsonResult)
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