namespace Facebook.Web.Tests.FacebookSession.ctor_dictionary
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenOnlyAValidUserAccessTokenWhichDoesntNotExpireThen
    {
        private readonly string accessToken;
        private readonly IDictionary<string, object> parameterWithAccessTokenOnly;

        public GivenOnlyAValidUserAccessTokenWhichDoesntNotExpireThen()
        {
            accessToken = "1249203702|76a68f298-100001327642026|q_BXv8TmYg";
            parameterWithAccessTokenOnly = new Dictionary<string, object> { { "access_token", accessToken } };
        }

        [Fact]
        public void AccessTokenShouldBeSetCorrectly()
        {
            var session = new FacebookSession(parameterWithAccessTokenOnly);

            Assert.Equal(accessToken, session.AccessToken);
        }

        [Fact]
        public void UserIdShouldNotBeNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenOnly);

            Assert.NotNull(session.UserId);
        }

        [Fact]
        public void UserIdShouldBeSetCorrectly()
        {
            var session = new FacebookSession(parameterWithAccessTokenOnly);

            Assert.Equal(100001327642026, session.UserId);
        }

        [Fact]
        public void SecretIsNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenOnly);

            Assert.Null(session.Secret);
        }

        [Fact]
        public void SecretKeyIsNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenOnly);

            Assert.Null(session.SessionKey);
        }

        [Fact]
        public void ExpiresIsMinvalue()
        {
            var session = new FacebookSession(parameterWithAccessTokenOnly);

            Assert.Equal(DateTime.MinValue, session.Expires);
        }

        [Fact]
        public void SignatureIsNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenOnly);

            Assert.Null(session.Signature);
        }

        [Fact]
        public void BaseDomainIsNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenOnly);

            Assert.Null(session.BaseDomain);
        }
    }
}