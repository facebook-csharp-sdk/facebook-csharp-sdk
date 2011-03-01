namespace Facebook.Tests.ExceptionFactory.GetRestException
{
    using Facebook;
    using Xunit;

    public class GivenInputAsNullThen
    {
        [Fact]
        public void ResultIsNull()
        {
            object input = null;

            var result = ExceptionFactory.GetRestException(input);

            Assert.Null(result);
        }
    }
}