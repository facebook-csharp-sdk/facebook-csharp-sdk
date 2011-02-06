namespace Facebook.Web.Tests.FacebookWebUtils.VerifyHttpXHubSignature
{
    using Facebook.Web;
    using Xunit;
    using Xunit.Extensions;

    public class GivenHttpXHubSignatureAsNullOrEmptyThen
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ReturnsFalse(string httpXHubSignature)
        {
            string secret = "dummysecret";
            string jsonString = "{value:\"dummy json\"}";

            var result = FacebookWebUtils.VerifyHttpXHubSignature(secret, httpXHubSignature, jsonString);

            Assert.False(result);
        }
    }
}