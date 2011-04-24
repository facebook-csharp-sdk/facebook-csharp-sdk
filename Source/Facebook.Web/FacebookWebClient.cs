// --------------------------------
// <copyright file="FacebookWebClient.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class FacebookWebClient : FacebookClient
    {
        private FacebookWebContext _request;

        private bool _isSecureConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebClient"/> class.
        /// </summary>
        public FacebookWebClient()
            : this(FacebookWebContext.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebClient"/> class.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public FacebookWebClient(string accessToken)
            : base(accessToken)
        {
            Contract.Requires(!string.IsNullOrEmpty(accessToken));

            Initialize(FacebookWebContext.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebClient"/> class.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        public FacebookWebClient(string appId, string appSecret)
            : base(appId, appSecret)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(!string.IsNullOrEmpty(appSecret));

            Initialize(FacebookWebContext.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebClient"/> class.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        public FacebookWebClient(IFacebookApplication facebookApplication)
            : base(facebookApplication)
        {
            Initialize(FacebookWebContext.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebClient"/> class.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        public FacebookWebClient(FacebookWebContext request)
        {
            Contract.Requires(request != null);

            Initialize(request);
            AccessToken = request.AccessToken;
        }

        /// <summary>
        /// Initializes the FacebookWebClient from <see cref="FacebookWebContext"/>.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        private void Initialize(FacebookWebContext request)
        {
            Contract.Requires(request != null);

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
