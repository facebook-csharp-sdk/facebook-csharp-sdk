namespace Facebook.Web.Tests.FacebookSession.ctor_dictionary
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAnAccessTokenAndUserIdThen
    {
        private string accessToken;
        private IDictionary<string, object> parameterWithAccessTokenAndUIdOnly;
        private long uid;

        public GivenAnAccessTokenAndUserIdThen()
        {
            accessToken = "1249203702|76a68f298-100001327642026|q_BXv8TmYg";
            uid = 123;
            parameterWithAccessTokenAndUIdOnly = new Dictionary<string, object>
                                               {
                                                   { "access_token", accessToken },
                                                   { "uid", uid }
                                               };
        }

        [Fact]
        public void AccessTokenIsNotNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenAndUIdOnly);

            Assert.NotNull(session.AccessToken);
        }

        [Fact]
        public void AccessTokenIsSetCorrectly()
        {
            var session = new FacebookSession(parameterWithAccessTokenAndUIdOnly);

            Assert.Equal(accessToken, session.AccessToken);
        }

        [Fact]
        public void UserIdIsNotNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenAndUIdOnly);

            Assert.NotNull(session.UserId);
        }

        [Fact]
        public void UserIdIsEqualToUId()
        {
            var session = new FacebookSession(parameterWithAccessTokenAndUIdOnly);

            Assert.Equal(session.UserId, uid);
        }

        [Fact]
        public void SecretIsNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenAndUIdOnly);

            Assert.Null(session.Secret);
        }

        [Fact]
        public void SecretKeyIsNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenAndUIdOnly);

            Assert.Null(session.SessionKey);
        }

        [Fact]
        public void ExpiresIsMinvalue()
        {
            var session = new FacebookSession(parameterWithAccessTokenAndUIdOnly);

            Assert.Equal(DateTime.MinValue, session.Expires);
        }

        [Fact]
        public void SignatureIsNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenAndUIdOnly);

            Assert.Null(session.Signature);
        }

        [Fact]
        public void BaseDomainIsNull()
        {
            var session = new FacebookSession(parameterWithAccessTokenAndUIdOnly);

            Assert.Null(session.BaseDomain);
        }
    }
}