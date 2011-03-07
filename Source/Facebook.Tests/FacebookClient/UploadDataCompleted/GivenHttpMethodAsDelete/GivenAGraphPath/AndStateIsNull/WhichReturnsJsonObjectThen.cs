namespace Facebook.Tests.FacebookClient.UploadDataCompleted.GivenHttpMethodAsDelete.GivenAGraphPath.AndStateIsNull
{
    using System;
    using System.Text;
    using Facebook;
    using Xunit;

    public class WhichReturnsJsonObjectThen
    {
        private FacebookClient facebookClient;

        private HttpMethod httpMethod = HttpMethod.Delete;

        private string requestUrl = "https://graph.facebook.com/100001327642026_159122094142044?access_token=dummyAccessToken";
        private string jsonResult = "true";

        private UploadDataCompletedEventArgsWrapper uploadDataCompletedEventArgsWrapper;

        public WhichReturnsJsonObjectThen()
        {
            this.facebookClient = new FacebookClient();

            var tempState = new WebClientStateContainer
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
        public void DeleteCompletedEventShouldBeFired()
        {
            var executed = false;
            this.facebookClient.DeleteCompleted += (o, e) => executed = true;

            ExecuteMethodToTest();

            Assert.True(executed);
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
            this.facebookClient.DeleteCompleted += (o, e) => isNull = e == null;

            ExecuteMethodToTest();

            Assert.False(isNull.Value);
        }

        [Fact]
        public void ErrorShouldBeNull()
        {
            Exception exception = null;
            this.facebookClient.DeleteCompleted += (o, e) => exception = e.Error;

            ExecuteMethodToTest();

            Assert.Null(exception);
        }

        [Fact]
        public void ResultShouldNotBeNull()
        {
            object result = null;
            this.facebookClient.DeleteCompleted += (o, e) => result = e.GetResultData();

            ExecuteMethodToTest();

            Assert.NotNull(result);
        }

        [Fact]
        public void ResultIsOfTypeBoolean()
        {
            object result = null;
            this.facebookClient.DeleteCompleted += (o, e) => result = e.GetResultData();

            ExecuteMethodToTest();

            Assert.IsType<bool>(result);
        }

        [Fact]
        public void UserStateIsNull()
        {
            object userState = null;
            this.facebookClient.DeleteCompleted += (o, e) => userState = e.UserState;

            ExecuteMethodToTest();

            Assert.Null(userState);
        }

        private void ExecuteMethodToTest()
        {
            this.facebookClient.UploadDataCompleted(this, this.uploadDataCompletedEventArgsWrapper);
        }
    }
}