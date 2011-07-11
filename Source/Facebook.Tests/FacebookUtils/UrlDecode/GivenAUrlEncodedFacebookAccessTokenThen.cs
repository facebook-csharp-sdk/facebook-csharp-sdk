namespace Facebook.Tests.FacebookUtils.UrlDecode
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAUrlEncodedFacebookAccessTokenThen
    {
        [InlineData("124983200973703%7C2.xZYnCri_odnkuj3xXUNDOA__.3600.1295836200-100001327642026%7CrPfJfZ38FcwV-8HzRGQdxio9D7B", "124983200973703|2.xZYnCri_odnkuj3xXUNDOA__.3600.1295836200-100001327642026|rPfJfZ38FcwV-8HzRGQdxio9D7B")]
        [InlineData("135972300873702%7C3.cxZrSyyPVHjISXQCB8MQ_g__.3600.1294833600-100001327642025%7Cjbo3zk3aHYVJiLWnKArjERsAU0c", "135972300873702|3.cxZrSyyPVHjISXQCB8MQ_g__.3600.1294833600-100001327642025|jbo3zk3aHYVJiLWnKArjERsAU0c")]
        [Theory(DisplayName = "UrlDecode: Given a url encoded facebook access token Then it should decode correctly")]
        public void ItShouldDecodeCorrectly(string encodedAccessToken, string expectedAccessToken)
        {
            var result = FluentHttp.HttpHelper.UrlDecode(encodedAccessToken);

            Assert.Equal(expectedAccessToken, result);
        }
    }
}