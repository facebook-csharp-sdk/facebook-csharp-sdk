// --------------------------------
// <copyright file="IFacebookApp.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using Facebook.Utilities;

namespace Facebook
{
    public abstract class FacebookAppBase
    {
        private static Collection<string> _dropQueryParameters = new Collection<string> {
            "session",
            "signed_request",
        };

        private static Dictionary<string, Uri> _domainMaps = new Dictionary<string, Uri> {
            { "api", new Uri("https://api.facebook.com/") },
            { "api_read", new Uri("https://api-read.facebook.com/") },
            { "graph", new Uri("https://graph.facebook.com/") },
            { "www", new Uri("https://www.facebook.com/") }
        };

        private static string[] _readOnlyCalls = new string[] {
            "admin.getallocation",
            "admin.getappproperties",
            "admin.getbannedusers",
            "admin.getlivestreamvialink",
            "admin.getmetrics",
            "admin.getrestrictioninfo",
            "application.getpublicinfo",
            "auth.getapppublickey",
            "auth.getsession",
            "auth.getsignedpublicsessiondata",
            "comments.get",
            "connect.getunconnectedfriendscount",
            "dashboard.getactivity",
            "dashboard.getcount",
            "dashboard.getglobalnews",
            "dashboard.getnews",
            "dashboard.multigetcount",
            "dashboard.multigetnews",
            "data.getcookies",
            "events.get",
            "events.getmembers",
            "fbml.getcustomtags",
            "feed.getappfriendstories",
            "feed.getregisteredtemplatebundlebyid",
            "feed.getregisteredtemplatebundles",
            "fql.multiquery",
            "fql.query",
            "friends.arefriends",
            "friends.get",
            "friends.getappusers",
            "friends.getlists",
            "friends.getmutualfriends",
            "gifts.get",
            "groups.get",
            "groups.getmembers",
            "intl.gettranslations",
            "links.get",
            "notes.get",
            "notifications.get",
            "pages.getinfo",
            "pages.isadmin",
            "pages.isappadded",
            "pages.isfan",
            "permissions.checkavailableapiaccess",
            "permissions.checkgrantedapiaccess",
            "photos.get",
            "photos.getalbums",
            "photos.gettags",
            "profile.getinfo",
            "profile.getinfooptions",
            "stream.get",
            "stream.getcomments",
            "stream.getfilters",
            "users.getinfo",
            "users.getloggedinuser",
            "users.getstandardinfo",
            "users.hasapppermission",
            "users.isappuser",
            "users.isverified",
            "video.getuploadlimits" 
        };

        /// <summary>
        /// List of query parameters that get automatically dropped when rebuilding the current URL.
        /// </summary>
        protected virtual ICollection<string> DropQueryParameters
        {
            get { return _dropQueryParameters; }
        }

        /// <summary>
        /// Maps aliases to Facebook domains.
        /// </summary>
        public virtual Dictionary<string, Uri> DomainMaps
        {
            get { return _domainMaps; }
        }

        /// <summary>
        /// Gets the Application ID.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets the Application API Secret.
        /// </summary>
        public string ApiSecret { get; set; }

        /// <summary>
        /// The active user session, if one is available.
        /// </summary>
        public virtual FacebookSession Session { get; set; }

        /// <summary>
        /// Gets or sets the Cookie Support status.
        /// </summary>
        public bool CookieSupport { get; set; }

        /// <summary>
        /// Indicates that we already loaded the session as best as we could.
        /// </summary>
        public bool SessionLoaded { get; set; }

        /// <summary>
        /// Gets or sets the base domain of the the application.
        /// </summary>
        public string BaseDomain { get; set; }

        /// <summary>
        /// Gets the default Facebook login success url.
        /// </summary>
        protected virtual Uri CurrentUrl
        {
            get
            {
                return new Uri("http://www.facebook.com/connect/login_success.html");
            }
        }

        /// <summary>
        /// Get the UID from the session.
        /// </summary>
        public long UserId
        {
            get
            {
                if (this.Session == null)
                {
                    return 0;
                }
                return this.Session.UserId;
            }
        }

        /// <summary>
        /// Gets a OAuth access token.
        /// </summary>
        public string AccessToken
        {
            get
            {
                // either user session signed, or app signed
                if (this.Session != null)
                {
                    return this.Session.AccessToken;
                }
                else
                {
                    return string.Concat(this.AppId, "|", this.ApiSecret);
                }
            }
        }

        /// <summary>
        /// Gets the name of the current session cookie.
        /// </summary>
        protected string SessionCookieName
        {
            get
            {
                return string.Concat("fbs_", this.AppId);
            }
        }

        /// <summary>
        /// Get a Login URL for use with redirects. By default, 
        /// full page redirect is assumed.
        /// </summary>
        /// <returns>The URL for the login flow.</returns>
        public Uri GetLoginUrl()
        {
            return GetLoginUrl(null);
        }

