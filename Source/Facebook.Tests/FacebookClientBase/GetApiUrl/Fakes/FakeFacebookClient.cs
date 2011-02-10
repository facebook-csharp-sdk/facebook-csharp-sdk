namespace Facebook.Tests.FacebookClientBase.GetApiUrl.Fakes
{
    using System;
    using System.Collections.Generic;
    using Facebook;

    class FakeFacebookClient : FacebookClientBase
    {
        #region not implemented
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