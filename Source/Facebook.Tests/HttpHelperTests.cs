using System;
using System.Text;
using Xunit;

namespace Facebook.Tests
{
    public class HttpHelperTests
    {
        public class UrlEncodeTests
        {
            [Fact]
            public void UrlEncodeTest()
            {
                var sb = new StringBuilder();
                for (int i = 0; i <= 100000; i++)
                {
                    sb.Append("a");
                }
                Assert.DoesNotThrow(() => HttpHelper.UrlEncode(sb.ToString()));
            }

            [Fact]
            public void UrlEncodeNullString()
            {
                Assert.Throws<ArgumentNullException>(() => HttpHelper.UrlEncode(null));
            }

            [Fact]
            public void UrlEncodeEmptyString()
            {
                var result = HttpHelper.UrlEncode("");
                Assert.Equal("", result);
            }
        }

    }
}