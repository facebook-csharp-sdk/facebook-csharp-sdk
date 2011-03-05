namespace Facebook.Tests.FacebookClient.Api.GivenHttpMethodAsGet.AndResultTypeAsNull.AndParametersAsNull.AndAValidGraphPath
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Facebook;
    using Xunit;

    public class WhichReturnsJsonObjectThen
    {
        private FacebookClient facebookClient;

        private string path = "/4";
        private IDictionary<string, object> parameters = null;
        private HttpMethod httpMethod = HttpMethod.Get;
        private Type resultType = null;

        private string requestUrl = "https://graph.facebook.com/4";
        private string jsonResult = "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}";

        public WhichReturnsJsonObjectThen()
        {
            this.facebookClient = new FacebookClient
                                      {
                                          WebClient = WebClientFakes.DownloadAndUploadData(requestUrl, jsonResult)
                                      };
        }

        [Fact]
        public void DoesNotThrowError()
        {
            Assert.DoesNotThrow(() => this.ExecuteMethodToTest());
        }

        [Fact]
        public void DoesNotContainContentTypeHeader()
        {
            ExecuteMethodToTest();

            bool hasContentTypeHeader = this.facebookClient.WebClient.Headers.AllKeys.Contains("Content-Type", StringComparer.OrdinalIgnoreCase);

            Assert.False(hasContentTypeHeader);
        }

        [Fact]
        public void GetCompletedEventShouldNotBeFired()
        {
            bool executed = false;
            this.facebookClient.GetCompleted += (o, e) => executed = true;

            this.ExecuteMethodToTest();

            Assert.False(executed);
        }

        [Fact]
        public void DeleteCompletedEventShouldNotBeFired()
        {
            bool executed = false;
            this.facebookClient.DeleteCompleted += (o, e) => executed = true;

            this.ExecuteMethodToTest();

            Assert.False(executed);
        }

        [Fact]
        public void PostCompletedEventShouldNotBeFired()
        {
            bool executed = false;
            this.facebookClient.PostCompleted += (o, e) => executed = true;

            this.ExecuteMethodToTest();

            Assert.False(executed);
        }

        [Fact]
        public void ResultShouldNotBeNull()
        {
            var result = this.ExecuteMethodToTest();

            Assert.NotNull(result);
        }

        [Fact]
        public void ResultIsAssignableFromIDictionaryStringObject()
        {
            var result = this.ExecuteMethodToTest();

            Assert.IsAssignableFrom<IDictionary<string, object>>(result);
        }

        [Fact]
        public void ResultIsTypeOfJsonObject()
        {
            var result = this.ExecuteMethodToTest();

            Assert.IsType<JsonObject>(result);
        }

        public void Dispose()
        {
            this.facebookClient.Dispose();
        }

        private object ExecuteMethodToTest()
        {
            return this.facebookClient.Api(this.path, this.parameters, this.httpMethod, this.resultType);
        }
    }
}