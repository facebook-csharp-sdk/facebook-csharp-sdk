
namespace Facebook.Tests.FacebookClient.UploadDataCompleted.GivenHttpMethodAsPost.GivenAGraphPath.AndStateIsNull
{
    using System;
    using Facebook;
    using Xunit;

    public class WhichThrowsGraphExceptionThen
    {
        private FacebookClient facebookClient;

        private HttpMethod httpMethod = HttpMethod.Post;

        private string requestUrl = "https://graph.facebook.com/me";
        private string jsonResult = "{\"error\":{\"type\":\"OAuthException\",\"message\":\"An active access token must be used to query information about the current user.\"}}";

        private UploadDataCompletedEventArgsWrapper uploadDataCompletedEventArgs;

        public WhichThrowsGraphExceptionThen()
        {
            this.facebookClient = new FacebookClient();

            var tempState = new WebClientStateContainer
            {
                Method = this.httpMethod,
                RequestUri = new Uri(this.requestUrl),
            };

            this.uploadDataCompletedEventArgs =
                new UploadDataCompletedEventArgsWrapper(
                    WebClientFakes.GetFakeWebException(jsonResult), false, tempState, null);
        }

        [Fact]
        public void GetCompletedEventShouldNotBeFired()
        {
            var executed = false;
            this.facebookClient.GetCompleted += (o, e) => executed = true;

            ExecuteMethodToTest();

            Assert.False(executed);
        }

        [Fact]
        public void DeleteCompletedEventShouldNotBeFired()
        {
            var executed = false;
            this.facebookClient.DeleteCompleted += (o, e) => executed = true;

            ExecuteMethodToTest();

            Assert.False(executed);
        }

        [Fact]
        public void PostCompletedEventShouldBeFired()
        {
            var executed = false;
            this.facebookClient.PostCompleted += (o, e) => executed = true;

            ExecuteMethodToTest();

            Assert.True(executed);
        }

        [Fact]
        public void FacebookApiEventArgsShouldNotBeNull()
        {
            bool? isNull = null;
            this.facebookClient.PostCompleted += (o, e) => isNull = e == null;

            ExecuteMethodToTest();

            Assert.False(isNull.Value);
        }

        [Fact]
        public void ErrorShouldNotBeNull()
        {
            Exception exception = null;
            this.facebookClient.PostCompleted += (o, e) => exception = e.Error;

            ExecuteMethodToTest();

            Assert.NotNull(exception);
        }

        [Fact]
        public void ErrorShouldBeAssignableFromFacebookApiException()
        {
            Exception exception = null;
            this.facebookClient.PostCompleted += (o, e) => exception = e.Error;

            ExecuteMethodToTest();

            Assert.IsAssignableFrom<FacebookApiException>(exception);
        }

        [Fact]
        public void ResultShouldBeNull()
        {
            object result = null;
            this.facebookClient.PostCompleted += (o, e) => result = e.GetResultData();

            ExecuteMethodToTest();

            Assert.Null(result);
        }

        [Fact]
        public void UserStateShouldBeNull()
        {
            object userState = null;
            this.facebookClient.PostCompleted += (o, e) => userState = e.UserState;

            ExecuteMethodToTest();

            Assert.Null(userState);
        }

        private void ExecuteMethodToTest()
        {
            this.facebookClient.UploadDataCompleted(this, this.uploadDataCompletedEventArgs);
        }
    }
}