        /// <summary>
        /// Get a Login URL for use with redirects. By default, full page redirect is
        /// assumed. If you are using the generated URL with a window.open() call in
        /// JavaScript, you can pass in display=popup as part of the parameters.
        /// The parameters:
        ///     - next: the url to go to after a successful login
        ///     - cancel_url: the url to go to after the user cancels
        ///     - req_perms: comma separated list of requested extended perms
        ///     - display: can be "page" (default, full page) or "popup"
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public abstract Uri GetLoginUrl(IDictionary<string, object> parameters);

        /// <summary>
        /// Get a Logout URL suitable for use with redirects.
        /// </summary>
        /// <returns>The URL for the logout flow.</returns>
        public Uri GetLogoutUrl()
        {
            return GetLogoutUrl(null);
        }

        /// <summary>
        /// Get a Logout URL suitable for use with redirects.
        /// The parameters:
        ///     - next: the url to go to after a successful logout
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public abstract Uri GetLogoutUrl(IDictionary<string, object> parameters);

        /// <summary>
        /// Get a login status URL to fetch the status from facebook.
        /// </summary>
        /// <returns>The URL for the logout flow</returns>
        public Uri GetLoginStatusUrl()
        {
            return GetLoginStatusUrl(null);
        }

        /// <summary>
        /// Get a login status URL to fetch the status from facebook.
        /// The parameters:
        ///     - ok_session: the URL to go to if a session is found
        ///     - no_session: the URL to go to if the user is not connected
        ///     - no_user: the URL to go to if the user is not signed into facebook
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the logout flow</returns>
        public abstract Uri GetLoginStatusUrl(IDictionary<string, object> parameters);

        protected virtual Uri CleanUrl(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            Contract.EndContractBlock();

            UriBuilder builder = new UriBuilder(uri);
            if (!String.IsNullOrEmpty(uri.Query))
            {
                var querystring = new Dictionary<string, string>();
                var parts = uri.Query.Split('&');
                foreach (var part in parts)
                {
                    if (part != null)
                    {
                        var keyValuePair = part.Split('=');
                        if (keyValuePair.Length > 0)
                        {
                            string key = keyValuePair[0];
                            if (!DropQueryParameters.Contains(key))
                            {
                                string value = keyValuePair.Length > 1 ? keyValuePair[1] : null;
                                querystring.Add(key, value);
                            }
                        }
                    }
                }
                builder.Query = querystring.ToJsonQueryString();
            }
            return builder.Uri;
        }

