namespace Facebook.Web.Tests.FacebookSession.ctor_accessToken
{
    using System;
    using Facebook;
    using Xunit;

    public class GivenAValidUserAccessTokenWhichExpiresThen
    {
        private readonly FacebookSession session;
        private readonly string accessToken;

        public GivenAValidUserAccessTokenWhichExpiresThen()
        {
            accessToken = "1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc";
            session = new FacebookSession(accessToken);
        }

        [Fact]
        public void AccessTokenShouldBeSetCorrectly()
        {
            Assert.Equal(accessToken, session.AccessToken);
        }

        [Fact]
        public void UserIdShouldNotBeNull()
        {
            Assert.NotNull(session.UserId);
        }

        [Fact]
        public void UserIdShouldBeSetCorrectly()
        {
            Assert.Equal(605430316, session.UserId);
        }

        [Fact]
        public void SecretIsNull()
        {
            Assert.Null(session.Secret);
        }

        [Fact]
        public void SecretKeyIsNull()
        {
            Assert.Null(session.SessionKey);
        }

        [Fact]
        public void ExpiresIsMinvalue()
        {
            Assert.Equal(DateTime.MinValue, session.Expires);
        }

        [Fact]
        public void SignatureIsNull()
        {
            Assert.Null(session.Signature);
        }

        [Fact]
        public void BaseDomainIsNull()
        {
            Assert.Null(session.BaseDomain);
        }
    }
}