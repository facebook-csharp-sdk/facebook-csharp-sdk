namespace Facebook.Tests.FacebookUtils.ParseQueryParametersToDictionary
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAPathStartingWithForwardSlashAndNoQueryStringAndEmptyParametersThen
    {
        [Fact(DisplayName = "ParseUrlParameters: Given a path starting with Forward slash and empty parameters Then return path equals the path without forward slash")]
        public void ReturnPathEqualsThePathWithoutForwardSlash()
        {
            string originalPath = "/me/likes";
            string pathWithoutForwardSlash = "me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookUtils.ParseQueryParametersToDictionary(originalPath, parameters);

            Assert.Equal(pathWithoutForwardSlash, path);
        }
    }
}