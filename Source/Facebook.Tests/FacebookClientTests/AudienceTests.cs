using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Tests.FacebookClientTests
{
    using Facebook;
    using Xunit;

    public class AudienceTests
    {
        public readonly FacebookClient _fb = new FacebookClient();

        [Fact]
        public void TheUserAdditionToAudienceWorks()
        {
            var audienceId = 6025792497450;
            var accessToken = "your_access_token";
            var payload = @"{ 
                    'schema': 'EMAIL_SHA256', 
                    'data': [   'HASH1', 'HASH2' ] }";
            _fb.ForceMultipartFormData = true;

            var parameters = new Dictionary<string, object>();
            parameters["payload"] =  payload;
            _fb.AccessToken = accessToken;
            var result = _fb.Post(string.Format("{0}/users", audienceId), parameters);
            Assert.Equal(((dynamic)result).num_received, 2);
        }
    }
}
