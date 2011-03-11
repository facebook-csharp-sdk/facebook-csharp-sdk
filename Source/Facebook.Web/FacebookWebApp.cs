// --------------------------------
// <copyright file="FacebookWebApp.cs" company="Facebook C# SDK">
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
    /// Represents the core Facebook functionality for web applications.
    /// </summary>
    public class FacebookWebApp : FacebookApp
    {
        /// <summary>
        /// The current http context.
        /// </summary>
        private readonly HttpContextBase _httpContext;

        /// <summary>
        /// The current facebook session.
        /// </summary>
        private FacebookSession _session;

        /// <summary>
        /// The current facebook signed request.
        /// </summary>
        private FacebookSignedRequest _signedRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebApp"/> class.
        /// </summary>
        public FacebookWebApp()
            : this(new HttpContextWrapper(HttpContext.Current))
        {
            Contract.Requires(HttpContext.Current != null);
            Contract.Requires(HttpContext.Current.Request != null);
            Contract.Requires(HttpContext.Current.Response != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebApp"/> class.
        /// </summary>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        public FacebookWebApp(HttpContextBase httpContext)
        {
            Contract.Requires(httpContext != null);
            Contract.Requires(httpContext.Request != null);
            Contract.Requires(httpContext.Response != null);

            _httpContext = httpContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebApp"/> class.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public FacebookWebApp(string accessToken)
        {
            Contract.Requires(!string.IsNullOrEmpty(accessToken));
            AccessToken = accessToken;
        }

        /// <summary>
        /// Gets the signed request.
        /// </summary>
        public FacebookSignedRequest SignedRequest
        {
            get
            {
                if (_signedRequest == null && _httpContext != null && _httpContext.Request != null)
                {
                    if (_httpContext.Request.Params.AllKeys.Contains("signed_request"))
                    {
                        _signedRequest = FacebookSignedRequest.Parse(AppSecret, _httpContext.Request.Params["signed_request"]);
                    }
                }

                return _signedRequest;
            }
        }

        /// <summary>
        /// Gets or sets the active user session.
        /// </summary>
        public FacebookSession Session
        {
            get
            {
                if (_session == null && _httpContext != null)
                {
                    _session = FacebookSession.GetSession(AppId, AppSecret, _httpContext, SignedRequest);
                }

                return _session;
            }

            set
            {
                _session = value;
            }
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public new string AccessToken
        {
            get
            {
                if (Session != null)
                {
                    return Session.AccessToken;
                }
                else if (!string.IsNullOrEmpty(AppId) && string.IsNullOrEmpty(AppSecret))
                {
                    return string.Concat(AppId, "|", AppSecret);
                }

                return null;
            }

            set
            {
                Session = new FacebookSession(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current session is authenticated or not.
        /// </summary>
        public bool IsAuthenticated
        {
            get { return Session != null; }
        }
    }
}