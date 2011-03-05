
namespace Facebook.Tests.FacebookClient.UploadDataCompleted.GivenHttpMethodAsPost.GivenARestApiError.AndStateIsNull
{
    using System;
    using Facebook;
    using Xunit;

    public class ThrowsRestApiErrorThen
    {
        private FacebookClient facebookClient;

        private HttpMethod httpMethod = HttpMethod.Post;

        private string requestUrl = "https://api.facebook.com/restserver.php";
        private string jsonResult = "{\"error_code\":\"101\",\"error_msg\":\"Invalid API key\",\"request_args\":[{\"key\":\"method\",\"value\":\"comments.add\"},{\"key\":\"format\",\"value\":\"json-strings\"}]}";

        private UploadDataCompletedEventArgsWrapper uploadDataCompletedEventArgsWrapper;

        public ThrowsRestApiErrorThen()
        {
            this.facebookClient = new FacebookClient();

            var tempState = new FacebookClient.WebClientTempState
                                {
                                    Method = this.httpMethod,
                                    RequestUri = new Uri(this.requestUrl),
                                };

            this.uploadDataCompletedEventArgsWrapper =
                new UploadDataCompletedEventArgsWrapper(
                    null, false, tempState, System.Text.Encoding.UTF8.GetBytes(jsonResult));
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
        public void UserStateIsNull()
        {
            object userState = null;
            this.facebookClient.PostCompleted += (o, e) => userState = e.UserState;

            ExecuteMethodToTest();

            Assert.Null(userState);
        }

        private void ExecuteMethodToTest()
        {
            this.facebookClient.UploadDataCompleted(this, this.uploadDataCompletedEventArgsWrapper);
        }
    }
}