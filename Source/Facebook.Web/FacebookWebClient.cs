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

    /// <summary>
    /// Represents the Facebook Web Client.
    /// </summary>
    public class FacebookWebClient : FacebookClient
    {
        private FacebookWebContext _request;

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
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException("accessToken");

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
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException("appId");
            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException("appSecret");

            Initialize(FacebookWebContext.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebClient"/> class.
        /// </summary>
        /// <param name="facebookApplication">
        /// The Facebook application.
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
            if (request == null)
                throw new ArgumentNullException("request");

            _request = request;
            IsSecureConnection = _request.IsSecureConnection;

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
                return base.Api(path, parameters, httpMethod, resultType);
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

        protected override void OnGetCompleted(FacebookApiEventArgs args)
        {
            DeleteAuthCookieIfOAuthException(args);
            base.OnGetCompleted(args);
        }

        protected override void OnPostCompleted(FacebookApiEventArgs args)
        {
            DeleteAuthCookieIfOAuthException(args);
            base.OnPostCompleted(args);
        }

        protected override void OnDeleteCompleted(FacebookApiEventArgs args)
        {
            DeleteAuthCookieIfOAuthException(args);
            base.OnDeleteCompleted(args);
        }

        private void DeleteAuthCookieIfOAuthException(FacebookApiEventArgs args)
        {
            if (args.Error != null && args.Error is FacebookOAuthException)
            {
                try
                {
                    _request.DeleteAuthCookie();
                }
                catch { }
            }
        }
    }
}
