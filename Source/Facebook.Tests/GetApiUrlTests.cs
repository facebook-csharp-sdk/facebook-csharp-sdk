using System;
using System.Collections.Generic;

namespace Facebook.Tests
{
    using Xunit;

    public class GetApiUrlTests
    {
        [Fact(DisplayName = "GetApiUrl: When method is video.upload The uri should start with api-video facebook domain")]
        public void GetApiUrl_WhenMethodIsVideoUpload_TheUriShouldStartWithApiVideoFacebookDomain()
        {
            var fb = new FakeFacebookApp();

            var uri = fb.GetApiUrl("video.upload");

            Assert.Equal("https://api-video.facebook.com/restserver.php",uri.AbsoluteUri);
        }

        class FakeFacebookApp : FacebookAppBase
        {
            #region not implemented
            public override Uri GetLoginUrl(IDictionary<string, object> parameters)
            {
                throw new NotImplementedException();
            }

            public override Uri GetLogoutUrl(IDictionary<string, object> parameters)
            {
                throw new NotImplementedException();
            }

            public override Uri GetLoginStatusUrl(IDictionary<string, object> parameters)
            {
                throw new NotImplementedException();
            }

            protected override void ValidateSessionObject(FacebookSession session)
            {
                throw new NotImplementedException();
            }

            protected override string GenerateSignature(FacebookSession session)
            {
                throw new NotImplementedException();
            }

            protected override object RestServer(IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
            {
                throw new NotImplementedException();
            }

            protected override object Graph(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
            {
                throw new NotImplementedException();
            }

            protected override object OAuthRequest(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, bool restApi)
            {
                throw new NotImplementedException();
            }

            protected override void RestServerAsync(IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, FacebookAsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }

            protected override void GraphAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, FacebookAsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }

            protected override void OAuthRequestAsync(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, bool restApi, FacebookAsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }
            #endregion

            public new System.Uri GetApiUrl(string method)
            {
                return base.GetApiUrl(method);
            }
        }
    }
}