namespace Facebook.Tests.FacebookUtils.UrlEncode
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAUrlEncodedCodeThen
    {
        [InlineData("2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026%7Ct8SsfSR2XI6yhBAkhX95J7p9hJ0", "2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026|t8SsfSR2XI6yhBAkhX95J7p9hJ0")]
        [Theory]
        public void ItShouldDecodeCorrectly(string encodedCode, string expectedCode)
        {
            var result = FluentHttp.HttpHelper.UrlDecode(encodedCode);

            Assert.Equal(expectedCode, result);
        }
    }
}