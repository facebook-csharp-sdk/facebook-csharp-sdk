namespace Facebook.Tests.FacebookBatchParameter
{
    using System;
    using Facebook;
    using Xunit;

    public class FacebookBatchParameterTests
    {
        [Fact]
        public virtual void GivenNullFqlInQueryThrowsArgumentNullException()
        {
            var bp = new FacebookBatchParameter();

            Assert.Throws<ArgumentNullException>(() => bp.Query((string)null));
        }

        [Fact]
        public virtual void GivenEmptyFqlInQueryThrowsArgumentNullException()
        {
            var bp = new FacebookBatchParameter();

            Assert.Throws<ArgumentNullException>(() => bp.Query(string.Empty));
        }

        [Fact]
        public virtual void GivenNullFqlInMultiQueryThrowsArgumentNullException()
        {
            var bp = new FacebookBatchParameter();

            Assert.Throws<ArgumentNullException>(() => bp.Query((string[])null));
        }

        [Fact]
        public virtual void GivenEmptyFqlInMultiQueryThrowsArgumentNullException()
        {
            var bp = new FacebookBatchParameter();

            Assert.Throws<ArgumentOutOfRangeException>(() => bp.Query(new string[] { }));
        }
    }
}