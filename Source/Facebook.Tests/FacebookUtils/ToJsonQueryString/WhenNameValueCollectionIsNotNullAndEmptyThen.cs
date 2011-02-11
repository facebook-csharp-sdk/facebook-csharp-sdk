namespace Facebook.Tests.FacebookUtils.ToJsonQueryString
{
    using System.Collections.Specialized;
    using Facebook;
    using Xunit;

    public class WhenNameValueCollectionIsNotNullAndEmptyThen
    {
        [Fact]
        public void ResultShouldBeEmptyString()
        {
            var nvc = new NameValueCollection();

            string result = FacebookUtils.ToJsonQueryString(nvc);

            Assert.Equal(string.Empty, result);
        }
    }
}