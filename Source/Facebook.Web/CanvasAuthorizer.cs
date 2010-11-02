using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Facebook.Web
{
    public class CanvasAuthorizer : Authorizer
    {
        public CanvasAuthorizer(FacebookAppBase facebookApp)
            : base(facebookApp) { }

        public virtual void Authorize(HttpRequestBase request, HttpResponseBase response)
        {
            if (!this.IsAuthorized())
            {
                var url = GetLoginUrl(request);
                response.Redirect(url.ToString());
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
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(request);
            return urlBuilder.GetLoginUrl(this.FacebookApp, Perms, ReturnUrlPath, CancelUrlPath);
        }
    }
}
