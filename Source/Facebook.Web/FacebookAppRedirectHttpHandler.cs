// --------------------------------
// <copyright file="FacebookAppRedirectHttpHandler.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

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
            if (context == null)
                throw new ArgumentNullException("context");

            var html = "<html><head><meta http-equiv=\"refresh\" content=\"0;url="
                       + GetUrl(new HttpContextWrapper(context)).AbsoluteUri + "\"></head></html>";

            context.Response.Write(html);
        }

        protected Uri GetUrl(HttpContextBase context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.Request == null)
                throw new Exception("context.Request is null");

            // TODO: need unit tests for this method, might as well need to refactor this method.
            UriBuilder redirectUriBuilder;

            if (!context.Request.QueryString.AllKeys.Contains("state"))
            {
                // todo: better to redirect to the default canvas page.
                return new Uri("http://www.facebook.com");
            }

            // if state is present.
            var state = Encoding.UTF8.GetString(FacebookWebUtils.Base64UrlDecode(context.Request.QueryString["state"]));
            var json = (IDictionary<string, object>)JsonSerializer.Current.DeserializeObject(state);

            // make it one letter character so more info can fit in.
            // r -> return_url_path (full uri)
            // c -> cancel_url_path (full uri)
            // s -> user_state
            FacebookOAuthResult oauthResult;
            if (!FacebookOAuthResult.TryParse(context.Request.Url, out oauthResult))
            {
                // todo: better to redirect to the default canvas page.
                return new Uri("http://www.facebook.com");
            }

            if (oauthResult.IsSuccess)
            {
                var returnUrl = json["r"].ToString();

                redirectUriBuilder = new UriBuilder(returnUrl);

                if (returnUrl.Contains("?"))
                {
                    // incase return url path contains querystrings.
                    var returnUrlParts = returnUrl.Split('?');
                    if (returnUrlParts.Length == 2 && !string.IsNullOrEmpty(returnUrlParts[1]))
                    {
                        var queryStrings = FacebookUtils.ParseUrlQueryString(returnUrlParts[1]);

                        if (queryStrings.ContainsKey("error_reason"))
                        {
                            // remove oauth stuffs.
                            if (queryStrings.ContainsKey("error_reason"))
                            {
                                queryStrings.Remove("error_reason");
                            }

                            if (queryStrings.ContainsKey("error"))
                            {
                                queryStrings.Remove("error");
                            }

                            if (queryStrings.ContainsKey("error_description"))
                            {
                                queryStrings.Remove("error_description");
                            }

                            redirectUriBuilder.Query = FacebookUtils.ToJsonQueryString(queryStrings);
                        }
                    }
                }
            }
            else
            {
                if (!json.ContainsKey("c"))
                {
                    // there is no cancel url path
                    redirectUriBuilder = new UriBuilder("http://facebook.com");
                }
                else
                {
                    var cancelUrl = json["c"].ToString();

                    IDictionary<string, object> cancelUrlQueryStrings = new Dictionary<string, object>
                                                                            {
                                                                                { "error_reason", context.Request.QueryString["error_reason"] },
                                                                                { "error", context.Request.QueryString["error"] },
                                                                                { "error_description", context.Request.QueryString["error_description"] }
                                                                            };

                    if (cancelUrl.Contains("?"))
                    {
                        // incase cancel url path contains querystrings.
                        var cancelUrlParts = cancelUrl.Split('?');
                        if (cancelUrlParts.Length == 2 && !string.IsNullOrEmpty(cancelUrlParts[1]))
                        {
                            var queryStrings = FacebookUtils.ParseUrlQueryString(cancelUrlParts[1]);
                            cancelUrlQueryStrings = FacebookUtils.Merge(cancelUrlQueryStrings, queryStrings);
                        }
                    }

                    redirectUriBuilder = new UriBuilder(cancelUrl)
                                             {
                                                 Query = FacebookUtils.ToJsonQueryString(cancelUrlQueryStrings)
                                             };
                }
            }

            return redirectUriBuilder.Uri;
        }

    }
}
