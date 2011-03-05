namespace Facebook.Tests.FacebookClient.DownloadDataCompleted.GivenHttpMethodAsGet.GivenGraphException.AndStateAsNull
{
    using System;
    using Facebook;
    using Xunit;

    public class WhichThrowsGraphExceptionThen
    {
        private FacebookClient facebookClient;

        private HttpMethod httpMethod = HttpMethod.Get;

        private string requestUrl = "https://graph.facebook.com/me";
        private string jsonResult = "{\"error\":{\"type\":\"OAuthException\",\"message\":\"An active access token must be used to query information about the current user.\"}}";

        private DownloadDataCompletedEventArgsWrapper downloadDataCompletedEventArgs;

        public WhichThrowsGraphExceptionThen()
        {
            this.facebookClient = new FacebookClient();

            var tempState = new FacebookClient.WebClientTempState
                                {
                                    Method = this.httpMethod,
                                    RequestUri = new Uri(this.requestUrl),
                                };

            this.downloadDataCompletedEventArgs =
                new DownloadDataCompletedEventArgsWrapper(
                    WebClientFakes.GetFakeWebException(jsonResult), false, tempState, null);
        }

        [Fact]
        public void GetCompletedEventShouldBeFired()
        {
            var executed = false;
            this.facebookClient.GetCompleted += (o, e) => executed = true;

            ExecuteMethodToTest();

            Assert.True(executed);
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
        public void PostCompletedEventShouldNotBeFired()
        {
            var executed = false;
            this.facebookClient.PostCompleted += (o, e) => executed = true;

            ExecuteMethodToTest();

            Assert.False(executed);
        }

        [Fact]
        public void FacebookApiEventArgsShouldNotBeNull()
        {
            bool? isNull = null;
            this.facebookClient.GetCompleted += (o, e) => isNull = e == null;

            ExecuteMethodToTest();

            Assert.False(isNull.Value);
        }

        [Fact]
        public void ErrorShouldNotBeNull()
        {
            Exception exception = null;
            this.facebookClient.GetCompleted += (o, e) => exception = e.Error;

            ExecuteMethodToTest();

            Assert.NotNull(exception);
        }

        [Fact]
        public void ResultShouldBeNull()
        {
            object result = null;
            this.facebookClient.GetCompleted += (o, e) => result = e.GetResultData();

            ExecuteMethodToTest();

            Assert.Null(result);
        }

        [Fact]
        public void UserStateIsNull()
        {
            object userState = null;
            this.facebookClient.GetCompleted += (o, e) => userState = e.UserState;

            ExecuteMethodToTest();

            Assert.Null(userState);
        }

        private void ExecuteMethodToTest()
        {
            this.facebookClient.DownloadDataCompleted(this, this.downloadDataCompletedEventArgs);
        }
    }
}