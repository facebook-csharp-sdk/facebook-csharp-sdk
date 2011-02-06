namespace Facebook.Web.Tests.FacebookWebUtils.VerifyHttpXHubSignature
{
    using Facebook.Web;
    using Xunit;
    using Xunit.Extensions;

    public class GivenSignatureWithoutStartingSha1EqualsThen
    {
        [Theory]
        [InlineData("doesnt start with sha1")]
        [InlineData("false")]
        public void ResultIsFalse(string httpXHubSignature)
        {
            string secret = "dummysecret";
            string jsonString = "{value:\"dummy json\"}";

            var result = FacebookWebUtils.VerifyHttpXHubSignature(secret, httpXHubSignature, jsonString);

            Assert.False(result);
        }
    }
}