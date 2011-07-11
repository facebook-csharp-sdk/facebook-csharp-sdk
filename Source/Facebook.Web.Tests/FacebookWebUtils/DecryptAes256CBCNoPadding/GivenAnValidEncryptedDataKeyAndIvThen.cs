namespace Facebook.Web.Tests.FacebookUtils.DecryptAes256CBCNoPadding
{
    using Facebook.Web;
    using Xunit;

    public class GivenAnValidEncryptedDataKeyAndIvThen
    {
        [Fact]
        public void ItShouldBeDecodedCorrectly()
        {
            // encryptedData = FacebookUtils.Base64UrlDecode("YGym4poQnMTrEgiAOkFTVI85ll5RuUiEl-IgqfxTOTHQNIvVVI8Y8kVuOoeKaWOeassWFTEv0Qg_7wCCBEen7lUBBzlFJ1V63HJ3Af0SIngcxWTJ7L6YLatMmwXgDAvWn5PsfqzWk4ml9h9DLnYptWHmDGL6iBiOhN7WyI7p6oEpVriFuJw_chLoPb8a3vGDnoW8e2Sxx06A2x2xkiapvg");
            var encryptedData = new byte[]
                                    {
                                        96, 108, 166, 226, 154, 16, 156, 196, 235, 18, 8, 128, 58, 65, 83, 84, 143, 57, 150,
                                        94, 81, 185, 72, 132, 151, 226, 32, 169, 252, 83, 57, 49, 208, 52, 139, 213, 84, 143,
                                        24, 242, 69, 110, 58, 135, 138, 105, 99, 158, 106, 203, 22, 21, 49, 47, 209, 8, 63, 
                                        239, 0, 130, 4, 71, 167, 238, 85, 1, 7, 57, 69, 39, 85, 122, 220, 114, 119, 1,
                                        253, 18, 34, 120, 28, 197, 100, 201, 236, 190, 152, 45, 171, 76, 155, 5, 224, 12,
                                        11, 214, 159, 147, 236, 126, 172, 214, 147, 137, 165, 246, 31, 67, 46, 118, 41, 181,
                                        97, 230, 12, 98, 250, 136, 24, 142, 132, 222, 214, 200, 142, 233, 234, 129, 41,
                                        86, 184, 133, 184, 156, 63, 114, 18, 232, 61, 191, 26, 222, 241, 131, 158, 133, 188,
                                        123, 100, 177, 199, 78, 128, 219, 29, 177, 146, 38, 169, 190
                                    };

            // key = Encoding.UTF8.GetBytes("13750c9911fec5865d01f3bd00bdf4db")
            var key = new byte[]
                          {
                              49, 51, 55, 53, 48, 99, 57, 57, 49, 49, 102, 101, 99, 53, 56, 54, 53, 100, 48, 49, 102, 51,
                              98, 100, 48, 48, 98, 100, 102, 52, 100, 98
                          };

            // iv = FacebookUtils.Base64UrlDecode("fDLJCW-yiXmuNa24eSarJg")
            var iv = new byte[] { 124, 50, 201, 9, 111, 178, 137, 121, 174, 53, 173, 184, 121, 38, 171, 38 };

            var result = FacebookWebUtils.DecryptAes256CBCNoPadding(encryptedData, key, iv);

            Assert.Equal(result, "{\"access_token\":\"101244219942650|2.wdrSr7KyE_VwQ0fjwOfW9A__.3600.1287608400-499091902|XzxMQd-_4tjlC2VEgide4rmg6LI\",\"expires_in\":6412,\"user_id\":\"499091902\"}");
        }
    }
}