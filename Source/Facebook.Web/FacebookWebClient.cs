namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class FacebookWebClient : FacebookClient
    {
        private FacebookWebContext _request;

        private bool _isSecureConnection;

        public FacebookWebClient()
            : this(FacebookWebContext.Current)
        {
        }

        public FacebookWebClient(FacebookWebContext request)
        {
            AccessToken = request.AccessToken;

            _request = request;
            _isSecureConnection = request.IsSecureConnection;

            UseFacebookBeta = _request.Settings.UseFacebookBeta;

            if (request.HttpContext.Request.UrlReferrer != null && _request.HttpContext.Request.UrlReferrer.Host == "apps.beta.facebook.com")
            {
                UseFacebookBeta = true;
            }
        }

        internal protected override object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            try
            {
                return base.Api(path, AddReturnSslResourceIfRequired(parameters, IsSecureConnection), httpMethod, resultType);
            }
            catch (FacebookOAuthException)
            {
                try
                {
                    _request.DeleteAuthCookie();
                }
                catch { }
                throw;
            }
        }

        protected internal override void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, object userToken)
        {
            base.ApiAsync(path, AddReturnSslResourceIfRequired(parameters, IsSecureConnection), httpMethod, userToken);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scheme is secure.
        /// </summary>
        public bool IsSecureConnection
        {
            get { return _isSecureConnection; }
            set { _isSecureConnection = value; }
        }

        internal static IDictionary<string, object> AddReturnSslResourceIfRequired(IDictionary<string, object> parameters, bool isSecuredConnection)
        {
            Contract.Ensures(Contract.Result<IDictionary<string, object>>() != null);

            var mergedParameters = FacebookUtils.Merge(null, parameters);

            if (isSecuredConnection && !mergedParameters.ContainsKey(Facebook.Web.Properties.Resources.return_ssl_resources))
            {
                mergedParameters[Facebook.Web.Properties.Resources.return_ssl_resources] = true;
            }

            return mergedParameters;
        }
    }
}
