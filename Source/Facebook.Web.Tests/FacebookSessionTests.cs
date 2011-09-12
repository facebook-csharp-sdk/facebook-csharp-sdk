namespace Facebook.Tests
{
    using System.Collections.Generic;
    using Facebook.Web;
    using Xunit;

    public class FacebookSessionTests
    {
        [Fact(DisplayName = "GenerateSessionSignature: Given valid secret and correct parameters Then it should generate correct session signature")]
        public void GenerateSessionSignature_GivenValidSecretAndCorrectParameters_ThenItShouldGenerateCorrectSessionSignature()
        {
            var secret = "3b4a872617be2ae1932baa1d4d240272";
            var dictionary = new Dictionary<string, object>
                                 {
                                     { "access_token", "124973200873702|2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026|vz4H9xjlRZPfg2quCv0XOM5g9_o" },
                                     { "expires", "1295118000" },
                                     { "secret", "lddpssZCuPoEtjcDFcWtoA__" },
                                     { "session_key", "2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026" },
                                     { "sig", "1d95fa4b3dfa5b26c01c8ac8676d80b8" },
                                     { "uid", "100001327642026" }
                                 };
            var expectedSignature = "1d95fa4b3dfa5b26c01c8ac8676d80b8";

            var signature = FacebookSession.GenerateSessionSignature(secret, dictionary);

            Assert.Equal(expectedSignature, signature);
        }

        [Fact(DisplayName = "GenerateSessionSignature: Given valid secret and correct parameters without sig Then it should generate correct session signature")]
        public void GenerateSessionSignature_GivenValidSecretAndCorrectParametersWithoutSig_ThenItShouldGenerateCorrectSessionSignature()
        {
            var secret = "3b4a872617be2ae1932baa1d4d240272";
            var dictionary = new Dictionary<string, object>
                                 {
                                     { "access_token", "124973200873702|2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026|vz4H9xjlRZPfg2quCv0XOM5g9_o" },
                                     { "expires", "1295118000" },
                                     { "secret", "lddpssZCuPoEtjcDFcWtoA__" },
                                     { "session_key", "2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026" },
                                     { "uid", "100001327642026" }
                                 };
            var expectedSignature = "1d95fa4b3dfa5b26c01c8ac8676d80b8";

            var signature = FacebookSession.GenerateSessionSignature(secret, dictionary);

            Assert.Equal(expectedSignature, signature);
        }

        [Fact(DisplayName = "GenerateSessionSignature: Given valid secret and correct parameters in random order Then it should generate correct session signature")]
        public void GenerateSessionSignature_GivenValidSecretAndCorrectParametersInRandomOrder_ThenItShouldGenerateCorrectSessionSignature()
        {
            var secret = "3b4a872617be2ae1932baa1d4d240272";
            var dictionary = new Dictionary<string, object>
                                 {
                                     { "expires", "1295118000" },
                                     { "uid", "100001327642026" },
                                     { "access_token", "124973200873702|2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026|vz4H9xjlRZPfg2quCv0XOM5g9_o" },
                                     { "secret", "lddpssZCuPoEtjcDFcWtoA__" },
                                     { "sig", "1d95fa4b3dfa5b26c01c8ac8676d80b8" },
                                     { "session_key", "2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026" }
                                 };
            var expectedSignature = "1d95fa4b3dfa5b26c01c8ac8676d80b8";

            var signature = FacebookSession.GenerateSessionSignature(secret, dictionary);

            Assert.Equal(expectedSignature, signature);
        }

        [Fact(DisplayName = "GenerateSessionSignature: Given valid secret and correct parameters in random order without sig Then it should generate correct session signature")]
        public void GenerateSessionSignature_GivenValidSecretAndCorrectParametersInRandomOrderWithoutSig_ThenItShouldGenerateCorrectSessionSignature()
        {
            var secret = "3b4a872617be2ae1932baa1d4d240272";
            var dictionary = new Dictionary<string, object>
                                 {
                                     { "expires", "1295118000" },
                                     { "uid", "100001327642026" },
                                     { "access_token", "124973200873702|2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026|vz4H9xjlRZPfg2quCv0XOM5g9_o" },
                                     { "secret", "lddpssZCuPoEtjcDFcWtoA__" },
                                     { "session_key", "2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026" }
                                 };
            var expectedSignature = "1d95fa4b3dfa5b26c01c8ac8676d80b8";

            var signature = FacebookSession.GenerateSessionSignature(secret, dictionary);

            Assert.Equal(expectedSignature, signature);
        }

        [Fact(DisplayName = "ParseCookieValue: Given valid fbs_ cookie value Then result should not be null")]
        public void ParseCookieValue_GivenValidFbsCookieValue_ThenResultShouldNotBeNull()
        {
            var secret = "3b4a872617be2ae1932baa1d4d240272";
            var cookieValue = "access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026";

            Assert.NotNull(FacebookSession.ParseCookieValue(new DefaultFacebookApplication { AppSecret = secret }, cookieValue));
        }

        [Fact(DisplayName = "ParseCookieValue: Given valid fbs_ cookie value Then should extract the access token correctly")]
        public void ParseCookieValue_GivenValidFbsCookieValue_ThenShouldExtractTheAccessTokenCorrectly()
        {
            var secret = "3b4a872617be2ae1932baa1d4d240272";
            var cookieValue = "access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026";
            var expectedAccessToken = "124973200873702|2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026|vz4H9xjlRZPfg2quCv0XOM5g9_o";

            var session = FacebookSession.ParseCookieValue(new DefaultFacebookApplication { AppSecret = secret }, cookieValue);

            Assert.Equal(expectedAccessToken, session.AccessToken);
        }

        [Fact(DisplayName = "ParseCookieValue: Given valid fbs_ cookie value Then should extract the user id correctly")]
        public void ParseCookieValue_GivenValidFbsCookieValue_ThenShouldExtractTheUserIdCorrectly()
        {
            var secret = "3b4a872617be2ae1932baa1d4d240272";
            var cookieValue = "access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026";
            long expectedUserId = 100001327642026;

            var session = FacebookSession.ParseCookieValue(new DefaultFacebookApplication { AppSecret = secret }, cookieValue);

            Assert.Equal(expectedUserId, session.UserId);
        }

        [Fact(DisplayName = "ParseCookieValue: Given valid fbs_ cookie value Then should extract the secret correctly")]
        public void ParseCookieValue_GivenValidFbsCookieValue_ThenShouldExtractTheSecretCorrectly()
        {
            var secret = "3b4a872617be2ae1932baa1d4d240272";
            var cookieValue = "access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026";
            var expectedSecret = "lddpssZCuPoEtjcDFcWtoA__";

            var session = FacebookSession.ParseCookieValue(new DefaultFacebookApplication { AppSecret = secret }, cookieValue);

            Assert.Equal(expectedSecret, session.Secret);
        }

        [Fact(DisplayName = "ParseCookieValue: Given valid fbs_ cookie value Then should extract the session key correctly")]
        public void ParseCookieValue_GivenValidFbsCookieValue_ThenShouldExtractTheSessionKeyCorrectly()
        {
            var secret = "3b4a872617be2ae1932baa1d4d240272";
            var cookieValue = "access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026";
            var expectedSessionKey = "2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026";

            var session = FacebookSession.ParseCookieValue(new DefaultFacebookApplication { AppSecret = secret }, cookieValue);

            Assert.Equal(expectedSessionKey, session.SessionKey);
        }

        [Fact(DisplayName = "ParseCookieValue: Given valid fbs_ cookie value Then should extract the signature correctly")]
        public void ParseCookieValue_GivenValidFbs_CookieValue_ThenShouldExtractTheSignatureCorrectly()
        {
            var secret = "3b4a872617be2ae1932baa1d4d240272";
            var cookieValue = "access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026";
            var expectedSignature = "1d95fa4b3dfa5b26c01c8ac8676d80b8";

            var session = FacebookSession.ParseCookieValue(new DefaultFacebookApplication { AppSecret = secret }, cookieValue);

            Assert.Equal(expectedSignature, session.Signature);
        }
    }
}