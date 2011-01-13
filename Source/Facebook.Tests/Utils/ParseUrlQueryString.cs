namespace Facebook.Utils.Tests
{
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public class ParseUrlQueryString
    {
        [Fact(DisplayName = "ParseUrlQueryString: Given a querystring as null Then the result should not be null")]
        public void ParseUrlQueryString_GivenAQuerystringAsNull_ThenTheResultShouldNotBeNull()
        {
            var result = FacebookUtils.ParseUrlQueryString(null);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "ParseUrlQueryString: Given a querystring as null Then the count of result should be 0")]
        public void ParseUrlQueryString_GivenAQuerystringAsNull_ThenTheCountOfResultShouldBe0()
        {
            var result = FacebookUtils.ParseUrlQueryString(null);

            Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "ParseUrlQueryString: Given a querystring as string.Empty Then the result should not be null")]
        public void ParseUrlQueryString_GivenAQuerystringAsStringEmpty_ThenTheResultShouldNotBeNull()
        {
            var result = FacebookUtils.ParseUrlQueryString(string.Empty);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "ParseUrlQueryString: Given a querystring as string.Empty Then the count of result should be 0")]
        public void ParseUrlQueryString_GivenAQuerystringAsStringEmpty_ThenTheCountOfResultShouldBe0()
        {
            var result = FacebookUtils.ParseUrlQueryString(string.Empty);

            Assert.Equal(0, result.Count);
        }

        [Fact(DisplayName = "ParseUrlQueryString: Given a querystring as whitespace Then the result should not be null")]
        public void ParseUrlQueryString_GivenAQuerystringAsWhitespace_ThenTheResultShouldNotBeNull()
        {
            string whiteSpaceQueryString = "    ";
            var result = FacebookUtils.ParseUrlQueryString(whiteSpaceQueryString);

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "ParseUrlQueryString: Given a querystring as whitespace Then the count of result should be 0")]
        public void ParseUrlQueryString_GivenAQuerystringAsWhitespace_ThenTheCountOfResultShouldBe0()
        {
            string whiteSpaceQueryString = "    ";
            var result = FacebookUtils.ParseUrlQueryString(whiteSpaceQueryString);

            Assert.Equal(0, result.Count);
        }

        [PropertyData("ValidQueryStrings")]
        [Theory(DisplayName = "ParseUrlQueryString: Given a valid querystring Then the count of result should be number of querystring keys")]
        public void ParseUrlQueryString_GivenAValidQuerystring_ThenTheCountOfResultShouldBeNumberOfQuerystringKeys(string queryString, int total)
        {
            var result = FacebookUtils.ParseUrlQueryString(queryString);

            Assert.Equal(total, result.Count);
        }

        [Fact(DisplayName = "ParseUrlQueryString: Given a query string with acess token and expires in Then access token should be decoded correctly")]
        public void ParseUrlQueryString_GivenAQueryStringWithAcessTokenAndExpiresIn_ThenAccessTokenShouldBeDecodedCorrectly()
        {
            var queryString =
                "access_token=124973200873702%7C2.16KX_wTFlY2IAvWucsCKWA__.3600.1294927200-100001327642026%7CERLPsyFd8CP4ZI57VzAn0nl6WXo&expires_in=3699";

            var result = FacebookUtils.ParseUrlQueryString(queryString);

            Assert.Equal("124973200873702|2.16KX_wTFlY2IAvWucsCKWA__.3600.1294927200-100001327642026|ERLPsyFd8CP4ZI57VzAn0nl6WXo", result["access_token"]);
        }

        [Fact(DisplayName = "ParseUrlQueryString: Given a query string with acess token and expires in Then expires_in should be decoded correctly")]
        public void ParseUrlQueryString_GivenAQueryStringWithAcessTokenAndExpiresIn_ThenExpires_inShouldBeDecodedCorrectly()
        {
            var queryString =
                "access_token=124973200873702%7C2.16KX_wTFlY2IAvWucsCKWA__.3600.1294927200-100001327642026%7CERLPsyFd8CP4ZI57VzAn0nl6WXo&expires_in=3699";

            var result = FacebookUtils.ParseUrlQueryString(queryString);

            Assert.Equal("3699", result["expires_in"]);
        }

        [Fact(DisplayName = "ParseUrlQueryString: Given a query string with error_description containing + sign Then error_description  should be decoded correctly")]
        public void ParseUrlQueryString_GivenAQueryStringWithError_descriptionContainingPlusSign_ThenError_descriptionShouldBeDecodedCorrectly()
        {
            var queryString = "error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.";

            var result = FacebookUtils.ParseUrlQueryString(queryString);

            Assert.Equal("The user denied your request.", result["error_description"]);
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