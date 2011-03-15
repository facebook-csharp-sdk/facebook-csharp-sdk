namespace Facebook.Web.Tests.FacebookSession.ctor_accessToken
{
    using System;
    using Facebook;
    using Xunit;

    public class GivenAValidUserAccessTokenWhichDoesNotExpireThen
    {
        private readonly FacebookSession session;
        private readonly string accessToken;

        public GivenAValidUserAccessTokenWhichDoesNotExpireThen()
        {
            accessToken = "1249203702|76a68f298-100001327642026|q_BXv8TmYg";
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
            Assert.Equal(100001327642026, session.UserId);
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