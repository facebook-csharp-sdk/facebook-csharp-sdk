namespace Facebook.Tests.FacebookOAuthResult.ctor
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenADictionaryWithCodeValueThen
    {
        [Fact]
        public void CodeShouldBeTheOneSpecifiedInDictionary()
        {
            var code = "2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026|t8SsfSR2XI6yhBAkhX95J7p9hJ0";
            var parameters = new Dictionary<string, object>
                                 {
                                     { "code", code }
                                 };

            var result = new FacebookOAuthResult(parameters);

            Assert.Equal(code, result.Code);
        }

        [Fact]
        public void IsSuccessShouldBeTrue()
        {
            var parameters = new Dictionary<string, object>
                                 {
                                     { "code", "dummycode" }
                                 };

            var result = new FacebookOAuthResult(parameters);

            Assert.True(result.IsSuccess);
        }
    }
}