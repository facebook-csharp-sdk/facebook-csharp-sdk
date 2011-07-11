namespace Facebook.Web.Tests.FacebookWebUtils.ComputerMd5Hash
{
    using System.Linq;
    using Facebook.Web;
    using Xunit;

    public class GivenADataThen
    {
        [Fact]
        public void ItShouldGenerateCorrectMd5Hash()
        {
            var input = System.Text.Encoding.UTF8.GetBytes("access_token=124973200873702|2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026|vz4H9xjlRZPfg2quCv0XOM5g9_oexpires=1295118000secret=lddpssZCuPoEtjcDFcWtoA__session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026uid=1000013276420263b4a872617be2ae1932baa1d4d240272");
            var expected = new byte[] { 29, 149, 250, 75, 61, 250, 91, 38, 192, 28, 138, 200, 103, 109, 128, 184 };

            var md5Hash = FacebookWebUtils.ComputerMd5Hash(input);

            Assert.True(expected.SequenceEqual(md5Hash));
        }
    }
}