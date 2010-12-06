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

        public void Authorize()
        {
            Authorize(HttpContext.Current.Request, HttpContext.Current.Response);
        }

        public virtual void Authorize(HttpRequestBase request, HttpResponseBase response)
        {
            if (!this.IsAuthorized())
            {
                var url = GetLoginUrl(request);
                response.ContentType = "text/html";
                response.Write(CanvasUrlBuilder.GetCanvasRedirectHtml(url));
            }
        }

        public virtual void Authorize(HttpRequest request, HttpResponse response)
        {
            var requestWrapper = new HttpRequestWrapper(request);
            var responseWrapper = new HttpResponseWrapper(response);
            Authorize(requestWrapper, responseWrapper);
        }

        public virtual Uri GetLoginUrl(HttpRequestBase request)
        {
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(request, canvasSettings);
            return urlBuilder.GetLoginUrl(this.FacebookApp, Perms, ReturnUrlPath, CancelUrlPath);
        }

    }
}
