// --------------------------------
// <copyright file="IFacebookApp.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Represents the core Facebook functionality.
    /// </summary>
    [ContractClass(typeof(FacebookAppBaseContracts))]
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
        /// The code contracts invarient object method.
        /// </summary>
        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code contracts invarient method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Code contracts invarient method.")]
        private void InvarientObject()
        {
            Contract.Invariant(_domainMaps != null);
            Contract.Invariant(_dropQueryParameters != null);
            Contract.Invariant(_readOnlyCalls != null);
        }

        /// <summary>
        /// Gets the list of query parameters that get automatically dropped when rebuilding the current URL.
        /// </summary>
        protected virtual ICollection<string> DropQueryParameters
        {
            get { return _dropQueryParameters; }
        }

        /// <summary>
        /// Gets the aliases to Facebook domains.
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
        /// Gets or sets the active user session.
        /// </summary>
        /// <value>The session.</value>
        public virtual FacebookSession Session { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether cookies are supported.
        /// </summary>
        /// <value><c>true</c> if cookies are supported; otherwise, <c>false</c>.</value>
        public bool CookieSupport { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the session has attempted to be loaded.
        /// </summary>
        /// <value><c>true</c> if [session loaded]; otherwise, <c>false</c>.</value>
        public bool SessionLoaded { get; set; }

        /// <summary>
        /// Gets or sets the base domain.
        /// </summary>
        /// <value>The base domain.</value>
        public string BaseDomain { get; set; }


        /// <summary>
        /// Gets the current URL.
        /// </summary>
        /// <value>The current URL.</value>
        protected virtual Uri CurrentUrl
        {
            get
            {
                return new Uri("http://www.facebook.com/connect/login_success.html");
            }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
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
        /// Gets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken
        {
            get
            {
                // either user session signed, or app signed
                if (this.Session != null)
                {
                    return this.Session.AccessToken;
                }
                else if (!String.IsNullOrEmpty(this.AppId) && !String.IsNullOrEmpty(this.ApiSecret))
                {
                    return string.Concat(this.AppId, "|", this.ApiSecret);
                }
                return null;
            }
        }


        /// <summary>
        /// Gets the name of the session cookie.
        /// </summary>
        /// <value>The name of the session cookie.</value>
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

        /// <summary>
        /// Cleans the URL or known Facebook querystring values.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        protected virtual Uri CleanUrl(Uri uri)
        {
            Contract.Requires(uri != null);

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
        public object Api(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            return this.Api(null, parameters, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public object Api(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return this.Api(path, null, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="httpMethod">The http method for the request. Default is 'GET'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public object Api(string path, HttpMethod httpMethod)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return this.Api(path, null, httpMethod);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <param name="httpMethod">The http method for the request. Default is 'GET'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public object Api(IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(parameters != null);

            return this.Api(null, parameters, httpMethod);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public object Api(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

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
        public virtual object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            parameters = parameters ?? new Dictionary<string, object>();

            path = ParseUrlParameters(path, parameters);

            if (parameters.ContainsKey("method"))
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
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void ApiAsync(FacebookAsyncCallback callback, object state, IDictionary<string, object> parameters)
        {
            Contract.Requires(callback != null);
            Contract.Requires(parameters != null);

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
            Contract.Requires(callback != null);
            Contract.Requires(!String.IsNullOrEmpty(path));

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
            Contract.Requires(callback != null);
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.ApiAsync(callback, state, path, null, httpMethod);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void ApiAsync(FacebookAsyncCallback callback, object state, string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(callback, state, path, parameters, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <exception cref="Facebook.FacebookApiException" />
        public virtual void ApiAsync(FacebookAsyncCallback callback, object state, string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            parameters = parameters ?? new Dictionary<string, object>();

            path = ParseUrlParameters(path, parameters);

            if (parameters.ContainsKey("method"))
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
        protected abstract void ValidateSessionObject(FacebookSession session);

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
        /// <param name="httpMethod">The http method for the request.</param>
        /// <returns>The decoded response object.</returns>
        protected abstract object RestServer(IDictionary<string, object> parameters, HttpMethod httpMethod);

        /// <summary>
        /// Invoke the Graph API.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected abstract object Graph(string path, IDictionary<string, object> parameters, HttpMethod httpMethod);

        /// <summary>
        /// Make a OAuth Request
        /// </summary>
        /// <param name="uri">The url to make the request.</param>
        /// <param name="parameters">The parameters of the request.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <returns>The decoded response object.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected abstract object OAuthRequest(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod);
#endif

        /// <summary>
        /// Invoke the old restserver.php endpoint.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="parameters">The parameters for the server call.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        protected abstract void RestServerAsync(FacebookAsyncCallback callback, object state, IDictionary<string, object> parameters, HttpMethod httpMethod);

        /// <summary>
        /// Invoke the Graph API.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <param name="parameters">object of url parameters.</param>
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
            Contract.Requires(!String.IsNullOrEmpty(method));
            Contract.Ensures(Contract.Result<Uri>() != default(Uri));

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
            Contract.Requires(!String.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<Uri>() != default(Uri));

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
            Contract.Requires(!String.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<Uri>() != default(Uri));

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
            Contract.Requires(!String.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<Uri>() != default(Uri));

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
            Contract.Requires(!String.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<Uri>() != default(Uri));

            UriBuilder uri = new UriBuilder(_domainMaps[name]);
            if (!String.IsNullOrEmpty(path))
            {
                if (path[0] == '/')
                {
                    if (path.Length > 1)
                    {
                        path = path.Substring(1);
                    }
                    else
                    {
                        path = string.Empty;
                    }
                }
                uri.Path = Uri.EscapeDataString(path);
            }
            if (parameters != null)
            {
                uri.Query = parameters.ToJsonQueryString();
            }
            return uri.Uri;
        }

        /// <summary>
        /// Removes the querystring parameters from the path value and adds them
        /// to the parameters dictionary.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static string ParseUrlParameters(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            if (String.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            Uri url;
            if (Uri.TryCreate(path, UriKind.Absolute, out url))
            {
                if (url.Host == "graph.facebook.com")
                {
                    // If the host is graph.facebook.com the user has passed in the full url.
                    // We remove the host part and continue with the parsing.
                    path = String.Concat(url.AbsolutePath, url.Query);
                }
                else
                {
                    // If the url is a valid absolute url we are passing the full url as the 'id'
                    // parameter of the query. For example, if path is something like
                    // http://www.microsoft.com/page.aspx?id=23 it means that we are trying
                    // to make the request https://graph.facebook.com/http://www.microsoft.com/page.aspx%3Fid%3D23
                    // So we are just going to return the path
                    return path;
                }
            }

            // Clean the path, remove leading '/'. 
            // If the path is '/' just return.
            if (path[0] == '/' && path.Length > 1)
            {
                path = path.Substring(1);
            }

            // If the url does not have a host it means we are using a url
            // like /me or /me?fields=first_name,last_name so we want to
            // remove the querystring info and add it to parameters
            if (!String.IsNullOrEmpty(path) && path.Contains('?'))
            {
                var parts = path.Split('?');
                path = parts[0]; // Set the path to only the path portion of the url
                if (parts.Length > 1 && parts[1] != null)
                {
                    // Add the query string values to the parameters dictionary
                    var qs = parts[1];
                    var keyValPairs = qs.Split('&');
                    foreach (var kvp in keyValPairs)
                    {
                        if (!String.IsNullOrEmpty(kvp))
                        {
                            var kv = kvp.Split('=');
                            if (kv.Length == 2 && !String.IsNullOrEmpty(kv[0]))
                            {
                                parameters[kv[0]] = kv[1];
                            }
                        }
                    }
                }
            }

            return path;
        }

    }
}