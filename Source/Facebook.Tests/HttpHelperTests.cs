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
            public void UrlEncodeDoesNotThrow()
            {
                var sb = new StringBuilder();
                for (int i = 0; i <= 100000; i++)
                {
                    sb.Append("a");
                }
                Assert.DoesNotThrow(() => HttpHelper.UrlEncode(sb.ToString()));
            }

            [Fact]
            public void UrlEncodeEncodes1000Chars()
            {
                var charCount = 1000;

                UrlEncodeEncodes(charCount);
            }

            [Fact]
            public void UrlEncodeEncodesMoreThan1000Chars()
            {
                var charCount = 10001;

                UrlEncodeEncodes(charCount);
            }

            private void UrlEncodeEncodes(int charCount)
            {
                var sb = new StringBuilder(charCount);
                var sbVerification = new StringBuilder(3 * charCount);

                for (int i = 0; i < charCount; i++)
                {
                    sb.Append("/");
                    sbVerification.Append("%2F");
                }

                var result = HttpHelper.UrlEncode(sb.ToString());

                Assert.True(sbVerification.ToString().Equals(result, StringComparison.InvariantCultureIgnoreCase));
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