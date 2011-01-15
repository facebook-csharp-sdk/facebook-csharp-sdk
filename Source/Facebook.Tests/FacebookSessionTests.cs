namespace Facebook.Tests
{
    using System.Collections.Generic;
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
    }
}