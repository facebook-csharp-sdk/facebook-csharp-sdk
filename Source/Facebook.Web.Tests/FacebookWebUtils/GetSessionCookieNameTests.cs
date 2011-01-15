namespace Facebook.Web.Tests.FacebookWebUtils
{
    using Facebook.Web;
    using Xunit;
    using Xunit.Extensions;

    public class GetSessionCookieNameTests
    {
        [InlineData("dummyappid")]
        [Theory(DisplayName = "GetSessionCookieName: Given a app id Then it returns fbs_ contatenated with app id")]
        public void GetSessionCookieName_GivenAAppId_ThenItReturnsFbsUnderscoreContatenatedWithAppId(string appId)
        {
            var expectedCookieName = string.Concat("fbs_", appId);

            var result = FacebookWebUtils.GetSessionCookieName(appId);

            Assert.Equal(expectedCookieName, result);
        }
    }
}