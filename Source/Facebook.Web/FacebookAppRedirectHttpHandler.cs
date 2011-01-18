namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

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
            string pathInfo = context.Request.PathInfo;

            if (pathInfo.StartsWith("/cancel", StringComparison.Ordinal))
            {
                queryString = "cancel=1";
                pathInfo = pathInfo.Replace("/cancel", string.Empty);
            }

            var uri = new Uri("http://apps.facebook.com/");
            var uriBuilder = new UriBuilder(uri) { Query = queryString };


            if (context.Request.QueryString.AllKeys.Contains("state"))
            {
                var state = Encoding.UTF8.GetString(FacebookUtils.Base64UrlDecode(context.Request.QueryString["state"]));
                var json = (IDictionary<string, object>)JsonSerializer.DeserializeObject(state);

                var returnPathAndQuery = json["return_path"].ToString();
                if (returnPathAndQuery.Contains("?"))
                {
                    var parts = returnPathAndQuery.Split('?');
                    uriBuilder.Path = parts[0];
                    uriBuilder.Query = parts[1];
                }
                else
                {
                    uriBuilder.Path = returnPathAndQuery;
                }
            }

            var html = "<html><head><meta http-equiv=\"refresh\" content=\"0;url="
                       + uriBuilder.Uri + "\"></head></html>";
            context.Response.Write(html);
        }
    }
}
