namespace Facebook.Tests.FacebookWebUtils.ComputeHmacSha256Hash
{
    using System.Linq;
    using Facebook.Web;
    using Xunit;

    public class GivenAnValidDataAndKeyAsStringsThen
    {
        [Fact]
        public void ItShouldComputeTheHashCorrectly()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("eyJhbGdvcml0aG0iOiJBRVMtMjU2LUNCQyBITUFDLVNIQTI1NiIsImlzc3VlZF9hdCI6MTI4NzYwMTk4OCwiaXYiOiJmRExKQ1cteWlYbXVOYTI0ZVNhckpnIiwicGF5bG9hZCI6IllHeW00cG9Rbk1UckVnaUFPa0ZUVkk4NWxsNVJ1VWlFbC1JZ3FmeFRPVEhRTkl2VlZJOFk4a1Z1T29lS2FXT2Vhc3NXRlRFdjBRZ183d0NDQkVlbjdsVUJCemxGSjFWNjNISjNBZjBTSW5nY3hXVEo3TDZZTGF0TW13WGdEQXZXbjVQc2ZxeldrNG1sOWg5RExuWXB0V0htREdMNmlCaU9oTjdXeUk3cDZvRXBWcmlGdUp3X2NoTG9QYjhhM3ZHRG5vVzhlMlN4eDA2QTJ4MnhraWFwdmcifQ");
            var key = System.Text.Encoding.UTF8.GetBytes("13750c9911fec5865d01f3bd00bdf4db");

            var expected = new byte[]
                               {
                                   183, 173, 233, 101, 14, 16, 221, 148, 199, 38, 221, 33, 58, 194, 171, 99, 106, 91, 219,
                                   204, 81, 149, 219, 150, 210, 152, 56, 148, 191, 217, 134, 94
                               };

            var hash = FacebookWebUtils.ComputeHmacSha256Hash(data, key);

            Assert.True(expected.SequenceEqual(hash));
        }

    }
}