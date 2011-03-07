using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Web;

namespace Facebook.Web
{
    public class FacebookCanvasAuthorizer : FacebookWebAuthorizer
    {

        public FacebookCanvasAuthorizer()
            : base(FacebookCanvasContext.Current)
        {
        }

        public FacebookCanvasAuthorizer(FacebookCanvasContext context)
            : base(context)
        {
        }

        public FacebookCanvasAuthorizer(IFacebookApplication settings, HttpContextBase httpContext)
            : base(settings, httpContext)
        {
        }

        /// <summary>
        /// Handle unauthorized requests.
        /// </summary>
        public override void HandleUnauthorizedRequest()
        {
            this.FacebookWebRequest.HttpContext.Response.ContentType = "text/html";
            this.FacebookWebRequest.HttpContext.Response.Write(CanvasUrlBuilder.GetCanvasRedirectHtml(this.GetLoginUrl(null)));
        }

        /// <summary>
        /// Gets the canvas login url.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the login url.
        /// </returns>
        public Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            var defaultParameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(this.LoginDisplayMode))
            {
                defaultParameters["display"] = this.LoginDisplayMode;
            }

            if (this.Permissions != null)
            {
                defaultParameters["scope"] = String.Join(",", this.Permissions);
            }

            var canvasUrlBuilder = new CanvasUrlBuilder(this.FacebookWebRequest.Settings, this.FacebookWebRequest.HttpContext.Request);
            return canvasUrlBuilder.GetLoginUrl(this.ReturnUrlPath, this.CancelUrlPath, this.State, FacebookUtils.Merge(defaultParameters, parameters));
        }

    }
}
