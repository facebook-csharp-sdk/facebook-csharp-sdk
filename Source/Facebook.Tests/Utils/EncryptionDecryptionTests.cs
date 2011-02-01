namespace Facebook.Utils.Tests
{
    using System.Linq;
    using Xunit;

    public class EncryptionDecryptionTests
    {
        [Fact(DisplayName = "DecryptAes256CBCNoPadding: Given an valid encrypted data, key and iv Then the it should be decoded correctly")]
        public void DecryptAes256CBCNoPadding_GivenAnValidEncryptedDataKeyAndIv_ThenTheItShouldBeDecodedCorrectly()
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

            var result = FacebookUtils.DecryptAes256CBCNoPadding(encryptedData, key, iv);

            Assert.Equal(result, "{\"access_token\":\"101244219942650|2.wdrSr7KyE_VwQ0fjwOfW9A__.3600.1287608400-499091902|XzxMQd-_4tjlC2VEgide4rmg6LI\",\"expires_in\":6412,\"user_id\":\"499091902\"}");
        }

        [Fact(DisplayName = "DecryptAes256CBCNoPadding: Given an valid encrypted data and iv as base64url encoded string and key as plain string Then the it should be decoded correctly")]
        public void DecryptAes256CBCNoPadding_GivenAnValidEncryptedDataAndIvAsBase64urlEncodedStringAndKeyAsPlainString_ThenTheItShouldBeDecodedCorrectly()
        {
            var encryptedData = FacebookUtils.Base64UrlDecode("YGym4poQnMTrEgiAOkFTVI85ll5RuUiEl-IgqfxTOTHQNIvVVI8Y8kVuOoeKaWOeassWFTEv0Qg_7wCCBEen7lUBBzlFJ1V63HJ3Af0SIngcxWTJ7L6YLatMmwXgDAvWn5PsfqzWk4ml9h9DLnYptWHmDGL6iBiOhN7WyI7p6oEpVriFuJw_chLoPb8a3vGDnoW8e2Sxx06A2x2xkiapvg");
            var key = System.Text.Encoding.UTF8.GetBytes("13750c9911fec5865d01f3bd00bdf4db");
            var iv = FacebookUtils.Base64UrlDecode("fDLJCW-yiXmuNa24eSarJg");

            var result = FacebookUtils.DecryptAes256CBCNoPadding(encryptedData, key, iv);

            Assert.Equal(result, "{\"access_token\":\"101244219942650|2.wdrSr7KyE_VwQ0fjwOfW9A__.3600.1287608400-499091902|XzxMQd-_4tjlC2VEgide4rmg6LI\",\"expires_in\":6412,\"user_id\":\"499091902\"}");
        }

        [Fact(DisplayName = "ComputeHmacSha256Hash: Given an valid data and key Then it should compute the hash correctly")]
        public void ComputeHmacSha256Hash_GivenAnValidDataAndKey_ThenItShouldComputeTheHashCorrectly()
        {
            // var data = System.Text.Encoding.UTF8.GetBytes("eyJhbGdvcml0aG0iOiJBRVMtMjU2LUNCQyBITUFDLVNIQTI1NiIsImlzc3VlZF9hdCI6MTI4NzYwMTk4OCwiaXYiOiJmRExKQ1cteWlYbXVOYTI0ZVNhckpnIiwicGF5bG9hZCI6IllHeW00cG9Rbk1UckVnaUFPa0ZUVkk4NWxsNVJ1VWlFbC1JZ3FmeFRPVEhRTkl2VlZJOFk4a1Z1T29lS2FXT2Vhc3NXRlRFdjBRZ183d0NDQkVlbjdsVUJCemxGSjFWNjNISjNBZjBTSW5nY3hXVEo3TDZZTGF0TW13WGdEQXZXbjVQc2ZxeldrNG1sOWg5RExuWXB0V0htREdMNmlCaU9oTjdXeUk3cDZvRXBWcmlGdUp3X2NoTG9QYjhhM3ZHRG5vVzhlMlN4eDA2QTJ4MnhraWFwdmcifQ");
            var data = new byte[]
                           {
                               101, 121, 74, 104, 98, 71, 100, 118, 99, 109, 108, 48, 97, 71, 48, 105, 79, 105, 74, 66, 82,
                               86, 77, 116, 77, 106, 85, 50, 76, 85, 78, 67, 81, 121, 66, 73, 84, 85, 70, 68, 76, 86, 78,
                               73, 81, 84, 73, 49, 78, 105, 73, 115, 73, 109, 108, 122, 99, 51, 86, 108, 90, 70, 57, 104,
                               100, 67, 73, 54, 77, 84, 73, 52, 78, 122, 89, 119, 77, 84, 107, 52, 79, 67, 119, 105, 97, 88,
                               89, 105, 79, 105, 74, 109, 82, 69, 120, 75, 81, 49, 99, 116, 101, 87, 108, 89, 98, 88, 86,
                               79, 89, 84, 73, 48, 90, 86, 78, 104, 99, 107, 112, 110, 73, 105, 119, 105, 99, 71, 70, 53,
                               98, 71, 57, 104, 90, 67, 73, 54, 73, 108, 108, 72, 101, 87, 48, 48, 99, 71, 57, 82, 98, 107,
                               49, 85, 99, 107, 86, 110, 97, 85, 70, 80, 97, 48, 90, 85, 86, 107, 107, 52, 78, 87, 120, 115,
                               78, 86, 74, 49, 86, 87, 108, 70, 98, 67, 49, 74, 90, 51, 70, 109, 101, 70, 82, 80, 86, 69,
                               104, 82, 84, 107, 108, 50, 86, 108, 90, 74, 79, 70, 107, 52, 97, 49, 90, 49, 84, 50, 57, 108,
                               83, 50, 70, 88, 84, 50, 86, 104, 99, 51, 78, 88, 82, 108, 82, 70, 100, 106, 66, 82, 90, 49,
                               56, 51, 100, 48, 78, 68, 81, 107, 86, 108, 98, 106, 100, 115, 86, 85, 74, 67, 101, 109,
                               120, 71, 83, 106, 70, 87, 78, 106, 78, 73, 83, 106, 78, 66, 90, 106, 66, 84, 83, 87, 53, 110,
                               89, 51, 104, 88, 86, 69, 111, 51, 84, 68, 90, 90, 84, 71, 70, 48, 84, 87, 49, 51, 87, 71,
                               100, 69, 81, 88, 90, 88, 98, 106, 86, 81, 99, 50, 90, 120, 101, 108, 100, 114, 78, 71, 49,
                               115, 79, 87, 103, 53, 82, 69, 120, 117, 87, 88, 66, 48, 86, 48, 104, 116, 82, 69, 100, 77,
                               78, 109, 108, 67, 97, 85, 57, 111, 84, 106, 100, 88, 101, 85, 107, 51, 99, 68, 90, 118, 82,
                               88, 66, 87, 99, 109, 108, 71, 100, 85, 112, 51, 88, 50, 78, 111, 84, 71, 57, 81, 89, 106,
                               104, 104, 77, 51, 90, 72, 82, 71, 53, 118, 86, 122, 104, 108, 77, 108, 78, 52, 101, 68, 65,
                               50, 81, 84, 74, 52, 77, 110, 104, 114, 97, 87, 70, 119, 100, 109, 99, 105, 102, 81
                           };

            // var key = System.Text.Encoding.UTF8.GetBytes("13750c9911fec5865d01f3bd00bdf4db");
            var key = new byte[]
                          {
                              49, 51, 55, 53, 48, 99, 57, 57, 49, 49, 102, 101, 99, 53, 56, 54, 53, 100, 48, 49, 102, 51,
                              98, 100, 48, 48, 98, 100, 102, 52, 100, 98
                          };

            var expected = new byte[]
                               {
                                   183, 173, 233, 101, 14, 16, 221, 148, 199, 38, 221, 33, 58, 194, 171, 99, 106, 91, 219,
                                   204, 81, 149, 219, 150, 210, 152, 56, 148, 191, 217, 134, 94
                               };

            var hash = FacebookUtils.ComputeHmacSha256Hash(data, key);

            Assert.True(expected.SequenceEqual(hash));
        }

        [Fact(DisplayName = "ComputeHmacSha256Hash: Given an valid data and key as strings Then it should compute the hash correctly")]
        public void ComputeHmacSha256Hash_GivenAnValidDataAndKeyAsStrings_ThenItShouldComputeTheHashCorrectly()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("eyJhbGdvcml0aG0iOiJBRVMtMjU2LUNCQyBITUFDLVNIQTI1NiIsImlzc3VlZF9hdCI6MTI4NzYwMTk4OCwiaXYiOiJmRExKQ1cteWlYbXVOYTI0ZVNhckpnIiwicGF5bG9hZCI6IllHeW00cG9Rbk1UckVnaUFPa0ZUVkk4NWxsNVJ1VWlFbC1JZ3FmeFRPVEhRTkl2VlZJOFk4a1Z1T29lS2FXT2Vhc3NXRlRFdjBRZ183d0NDQkVlbjdsVUJCemxGSjFWNjNISjNBZjBTSW5nY3hXVEo3TDZZTGF0TW13WGdEQXZXbjVQc2ZxeldrNG1sOWg5RExuWXB0V0htREdMNmlCaU9oTjdXeUk3cDZvRXBWcmlGdUp3X2NoTG9QYjhhM3ZHRG5vVzhlMlN4eDA2QTJ4MnhraWFwdmcifQ");
            var key = System.Text.Encoding.UTF8.GetBytes("13750c9911fec5865d01f3bd00bdf4db");

            var expected = new byte[]
                               {
                                   183, 173, 233, 101, 14, 16, 221, 148, 199, 38, 221, 33, 58, 194, 171, 99, 106, 91, 219,
                                   204, 81, 149, 219, 150, 210, 152, 56, 148, 191, 217, 134, 94
                               };

            var hash = FacebookUtils.ComputeHmacSha256Hash(data, key);

            Assert.True(expected.SequenceEqual(hash));
        }

        [Fact(DisplayName = "ComputerMd5Hash: Given a data Then it should generate correct md5 hash")]
        public void ComputerMd5Hash_GivenAData_ThenItShouldGenerateCorrectMd5Hash()
        {
            var input = System.Text.Encoding.UTF8.GetBytes("access_token=124973200873702|2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026|vz4H9xjlRZPfg2quCv0XOM5g9_oexpires=1295118000secret=lddpssZCuPoEtjcDFcWtoA__session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026uid=1000013276420263b4a872617be2ae1932baa1d4d240272");
            var expected = new byte[] { 29, 149, 250, 75, 61, 250, 91, 38, 192, 28, 138, 200, 103, 109, 128, 184 };

            var md5Hash = FacebookUtils.ComputerMd5Hash(input);

            Assert.True(expected.SequenceEqual(md5Hash));
        }
    }
}