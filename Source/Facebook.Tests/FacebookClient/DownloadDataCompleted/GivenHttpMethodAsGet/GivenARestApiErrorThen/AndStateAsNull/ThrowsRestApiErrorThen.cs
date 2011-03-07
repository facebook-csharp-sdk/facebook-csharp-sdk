namespace Facebook.Tests.FacebookClient.DownloadDataCompleted.GivenHttpMethodAsGet.GivenARestApiErrorThen.AndStateAsNull
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class ThrowsRestApiErrorThen
    {
        private FacebookClient facebookClient;

        private string path = null;
        private IDictionary<string, object> parameters = new Dictionary<string, object>
                                                             {
                                                                 { "query", "invalid query" },
                                                                 { "method", "fql.query" }                                                                 
                                                             };
        private HttpMethod httpMethod = HttpMethod.Get;

        private string requestUrl = "https://api-read.facebook.com/restserver.php?query=invalid+query&method=fql.query&format=json-strings";
        private string jsonResult = "{\"error_code\":\"601\",\"error_msg\":\"Parser error: unexpected 'invalid' at position 0.\",\"request_args\":[{\"key\":\"query\",\"value\":\"invalid query\"},{\"key\":\"method\",\"value\":\"fql.query\"},{\"key\":\"format\",\"value\":\"json-strings\"}]}";

        private DownloadDataCompletedEventArgsWrapper downloadDataCompletedEventArgs;

        public ThrowsRestApiErrorThen()
        {
            this.facebookClient = new FacebookClient();

            var tempState = new WebClientStateContainer
                                {
                                    Method = this.httpMethod,
                                    RequestUri = new Uri(this.requestUrl),
                                };

            this.downloadDataCompletedEventArgs =
                new DownloadDataCompletedEventArgsWrapper(
                    null, false, tempState, System.Text.Encoding.UTF8.GetBytes(jsonResult));
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
        public void ErrorShouldBeAssignableFromFacebookApiException()
        {
            Exception exception = null;
            this.facebookClient.GetCompleted += (o, e) => exception = e.Error;

            ExecuteMethodToTest();

            Assert.IsAssignableFrom<FacebookApiException>(exception);
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