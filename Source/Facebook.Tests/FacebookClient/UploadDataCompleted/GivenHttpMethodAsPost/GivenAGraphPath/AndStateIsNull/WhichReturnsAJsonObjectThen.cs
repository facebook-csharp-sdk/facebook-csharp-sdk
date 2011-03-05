namespace Facebook.Tests.FacebookClient.UploadDataCompleted.GivenHttpMethodAsPost.GivenAGraphPath.AndStateIsNull
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Facebook;
    using Xunit;

    public class WhichReturnsAJsonObjectThen
    {
        private FacebookClient facebookClient;

        private HttpMethod httpMethod = HttpMethod.Post;

        private string requestUrl = "https://graph.facebook.com/me/feed?access_token=dummyAccessToken";
        private string jsonResult = "{\"id\":\"100001327642026_159109420809978\"}";

        private UploadDataCompletedEventArgsWrapper uploadDataCompletedEventArgsWrapper;

        public WhichReturnsAJsonObjectThen()
        {
            this.facebookClient = new FacebookClient();

            var tempState = new FacebookClient.WebClientTempState
                                {
                                    Method = this.httpMethod,
                                    RequestUri = new Uri(this.requestUrl),
                                };

            this.uploadDataCompletedEventArgsWrapper =
                new UploadDataCompletedEventArgsWrapper(null, false, tempState, Encoding.UTF8.GetBytes(jsonResult));
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
        public void ErrorShouldBeNull()
        {
            Exception exception = null;
            this.facebookClient.PostCompleted += (o, e) => exception = e.Error;

            ExecuteMethodToTest();

            Assert.Null(exception);
        }

        [Fact]
        public void ResultShouldNotBeNull()
        {
            object result = null;
            this.facebookClient.PostCompleted += (o, e) => result = e.GetResultData();

            ExecuteMethodToTest();

            Assert.NotNull(result);
        }

        [Fact]
        public void ResultIsAssignableFromIDictionaryStringObject()
        {
            object result = null;
            this.facebookClient.PostCompleted += (o, e) => result = e.GetResultData();

            ExecuteMethodToTest();

            Assert.IsAssignableFrom<IDictionary<string, object>>(result);
        }

        [Fact]
        public void ResultIsOfTypeJsonObject()
        {
            object result = null;
            this.facebookClient.PostCompleted += (o, e) => result = e.GetResultData();

            ExecuteMethodToTest();

            Assert.IsType<JsonObject>(result);
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