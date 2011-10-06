
namespace Facebook.Tests.FacebookApplication
{
    using System;
    using Facebook;
    using Xunit;

    public class SetApplicationTests
    {
        [Fact]
        public void GivenIFacebookApplicationAsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => FacebookApplication.SetApplication((IFacebookApplication)null));
        }

        [Fact]
        public void GivenFuncIFacebookApplicationAsNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => FacebookApplication.SetApplication((Func<IFacebookApplication>)null));
        }
    }
}