        #region API Calls

#if (!SILVERLIGHT) // Silverlight should only have async calls
        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public dynamic Api(IDictionary<string, object> parameters)
        {
            return this.Api(null, parameters, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public dynamic Api(string path)
        {
            return this.Api(path, null, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="httpMethod">The http method for the request. Default is 'GET'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public dynamic Api(string path, HttpMethod httpMethod)
        {
            return this.Api(path, null, httpMethod);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public dynamic Api(IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            return this.Api(null, parameters, httpMethod);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public dynamic Api(string path, IDictionary<string, object> parameters)
        {
            return this.Api(path, parameters, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <param name="httpMethod">The http method for the request. Default is 'GET'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public virtual dynamic Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
            {
                throw new ArgumentException("You must supply either the path or parameters argument.");
            }
            if (parameters != null && !(parameters is IDictionary<string, object>))
            {
                throw new ArgumentException("The argument 'parameters' must impliment IDicationary<string, object>.");
            }
            Contract.EndContractBlock();

            parameters = parameters ?? new ExpandoObject();

            if (((IDictionary<string, object>)parameters).ContainsKey("method"))
            {
                return this.RestServer(parameters, httpMethod);
            }
            else
            {
                return this.Graph(path, parameters, httpMethod);
            }
        }

#endif
        #endregion

        #region Async API Calls
        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="parameters">JsonObject of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void ApiAsync(FacebookAsyncCallback callback, object state, IDictionary<string, object> parameters)
        {
            this.ApiAsync(callback, state, null, parameters, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void ApiAsync(FacebookAsyncCallback callback, object state, string path)
        {
            this.ApiAsync(callback, state, path, null, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="httpMethod">The http method for the request. Default is 'GET'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void ApiAsync(FacebookAsyncCallback callback, object state, string path, HttpMethod httpMethod)
        {
            this.ApiAsync(callback, state, path, null, httpMethod);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">JsonObject of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void ApiAsync(FacebookAsyncCallback callback, object state, string path, IDictionary<string, object> parameters)
        {
            this.ApiAsync(callback, state, path, parameters, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">JsonObject of url parameters.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <exception cref="Facebook.FacebookApiException" />
        public virtual void ApiAsync(FacebookAsyncCallback callback, object state, string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
            {
                throw new ArgumentException("You must supply either the 'path' or 'parameters' argument.");
            }
            if (parameters != null && !(parameters is IDictionary<string, object>))
            {
                throw new ArgumentException("The argument 'parameters' must impliment IDicationary<string, object>.");
            }
            Contract.EndContractBlock();

            parameters = parameters ?? new ExpandoObject();

            if (((IDictionary<string, object>)parameters).ContainsKey("method"))
            {
                this.RestServerAsync(callback, state, parameters, httpMethod);
            }
            else
            {
                this.GraphAsync(callback, state, path, parameters, httpMethod);
            }
        }

        #endregion

#if !SILVERLIGHT
        /// <summary>
        /// Validates a session_version=3 style session object.
        /// </summary>
        /// <param name="session">The session to validate.</param>
        /// <returns>True if valid, else false.</returns>
        protected abstract bool ValidateSessionObject(FacebookSession session);

        /// <summary>
        /// Generates a MD5 signature for the facebook session.
        /// </summary>
        /// <param name="session">The session to generate a signature.</param>
        /// <returns>An MD5 signature.</returns>
        protected abstract string GenerateSignature(FacebookSession session);


        /// <summary>
        /// Invoke the old restserver.php endpoint.
        /// </summary>
        /// <param name="parameters">The parameters for the server call.</param>
        /// <returns>The decoded response object.</returns>
        protected abstract dynamic RestServer(IDictionary<string, object> parameters, HttpMethod httpMethod);

        /// <summary>
        /// Invoke the Graph API.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <param name="parameters">JsonObject of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected abstract dynamic Graph(string path, IDictionary<string, object> parameters, HttpMethod httpMethod);

        /// <summary>
        /// Make a OAuth Request
        /// </summary>
        /// <param name="uri">The url to make the request.</param>
        /// <param name="parameters">The parameters of the request.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <returns>The decoded response object.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected abstract dynamic OAuthRequest(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod);
#endif

        /// <summary>
        /// Invoke the old restserver.php endpoint.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="parameters">The parameters for the server call.</param>
        protected abstract void RestServerAsync(FacebookAsyncCallback callback, object state, IDictionary<string, object> parameters, HttpMethod httpMethod);

        /// <summary>
        /// Invoke the Graph API.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <param name="parameters">JsonObject of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected abstract void GraphAsync(FacebookAsyncCallback callback, object state, string path, IDictionary<string, object> parameters, HttpMethod httpMethod);

        /// <summary>
        /// Make a OAuth Request
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param> 
        /// <param name="uri">The url to make the request.</param>
        /// <param name="parameters">The parameters of the request.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <returns>The decoded response object.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected abstract void OAuthRequestAsync(FacebookAsyncCallback callback, object state, Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod);

        /// <summary>
        /// Build the URL for api given parameters.
        /// </summary>
        /// <param name="method">The method name.</param>
        /// <returns>The Url for the given parameters.</returns>
        protected virtual Uri GetApiUrl(string method)
        {
            string name = "api";
            if (_readOnlyCalls.Contains(method))
            {
                name = "api_read";
            }
            return this.GetUrl(name, "restserver.php");
        }

        /// <summary>
        /// Build the URL for given domain alias, path and parameters.
        /// </summary>
        /// <param name="name">The name of the domain (from the domain maps).</param>
        /// <returns>The string of the url for the given parameters.</returns>
        protected Uri GetUrl(string name)
        {
            return this.GetUrl(name, string.Empty, null);
        }

        /// <summary>
        /// Build the URL for given domain alias, path and parameters.
        /// </summary>
        /// <param name="name">The name of the domain (from the domain maps).</param>
        /// <param name="path">Path (without a leading slash)</param>
        /// <returns>The string of the url for the given parameters.</returns>
        protected Uri GetUrl(string name, string path)
        {
            return this.GetUrl(name, path, null);
        }

        /// <summary>
        /// Build the URL for given domain alias, path and parameters.
        /// </summary>
        /// <param name="name">The name of the domain (from the domain maps).</param>
        /// <param name="parameters">Optional query parameters</param>
        /// <returns>The string of the url for the given parameters.</returns>
        protected Uri GetUrl(string name, IDictionary<string, object> parameters)
        {
            return this.GetUrl(name, string.Empty, parameters);
        }

        /// <summary>
        /// Build the URL for given domain alias, path and parameters.
        /// </summary>
        /// <param name="name">The name of the domain (from the domain maps).</param>
        /// <param name="path">Optional path (without a leading slash)</param>
        /// <param name="parameters">Optional query parameters</param>
        /// <returns>The string of the url for the given parameters.</returns>
        protected virtual Uri GetUrl(string name, string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (parameters != null && !(parameters is IDictionary<string, object>))
            {
                throw new ArgumentException("The argument 'parameters' must impliment IDicationary<string, object>.");
            }

            UriBuilder uri = new UriBuilder(_domainMaps[name]);
            if (!String.IsNullOrEmpty(path))
            {
                if (path[0] == '/')
                {
                    if (path.Length > 1)
                    {
                        path = path.Substring(1, path.Length - 1);
                    }
                    else
                    {
                        path = string.Empty;
                    }
                }
                uri.Path = path;
            }
            if (parameters != null)
            {
                uri.Query = ((IDictionary<string, object>)parameters).ToJsonQueryString();
            }
            return uri.Uri;
        }

    }
}