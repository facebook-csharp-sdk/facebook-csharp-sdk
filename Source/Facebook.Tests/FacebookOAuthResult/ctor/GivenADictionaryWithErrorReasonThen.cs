namespace Facebook.Tests.FacebookOAuthResult.ctor
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenADictionaryWithErrorReasonThen
    {
        [Fact]
        public void IsSuccessShouldBeFalse()
        {
            var parameters = new Dictionary<string, object>
                                 {
                                     { "error_reason", "dummy error reason" }
                                 };

            var result = new FacebookOAuthResult(parameters);

            Assert.False(result.IsSuccess);
        }
    }
}