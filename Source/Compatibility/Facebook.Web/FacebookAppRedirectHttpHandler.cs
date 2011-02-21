using System;
using System.Web;

namespace Facebook.Web
{
    /// <summary>
    /// Represents the redirector used after a Facebook canvas authorization.
    /// </summary>
    public class FacebookAppRedirectHttpHandler : IHttpHandler
    {
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            string queryString = string.Empty;
            string pathInfo =context.Request.PathInfo;
            if (pathInfo.StartsWith("/cancel", StringComparison.Ordinal))
            {
                queryString = "cancel=1";
                pathInfo = pathInfo.Replace("/cancel", string.Empty);
            }
            var uri = new Uri("http://apps.facebook.com" + pathInfo);
            UriBuilder uriBuilder = new UriBuilder(uri);
            uriBuilder.Query = queryString;
            var html = "<html><head><meta http-equiv=\"refresh\" content=\"0;url=" 
                + uriBuilder.Uri.ToString() + 
                "\"></head></html>";
            context.Response.Write(html);
        }
    }
}
