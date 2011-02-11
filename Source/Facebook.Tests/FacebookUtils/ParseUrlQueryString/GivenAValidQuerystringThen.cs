namespace Facebook.Tests.FacebookUtils.ParseUrlQueryString
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAValidQuerystringThen
    {
        [PropertyData("ValidQueryStrings")]
        [Theory]
        public void TheCountOfResultShouldBeNumberOfQuerystringKeys(string queryString, int total)
        {
            var result = FacebookUtils.ParseUrlQueryString(queryString);

            Assert.Equal(total, result.Count);
        }

        public static IEnumerable<object[]> ValidQueryStrings
        {
            get
            {
                yield return new object[] { "access_token=124973200873702%7C2.16KX_wTFlY2IAvWucsCKWA__.3600.1294927200-100001327642026%7CERLPsyFd8CP4ZI57VzAn0nl6WXo&expires_in=3699", 2 };
                yield return new object[] { "type=user_agent&client_id=123&redirect_uri=http://www.facebook.com/connect/login_success.html&display=popup", 4 };
                yield return new object[] { "error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.", 3 };
            }
        }
    }
}