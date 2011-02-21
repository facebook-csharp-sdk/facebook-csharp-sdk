using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Diagnostics.Contracts;

namespace Facebook.Web
{
    public class CanvasAuthorizer : Authorizer
    {

        private ICanvasSettings canvasSettings;

        public CanvasAuthorizer(FacebookAppBase facebookApp)
            : base(facebookApp)
        {
            this.canvasSettings = CanvasSettings.Current;
        }

        public CanvasAuthorizer(FacebookAppBase facebookApp, ICanvasSettings canvasSettings)
            : base(facebookApp)
        {
            Contract.Requires(canvasSettings != null);

            this.canvasSettings = canvasSettings;
        }

        public override void HandleUnauthorizedRequest(HttpContextBase httpContext)
        {
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(httpContext.Request, canvasSettings);
            var url = urlBuilder.GetLoginUrl(this.FacebookApp, Perms, ReturnUrlPath, CancelUrlPath);
            httpContext.Response.ContentType = "text/html";
            httpContext.Response.Write(CanvasUrlBuilder.GetCanvasRedirectHtml(url));
        }

    }
}
