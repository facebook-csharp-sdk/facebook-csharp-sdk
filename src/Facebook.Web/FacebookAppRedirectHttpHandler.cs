using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Facebook.Web
{
    public class FacebookAppRedirectHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string queryString = string.Empty;
            string pathInfo =context.Request.PathInfo;
            if (pathInfo.StartsWith("/cancel"))
            {
                queryString = "cancel=1";
                pathInfo = pathInfo.Replace("/cancel", string.Empty);
            }
            UriBuilder uri = new UriBuilder("http://apps.facebook.com" + pathInfo);
            uri.Query = queryString;
            context.Response.Redirect(uri.ToString());
        }
    }
}
