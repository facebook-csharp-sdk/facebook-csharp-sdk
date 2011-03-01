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

        protected override Dictionary<string, Uri> DomainMaps
        {
            get { throw new NotImplementedException(); }
        }

        protected override object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            throw new NotImplementedException();
        }

        protected override void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, object userToken)
        {
            throw new NotImplementedException();
        }

        internal override Uri GetUrl(string name, string path, IDictionary<string, object> parameters)
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