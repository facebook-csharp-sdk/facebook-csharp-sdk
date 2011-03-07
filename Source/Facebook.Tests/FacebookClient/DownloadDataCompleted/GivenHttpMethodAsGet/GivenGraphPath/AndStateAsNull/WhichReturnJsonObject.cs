namespace Facebook.Tests.FacebookClient.DownloadDataCompleted.GivenHttpMethodAsGet.GivenGraphPath.AndStateAsNull
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Facebook;
    using Xunit;

    public class WhichReturnJsonObject
    {
        private FacebookClient facebookClient;

        private HttpMethod httpMethod = HttpMethod.Get;

        private string requestUrl = "https://graph.facebook.com/4";
        private string jsonResult = "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}";

        private DownloadDataCompletedEventArgsWrapper downloadDataCompletedEventArgs;

        public WhichReturnJsonObject()
        {
            this.facebookClient = new FacebookClient();

            var tempState = new WebClientStateContainer
                                {
                                    Method = this.httpMethod,
                                    RequestUri = new Uri(this.requestUrl),
                                };

            this.downloadDataCompletedEventArgs =
                new DownloadDataCompletedEventArgsWrapper(null, false, tempState, Encoding.UTF8.GetBytes(jsonResult));
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
        public void ErrorShouldBeNull()
        {
            Exception exception = null;
            this.facebookClient.GetCompleted += (o, e) => exception = e.Error;

            ExecuteMethodToTest();

            Assert.Null(exception);
        }

        [Fact]
        public void ResultShouldNotBeNull()
        {
            object result = null;
            this.facebookClient.GetCompleted += (o, e) => result = e.GetResultData();

            ExecuteMethodToTest();

            Assert.NotNull(result);
        }

        [Fact]
        public void ResultIsAssignableFromIDictionaryStringObject()
        {
            object result = null;
            this.facebookClient.GetCompleted += (o, e) => result = e.GetResultData();

            ExecuteMethodToTest();

            Assert.IsAssignableFrom<IDictionary<string, object>>(result);
        }

        [Fact]
        public void ResultIsOfTypeJsonObject()
        {
            object result = null;
            this.facebookClient.GetCompleted += (o, e) => result = e.GetResultData();

            ExecuteMethodToTest();

            Assert.IsType<JsonObject>(result);
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