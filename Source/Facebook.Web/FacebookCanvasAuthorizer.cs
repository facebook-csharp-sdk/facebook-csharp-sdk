namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web;

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
            FacebookWebRequest.HttpContext.Response.ContentType = "text/html";
            FacebookWebRequest.HttpContext.Response.Write(CanvasUrlBuilder.GetCanvasRedirectHtml(GetLoginUrl(null)));
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

            if (!string.IsNullOrEmpty(LoginDisplayMode))
            {
                defaultParameters["display"] = LoginDisplayMode;
            }

            if (Permissions != null)
            {
                defaultParameters["scope"] = String.Join(",", Permissions);
            }

            var canvasUrlBuilder = new CanvasUrlBuilder(FacebookWebRequest.Settings, FacebookWebRequest.HttpContext.Request);
            return canvasUrlBuilder.GetLoginUrl(ReturnUrlPath, CancelUrlPath, State, FacebookUtils.Merge(defaultParameters, parameters));
        }

    }
}
