using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Diagnostics.Contracts;
using System.ComponentModel;

namespace Facebook.Web
{
    [Obsolete("Use FacebookCanvasAuthorizer.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignTimeVisible(false)]
    [Browsable(false)]
    public class CanvasAuthorizer : Authorizer
    {

        public CanvasAuthorizer(FacebookAppBase facebookApp)
            : base(facebookApp)
        {
        }

        public CanvasAuthorizer(FacebookAppBase facebookApp, IFacebookApplication settings)
            : base(facebookApp)
        {
        }

        public override void HandleUnauthorizedRequest(HttpContextBase httpContext)
        {
            throw new NotImplementedException();
            //CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(httpContext.Request, canvasSettings);
            //var url = urlBuilder.GetLoginUrl(this.FacebookApp, Perms, ReturnUrlPath, CancelUrlPath);
            //httpContext.Response.ContentType = "text/html";
            //httpContext.Response.Write(CanvasUrlBuilder.GetCanvasRedirectHtml(url));
        }

    }
}
