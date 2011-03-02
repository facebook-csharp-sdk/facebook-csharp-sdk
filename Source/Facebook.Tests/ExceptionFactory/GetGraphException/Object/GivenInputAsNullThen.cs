namespace Facebook.Tests.ExceptionFactory.GetGraphException.Object
{
    using Facebook;
    using Xunit;

    public class GivenInputAsNullThen
    {
        [Fact]
        public void ResultIsNull()
        {
            object input = null;

            var result = ExceptionFactory.GetGraphException(input);

            Assert.Null(result);
        }
    }
}