using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Facebook.Web
{
    public class FacebookWebAuthorizer
    {

        private FacebookWebRequest m_request;

        public FacebookWebAuthorizer() :
            this(new FacebookWebRequest())
        {
        }

        public FacebookWebAuthorizer(FacebookWebRequest request)
        {
            this.m_request = request;
        }

        public FacebookWebAuthorizer(IFacebookApplication settings, HttpContextBase httpContext)
        {
            this.m_request = new FacebookWebRequest(settings, httpContext);
        }

        public FacebookWebRequest FacebookWebRequest
        {
            get { return this.m_request; }
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


        public bool Authorize()
        {
            return this.Authorize(null);
        }

        /// <summary>
        /// Authorizes the user if the user is not logged in or the application does not have all the sepcified permissions.
        /// </summary>
        /// <returns>
        /// Return true if the user is authenticated and the application has all the specified permissions.
        /// </returns>
        public bool Authorize(params string[] permissions)
        {
            var isAuthorized = m_request.IsAuthorized(permissions);

            if (!isAuthorized)
            {
                this.HandleUnauthorizedRequest();
            }

            return isAuthorized;
        }

        /// <summary>
        /// Handle unauthorized requests.
        /// </summary>
        public virtual void HandleUnauthorizedRequest()
        {
            this.m_request.HttpContext.Response.Redirect(this.CancelUrlPath);
        }

    }
}
