namespace Facebook.Tests.FacebookUtils.ParseUrlQueryString
{
    using Facebook;
    using Xunit;

    public class GivenAQuerystringWithCodeThen
    {
        [Fact]
        public void ItShouldDecodeCorrectly()
        {
            var queryString = "code=2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026%7Ct8SsfSR2XI6yhBAkhX95J7p9hJ0";

            var result = FacebookUtils.ParseUrlQueryString(queryString);

            Assert.Equal("2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026|t8SsfSR2XI6yhBAkhX95J7p9hJ0", result["code"]);
        }
    }
}