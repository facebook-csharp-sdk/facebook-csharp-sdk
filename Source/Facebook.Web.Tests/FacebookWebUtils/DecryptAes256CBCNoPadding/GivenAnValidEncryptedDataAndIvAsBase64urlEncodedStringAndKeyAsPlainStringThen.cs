namespace Facebook.Web.Tests.FacebookWebUtils.DecryptAes256CBCNoPadding
{
    using Facebook.Web;
    using Xunit;

    public class GivenAnValidEncryptedDataAndIvAsBase64urlEncodedStringAndKeyAsPlainStringThen
    {
        [Fact]
        public void ItShouldBeDecodedCorrectly()
        {
            var encryptedData = FacebookWebUtils.Base64UrlDecode("YGym4poQnMTrEgiAOkFTVI85ll5RuUiEl-IgqfxTOTHQNIvVVI8Y8kVuOoeKaWOeassWFTEv0Qg_7wCCBEen7lUBBzlFJ1V63HJ3Af0SIngcxWTJ7L6YLatMmwXgDAvWn5PsfqzWk4ml9h9DLnYptWHmDGL6iBiOhN7WyI7p6oEpVriFuJw_chLoPb8a3vGDnoW8e2Sxx06A2x2xkiapvg");
            var key = System.Text.Encoding.UTF8.GetBytes("13750c9911fec5865d01f3bd00bdf4db");
            var iv = FacebookWebUtils.Base64UrlDecode("fDLJCW-yiXmuNa24eSarJg");

            var result = FacebookWebUtils.DecryptAes256CBCNoPadding(encryptedData, key, iv);

            Assert.Equal(result, "{\"access_token\":\"101244219942650|2.wdrSr7KyE_VwQ0fjwOfW9A__.3600.1287608400-499091902|XzxMQd-_4tjlC2VEgide4rmg6LI\",\"expires_in\":6412,\"user_id\":\"499091902\"}");
        }
    }
}