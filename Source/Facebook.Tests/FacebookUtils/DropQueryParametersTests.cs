
namespace Facebook.Tests.FacebookUtils
{
    using Facebook;
    using Xunit;

    public class DropQueryParametersTests
    {
        [Fact]
        public void CountIs2()
        {
            var result = FacebookUtils.DropQueryParameters.Count;

            Assert.Equal(2, result);
        }

        [Fact]
        public void ContainsSession()
        {
            var result = FacebookUtils.DropQueryParameters.Contains("session");

            Assert.True(result);
        }

        [Fact]
        public void ContainsSignedRequest()
        {
            var result = FacebookUtils.DropQueryParameters.Contains("signed_request");

            Assert.True(result);
        }
    }
}