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
    using System.Linq;

    /// <summary>
    /// Represents the core Facebook functionality for web applications.
    /// </summary>
    public class FacebookWebApp : FacebookApp
    {
        /// <summary>
        /// The current http context.
        /// </summary>
        private readonly FacebookWebContext _request;

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
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public FacebookWebApp(FacebookWebContext request, string accessToken)
            : base(accessToken)
        {
            _request = request;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebApp"/> class.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public FacebookWebApp(string accessToken)
            : this(FacebookWebContext.Current, accessToken)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebApp"/> class.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        public FacebookWebApp(FacebookWebContext request)
            : this(request, request.AccessToken)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebApp"/> class.
        /// </summary>
        public FacebookWebApp()
            : this(FacebookWebContext.Current)
        {
        }

        /// <summary>
        /// Gets the signed request.
        /// </summary>
        public FacebookSignedRequest SignedRequest
        {
            get
            {
                if (_signedRequest == null)
                {
                    if (_request.HttpContext.Request.Params.AllKeys.Contains("signed_request"))
                    {
                        _signedRequest = FacebookSignedRequest.Parse(AppSecret, _request.HttpContext.Request.Params["signed_request"]);
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
            get { return _session ?? (_session = _request.Session); }
            set { _session = value; }
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

        protected internal override object Api(string path, System.Collections.Generic.IDictionary<string, object> parameters, HttpMethod httpMethod, System.Type resultType)
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
    }
}