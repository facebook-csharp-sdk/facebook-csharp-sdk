
namespace Facebook.Tests.FacebookUtils
{
    using System;
    using Xunit;

    public class FromIso8601FormattedDateTimeTests
    {
        [Fact]
        public void GivenNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DateTimeConvertor.FromIso8601FormattedDateTime(null));
        }

        [Fact]
        public void GivenEmptyThrowsArgumnetNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DateTimeConvertor.FromIso8601FormattedDateTime(string.Empty));
        }
    }
}
