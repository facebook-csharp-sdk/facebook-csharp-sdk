namespace Facebook.Tests.FacebookClient.Api.GivenHttpMethodAsGet.AndResultTypeAsNull.AndNonNullParameters.AndAValidRestApiPath
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
                                                                 { "query", "sd" },
                                                                 { "method", "fql.query" }                                                                 
                                                             };
        private HttpMethod httpMethod = HttpMethod.Get;
        private Type resultType = null;

        private string requestUrl = "https://api-read.facebook.com/restserver.php?query=sd&method=fql.query&format=json-strings";
        private string jsonResult = "{\"error_code\":\"601\",\"error_msg\":\"Parser error: unexpected 'sd' at position 0.\",\"request_args\":[{\"key\":\"query\",\"value\":\"sd\"},{\"key\":\"method\",\"value\":\"fql.query\"},{\"key\":\"format\",\"value\":\"json-strings\"}]}";

        public ThrowsRestApiErrorThen()
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