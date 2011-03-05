
namespace Facebook.Tests.FacebookClient.Api.GiveHttpMethodAsPost.AndTheResultTypeIsNull.AndParametersAsNotNullWhichDoesNotContainFacebookMediaObject.AndAValidGraphPath
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Facebook;
    using Xunit;

    public class WhichThrowsGraphExceptionThen
    {
        private FacebookClient facebookClient;

        private string path = "/me";
        private IDictionary<string, object> parameters = new Dictionary<string, object> { { "message", "dummy_message" } };
        private HttpMethod httpMethod = HttpMethod.Post;
        private Type resultType = null;

        private string requestUrl = "https://graph.facebook.com/me";
        private string jsonResult = "{\"error\":{\"type\":\"OAuthException\",\"message\":\"An active access token must be used to query information about the current user.\"}}";

        public WhichThrowsGraphExceptionThen()
        {
            this.facebookClient = new FacebookClient
                                      {
                                          WebClient = WebClientFakes.DownloadAndUploadDataThrowsGraphException(requestUrl, jsonResult)
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
        public void ContentTypeHeaderIsSet()
        {
            bool hasContentTypeHeader = false;
            try
            {
                ExecuteMethodToTest();
            }
            catch (FacebookApiException)
            {
                hasContentTypeHeader = this.facebookClient.WebClient.Headers.AllKeys.Contains("Content-Type", StringComparer.OrdinalIgnoreCase);
            }

            Assert.True(hasContentTypeHeader);
        }

        [Fact]
        public void ContentTypeIsSetToApplicationXWwwFormUrlEncoded()
        {
            string contentType = null;
            try
            {
                ExecuteMethodToTest();
            }
            catch (FacebookApiException)
            {
                contentType = this.facebookClient.WebClient.Headers["Content-Type"];
            }

            Assert.Equal("application/x-www-form-urlencoded", contentType);
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