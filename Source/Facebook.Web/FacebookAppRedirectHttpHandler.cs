﻿// --------------------------------
// <copyright file="FacebookAppRedirectHttpHandler.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
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
            Contract.Requires(context != null);

            var html = "<html><head><meta http-equiv=\"refresh\" content=\"0;url="
                       + this.GetUrl(new HttpContextWrapper(context)) + "\"></head></html>";

            context.Response.Write(html);
        }

        protected Uri GetUrl(HttpContextBase context)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.Request != null);

            // TODO: need unit tests for this method, might as well need to refactor this method.
            var uri = new Uri("http://apps.facebook.com/");
            var redirectUriBuilder = new UriBuilder(uri);

            if (context.Request.QueryString.AllKeys.Contains("state"))
            {
                var state = Encoding.UTF8.GetString(FacebookUtils.Base64UrlDecode(context.Request.QueryString["state"]));
                var json = (IDictionary<string, object>)JsonSerializer.Current.DeserializeObject(state);

                // make it one letter character so more info can fit in.
                // r -> return_url_path
                // c -> cancel_url_path
                // s -> user_state
                // n -> navigate url path.
                var returnUrlPath = json["r"].ToString();

                if (context.Request.QueryString.AllKeys.Contains("error_reason"))
                {
                    // TODO: might be good to append return_url_path
                    if (json.ContainsKey("c"))
                    {
                        // if there is cancel url path.
                        var cancelUrlPath = json["c"].ToString();

                        IDictionary<string, object> cancelUrlQueryStrings = new Dictionary<string, object>
                                                       {
                                                           { "error_reason", context.Request.QueryString["error_reason"] },
                                                           { "error", context.Request.QueryString["error"] },
                                                           { "error_description", context.Request.QueryString["error_description"] }
                                                       };

                        redirectUriBuilder.Path = json["c"].ToString();

                        if (cancelUrlPath.Contains("?"))
                        {
                            // incase cancel url path contains querystrings.
                            var cancelUrlParts = cancelUrlPath.Split('?');
                            if (cancelUrlParts.Length == 2 && !string.IsNullOrEmpty(cancelUrlParts[1]))
                            {
                                redirectUriBuilder.Path = cancelUrlParts[0];
                                var queryStrings = FacebookUtils.ParseUrlQueryString(cancelUrlParts[1]);
                                cancelUrlQueryStrings = FacebookUtils.Merge(cancelUrlQueryStrings, queryStrings);
                            }
                        }

                        redirectUriBuilder.Query = FacebookUtils.ToJsonQueryString(cancelUrlQueryStrings);
                    }
                    else
                    {
                        // if there is no cancel url path.
                        redirectUriBuilder = new UriBuilder("http://www.facebook.com");
                    }
                }
                else
                {
                    if (returnUrlPath.Contains("?"))
                    {
                        var parts = returnUrlPath.Split('?');
                        redirectUriBuilder.Path = parts[0];

                        redirectUriBuilder.Query = parts[1];
                        var rQueryString = FacebookUtils.ParseUrlQueryString(parts[1]);
                        if (rQueryString.ContainsKey("error_reason"))
                        {
                            // remove oauth stuffs.
                            if (rQueryString.ContainsKey("error_reason"))
                            {
                                rQueryString.Remove("error_reason");
                            }

                            if (rQueryString.ContainsKey("error"))
                            {
                                rQueryString.Remove("error");
                            }

                            if (rQueryString.ContainsKey("error_description"))
                            {
                                rQueryString.Remove("error_description");
                            }
                            redirectUriBuilder.Query = FacebookUtils.ToJsonQueryString(rQueryString);
                        }
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

            return redirectUriBuilder.Uri;
        }
    }
}
