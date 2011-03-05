namespace Facebook.Tests.FacebookClient.GetApiUrl.Fakes
{
    using System;
    using System.Collections.Generic;
    using Facebook;

    class FakeFacebookClient : FacebookClient
    {
        #region not implemented

        protected override ICollection<string> DropQueryParameters
        {
            get { throw new NotImplementedException(); }
        }

        internal protected override object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            throw new NotImplementedException();
        }

        internal protected override void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, object userToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        protected override Dictionary<string, Uri> DomainMaps
        {
            get { return base.DomainMaps; }
        }

        public new System.Uri GetApiUrl(string method)
        {
            return base.GetApiUrl(method);
        }
    }
}