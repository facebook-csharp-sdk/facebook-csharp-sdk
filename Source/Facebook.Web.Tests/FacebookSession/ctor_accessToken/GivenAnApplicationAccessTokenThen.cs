namespace Facebook.Web.Tests.FacebookSession.ctor_accessToken
{
    using System;
    using Facebook;
    using Xunit;

    public class GivenAnApplicationAccessTokenThen
    {
        private readonly FacebookSession session;
        private readonly string accessToken;

        public GivenAnApplicationAccessTokenThen()
        {
            string appId = "123";
            string appSecret = " A12aB";
            accessToken = string.Format("{0}|{1}", appId, appSecret);

            session = new FacebookSession(accessToken);
        }

        [Fact]
        public void AccessTokenShouldBeSetCorrectly()
        {
            Assert.Equal(accessToken, session.AccessToken);
        }

        [Fact]
        public void UserIdShouldBe0()
        {
            Assert.Equal(0,session.UserId);
        }

        [Fact]
        public void UserIdIs0()
        {
            Assert.Equal(0, session.UserId);
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