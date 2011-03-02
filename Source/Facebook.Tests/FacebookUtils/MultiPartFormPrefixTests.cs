
namespace Facebook.Tests.FacebookUtils
{
    using Facebook;
    using Xunit;

    public class MultiPartFormPrefixTests
    {
        [Fact]
        public void EqualsDoubleDash()
        {
            var result = FacebookUtils.MultiPartFormPrefix;

            Assert.Equal("--", result);
        }
    }
}