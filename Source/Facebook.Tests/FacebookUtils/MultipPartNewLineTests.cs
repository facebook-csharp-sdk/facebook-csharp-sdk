
namespace Facebook.Tests.FacebookUtils
{
    using Facebook;
    using Xunit;

    public class MultipPartNewLineTests
    {
        [Fact]
        public void EqualsSlashRSlashN()
        {
            var result = FacebookUtils.MultiPartNewLine;

            Assert.Equal("\r\n", result);
        }
    }
}