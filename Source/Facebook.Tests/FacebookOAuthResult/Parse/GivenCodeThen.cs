namespace Facebook.Tests.FacebookOAuthResult.Parse
{
    using Facebook;
    using Xunit;

    public class GivenCodeThen
    {
        [Fact]
        public void TheCodeShouldBeTheSame()
        {
            var url = "http://www.facebook.com/connect/login_success.html?code=2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026|t8SsfSR2XI6yhBAkhX95J7p9hJ0";

            var result = FacebookOAuthResult.Parse(url);

            Assert.Equal("2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026|t8SsfSR2XI6yhBAkhX95J7p9hJ0", result.Code);
        }
    }
}