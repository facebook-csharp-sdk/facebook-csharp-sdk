// --------------------------------
// <copyright file="FacebookWebExtensions.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Facebook web extension methods.
    /// </summary>
    public static class FacebookWebExtensions
    {
        #region GetFacebookSignedRequest

        /// <summary>
        /// Gets the facebook signed request.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <returns>
        /// The facebook signed request.
        /// </returns>
        public static FacebookSignedRequest GetFacebookSignedRequest(this HttpRequestBase httpRequest, string secret)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);
            Contract.Requires(!string.IsNullOrEmpty(secret));

            return httpRequest.Params.AllKeys.Contains("signed_request") && !string.IsNullOrEmpty(httpRequest.Params["signed_request"]) && httpRequest.Params["signed_request"].Contains(".")
                       ? FacebookSignedRequest.Parse(secret, httpRequest.Params["signed_request"])
                       : null;
        }

        /// <summary>
        /// Gets the facebook signed request.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <returns>
        /// The facebook signed request.
        /// </returns>
        public static FacebookSignedRequest GetFacebookSignedRequest(this HttpRequestBase httpRequest, IFacebookApplication facebookApplication)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);
            Contract.Requires(facebookApplication != null);
            Contract.Requires(!string.IsNullOrEmpty(facebookApplication.AppSecret));

            return GetFacebookSignedRequest(httpRequest, facebookApplication.AppSecret);
        }

        /// <summary>
        /// Gets the facebook signed request.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <returns>
        /// The facebook signed request.
        /// </returns>
        public static FacebookSignedRequest GetFacebookSignedRequest(this HttpRequestBase httpRequest)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);
            Contract.Requires(FacebookContext.Current != null);
            Contract.Requires(!string.IsNullOrEmpty(FacebookContext.Current.AppSecret));

            return GetFacebookSignedRequest(httpRequest, FacebookContext.Current);
        }

        /// <summary>
        /// Gets the facebook signed request.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <param name="secret">
        /// The secret.
        /// </param>
        /// <returns>
        /// The facebook signed request.
        /// </returns>
        public static FacebookSignedRequest GetFacebookSignedRequest(this HttpRequest httpRequest, string secret)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);
            Contract.Requires(!string.IsNullOrEmpty(secret));

            return GetFacebookSignedRequest(new HttpRequestWrapper(httpRequest), secret);
        }

        /// <summary>
        /// Gets the facebook signed request.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <returns>
        /// The facebook signed request.
        /// </returns>
        public static FacebookSignedRequest GetFacebookSignedRequest(this HttpRequest httpRequest, IFacebookApplication facebookApplication)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);
            Contract.Requires(facebookApplication != null);
            Contract.Requires(!string.IsNullOrEmpty(facebookApplication.AppSecret));

            return GetFacebookSignedRequest(httpRequest, facebookApplication.AppSecret);
        }

        /// <summary>
        /// Gets the facebook signed request.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <returns>
        /// The facebook signed request.
        /// </returns>
        public static FacebookSignedRequest GetFacebookSignedRequest(this HttpRequest httpRequest)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);
            Contract.Requires(FacebookContext.Current != null);
            Contract.Requires(!string.IsNullOrEmpty(FacebookContext.Current.AppSecret));

            return GetFacebookSignedRequest(httpRequest, FacebookContext.Current);
        }

        #endregion

        #region GetFacebookSession

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <returns>
        /// The facebook session.
        /// </returns>
        public static FacebookSession GetFacebookSession(this HttpRequestBase httpRequest, string appId, string appSecret)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(!string.IsNullOrEmpty(appSecret));
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);

            return FacebookWebUtils.GetSession(appId, appSecret, httpRequest);
        }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <returns>
        /// The facebook session.
        /// </returns>
        public static FacebookSession GetFacebookSession(this HttpRequestBase httpRequest, IFacebookApplication facebookApplication)
        {
            Contract.Requires(facebookApplication != null);
            Contract.Requires(!string.IsNullOrEmpty(facebookApplication.AppId));
            Contract.Requires(!string.IsNullOrEmpty(facebookApplication.AppSecret));
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);

            return GetFacebookSession(httpRequest, facebookApplication.AppId, facebookApplication.AppSecret);
        }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <returns>
        /// The facebook session.
        /// </returns>
        public static FacebookSession GetFacebookSession(this HttpRequestBase httpRequest)
        {
            Contract.Requires(FacebookContext.Current != null);
            Contract.Requires(!string.IsNullOrEmpty(FacebookContext.Current.AppId));
            Contract.Requires(!string.IsNullOrEmpty(FacebookContext.Current.AppSecret));
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);

            return GetFacebookSession(httpRequest, FacebookContext.Current);
        }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <returns>
        /// The facebook session.
        /// </returns>
        public static FacebookSession GetFacebookSession(this HttpRequest httpRequest, string appId, string appSecret)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(!string.IsNullOrEmpty(appSecret));

            return GetFacebookSession(new HttpRequestWrapper(httpRequest), appId, appSecret);
        }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        /// <returns>
        /// The facebook session.
        /// </returns>
        public static FacebookSession GetFacebookSession(this HttpRequest httpRequest, IFacebookApplication facebookApplication)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);
            Contract.Requires(!string.IsNullOrEmpty(facebookApplication.AppId));
            Contract.Requires(!string.IsNullOrEmpty(facebookApplication.AppSecret));

            return GetFacebookSession(httpRequest, facebookApplication.AppId, facebookApplication.AppSecret);
        }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <returns>
        /// The facebook session.
        /// </returns>
        public static FacebookSession GetFacebookSession(this HttpRequest httpRequest)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);
            Contract.Requires(FacebookContext.Current != null);
            Contract.Requires(!string.IsNullOrEmpty(FacebookContext.Current.AppId));
            Contract.Requires(!string.IsNullOrEmpty(FacebookContext.Current.AppSecret));

            return GetFacebookSession(httpRequest, FacebookContext.Current);
        }

        #endregion
    }
}