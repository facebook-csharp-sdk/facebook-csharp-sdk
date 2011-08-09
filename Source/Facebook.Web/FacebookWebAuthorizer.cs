// --------------------------------
// <copyright file="FacebookWebAuthorizer.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System.Web;

    /// <summary>
    /// Represents the Facebook Web Authorizer
    /// </summary>
    public class FacebookWebAuthorizer
    {
        private readonly FacebookWebContext _request;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebAuthorizer"/> class.
        /// </summary>
        public FacebookWebAuthorizer() :
            this(FacebookWebContext.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebAuthorizer"/> class.
        /// </summary>
        /// <param name="context">The <see cref="FacebookWebContext"/></param>
        public FacebookWebAuthorizer(FacebookWebContext context)
        {
            _request = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebAuthorizer"/> class.
        /// </summary>
        /// <param name="settings">The Facebook application settings.</param>
        /// <param name="httpContext">The HttpContext.</param>
        public FacebookWebAuthorizer(IFacebookApplication settings, HttpContextBase httpContext)
        {
            _request = new FacebookWebContext(settings, httpContext);
        }

        /// <summary>
        /// The <see cref="FacebookWebContext"/>.
        /// </summary>
        public FacebookWebContext FacebookWebRequest
        {
            get { return _request; }
        }

        /// <summary>
        /// The Facebook session.
        /// </summary>
        [System.Obsolete("Use FacebookWebRequest.Session instead.")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FacebookSession Session
        {
            get { return FacebookWebRequest.Session; }
        }

        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        public string[] Permissions { get; set; }

        /// <summary>
        /// Gets or sets the return url path.
        /// </summary>
        public string ReturnUrlPath { get; set; }

        /// <summary>
        /// Gets or sets the cancel url path.
        /// </summary>
        public string CancelUrlPath { get; set; }

        /// <summary>
        /// Gets or sets the login display mode.
        /// </summary>
        public string LoginDisplayMode { get; set; }

        /// <summary>
        /// Gets or sets an opaque state used to maintain application state between the request and callback.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Authorizes the user if the user is not logged in or the application does not have all the sepcified permissions.
        /// </summary>
        /// <returns>
        /// Return true if the user is authenticated and the application has all the specified permissions.
        /// </returns>
        public bool Authorize()
        {
            var isAuthorized = _request.IsAuthorized(Permissions);

            if (!isAuthorized)
            {
                HandleUnauthorizedRequest();
            }

            return isAuthorized;
        }

        /// <summary>
        /// Handle unauthorized requests.
        /// </summary>
        public virtual void HandleUnauthorizedRequest()
        {
            _request.HttpContext.Response.Redirect(CancelUrlPath);
        }

    }
}
