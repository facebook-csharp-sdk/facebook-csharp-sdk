namespace Facebook.Tests.FacebookUtils.ConvertToString
{
    using System;
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenANumberOutOfHttpMethodRangeThen
    {
        [InlineData(-1)]
        [InlineData(3)]
        [InlineData(4)]
        [Theory]
        public void ItShouldThrowInvalidOperationException(int number)
        {
            var httpMethod = (HttpMethod)number;

            Assert.Throws<InvalidOperationException>(
                () => FacebookUtils.ConvertToString(httpMethod));
        }
    }
}