namespace Facebook.Web.Tests.FacebookWebUtils.VerifyHttpXHubSignature
{
    using Facebook.Web;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAEmptyJsonString
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ReturnsFalse(string jsonString)
        {
            string secret = "dummysecret";
            string httpXHubSignature = "sha1=4594ae916543cece9de48e3289a5ab568f514b6a";

            var result = FacebookWebUtils.VerifyHttpXHubSignature(secret, httpXHubSignature, jsonString);

            Assert.False(result);
        }
    }
}