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

            var uri = new Uri("http://apps.facebook.com/");
            var redirectUriBuilder = new UriBuilder(uri) { Query = queryString };

            if (context.Request.QueryString.AllKeys.Contains("state"))
            {
                var state = Encoding.UTF8.GetString(FacebookUtils.Base64UrlDecode(context.Request.QueryString["state"]));
                var json = (IDictionary<string, object>)JsonSerializer.DeserializeObject(state);

                // make it one letter character so more info can fit in.
                // r -> return_url_path
                // c -> cancel_url_path
                // s -> user_state
                var returnUrlPath = json["r"].ToString();

                if (context.Request.QueryString.AllKeys.Contains("error_reason"))
                {
                    // TODO: might be good to append return_url_path
                    redirectUriBuilder.Path = json["c"].ToString();
                    redirectUriBuilder.Query = string.Format(
                        "error_reason={0}&error_denied={1}&error_description={2}",
                        context.Request.QueryString["error_reason"],
                        context.Request.QueryString["error"],
                        FacebookUtils.UrlEncode(context.Request.QueryString["error_description"]));
                }
                else
                {
                    if (returnUrlPath.Contains("?"))
                    {
                        var parts = returnUrlPath.Split('?');
                        redirectUriBuilder.Path = parts[0];
                        redirectUriBuilder.Query = parts[1];
                    }
                    else
                    {
                        redirectUriBuilder.Path = returnUrlPath;
                    }
                }
            }
            else
            {
                // if not state was given.
                redirectUriBuilder = new UriBuilder("http://www.facebook.com");
            }

            var html = "<html><head><meta http-equiv=\"refresh\" content=\"0;url="
                       + redirectUriBuilder.Uri + "\"></head></html>";
            context.Response.Write(html);
        }
    }
}
