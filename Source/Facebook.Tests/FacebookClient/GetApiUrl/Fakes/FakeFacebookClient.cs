namespace Facebook.Tests.FacebookClient.GetApiUrl.Fakes
{
    using System;
    using System.Collections.Generic;
    using Facebook;

    class FakeFacebookClient : FacebookClient
    {
        #region not implemented

#if !SILVERLIGHT
        
        internal protected override object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            throw new NotImplementedException();
        }

#endif

        internal protected override void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, object userToken)
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