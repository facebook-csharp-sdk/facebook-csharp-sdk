// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FacebookClient.cs" company="Facebook C# SDK">
//   Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Provides access to the Facebook Platform.
    /// </summary>
    public class FacebookClient
    {
        /// <summary>
        /// The multi-part form prefix characters.
        /// </summary>
        private const string Prefix = "--";

        /// <summary>
        /// The multi-part form new line characters.
        /// </summary>
        private const string NewLine = "\r\n";

        /// <summary>
        /// The collection of Facebook error types that should be retried.
        /// </summary>
        private static Collection<string> retryErrorTypes = new Collection<string>()
        {
            "OAuthException", // Graph OAuth Exception
            "190", // Rest OAuth Exception
        };

        /// <summary>
        /// How many times to retry a command if an error occurs until we give up.
        /// </summary>
        private int maxRetries = 2;

        /// <summary>
        /// How long in milliseconds to wait before retrying.
        /// </summary>
        private int retryDelay = 500;

        private static Collection<string> _dropQueryParameters = new Collection<string> {
            "session",
            "signed_request",
        };

        private static Dictionary<string, Uri> _domainMaps = new Dictionary<string, Uri> {
            { "api", new Uri("https://api.facebook.com/") },
            { "api_read", new Uri("https://api-read.facebook.com/") },
            { "api_video", new Uri("https://api-video.facebook.com/") },
            { "graph", new Uri("https://graph.facebook.com/") },
            { "www", new Uri("https://www.facebook.com/") }
        };

        private static string[] _readOnlyCalls = new[] {
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
        /// Initializes a new instance of the <see cref="FacebookClient"/>.
        /// </summary>
        public FacebookClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/>.
        /// </summary>
        /// <param name="accessToken">The Facebook access token.</param>
        public FacebookClient(string accessToken)
        {
            Contract.Requires(!String.IsNullOrEmpty(accessToken));

            this.AccessToken = accessToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/>.
        /// </summary>
        /// <param name="appId">The Facebook application id.</param>
        /// <param name="appSecret">The Facebook application secret.</param>
        public FacebookClient(string appId, string appSecret)
        {
            Contract.Requires(!String.IsNullOrEmpty(appId));
            Contract.Requires(!String.IsNullOrEmpty(appSecret));

            this.AccessToken = String.Concat(appId, "|", appSecret);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        public FacebookClient(IFacebookApplication facebookApplication)
        {
            if (facebookApplication != null)
            {
                if (!string.IsNullOrEmpty(facebookApplication.AppId) && !string.IsNullOrEmpty(facebookApplication.AppSecret))
                {
                    this.AccessToken = string.Concat(facebookApplication.AppId, "|", facebookApplication.AppSecret);
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of times to retry an api
        /// call after experiencing a recoverable exception.
        /// </summary>
        /// <value>The max retries.</value>
        public int MaxRetries
        {
            get
            {
                return this.maxRetries;
            }
            set
            {
                Contract.Requires(value >= 0);

                this.maxRetries = value;
            }
        }

        /// <summary>
        /// Gets or sets the value in seconds to wait before retrying, with exponential roll off.
        /// </summary>
        /// <value>The retry delay.</value>
        public int RetryDelay
        {
            get
            {
                return this.retryDelay;
            }
            set
            {
                Contract.Requires(value >= 0);

                this.retryDelay = value;
            }
        }

        /// <summary>
        /// Gets a collection of Facebook error types that
        /// should be retried in the event of a failure.
        /// </summary>
        protected virtual Collection<string> RetryErrorTypes
        {
            get { return retryErrorTypes; }
        }

        /// <summary>
        /// Gets the list of query parameters that get automatically dropped when rebuilding the current URL.
        /// </summary>
        protected virtual ICollection<string> DropQueryParameters
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<string>>() != null);
                return _dropQueryParameters;
            }
        }

        /// <summary>
        /// Gets the aliases to Facebook domains.
        /// </summary>
        protected virtual Dictionary<string, Uri> DomainMaps
        {
            get
            {
                Contract.Ensures(Contract.Result<Dictionary<string, Uri>>() != null);
                return _domainMaps;
            }
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; }

        #region Api Methods

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

            return this.Api(path, parameters, null, httpMethod);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <param name="resultType">The type of the API request result to deserialize the response data.</param>
        /// <param name="httpMethod">The http method for the request. Default is 'GET'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public virtual object Api(string path, IDictionary<string, object> parameters, Type resultType, HttpMethod httpMethod)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            parameters = parameters ?? new Dictionary<string, object>();

            path = ParseQueryParametersToDictionary(path, parameters);

            if (parameters.ContainsKey("method"))
            {
                if (String.IsNullOrEmpty((string)parameters["method"]))
                {
                    throw new ArgumentException("You must specify a value for the method parameter.");
                }
                return this.RestServer(parameters, httpMethod, resultType);
            }
            else
            {
                return this.Graph(path, parameters, httpMethod, resultType);
            }
        }

#endif
        #endregion

        #region Api Get/Post/Delete Methods

#if (!SILVERLIGHT) // Silverlight should only have async calls

        /// <summary>
        /// Makes a DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <returns>
        /// Returns the json result.
        /// </returns>
        public object Delete(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return this.Api(path, null, null, HttpMethod.Delete);

        }

        /// <summary>
        /// Makes a DELETE Facebook api.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public object Delete(string path, IDictionary<string, object> parameters)
        {
            return this.Api(path, parameters, null, HttpMethod.Delete);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public object Get(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return this.Api(path, null, null, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public object Get(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return this.Api(path, parameters, null, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public object Get(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            return this.Api(null, parameters, null, HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <typeparam name="T">The result of the API call.</typeparam>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>
        /// A dynamic object with the resulting data.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException"/>
        public T Get<T>(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return (T)this.Api(path, null, typeof(T), HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <typeparam name="T">The result of the API call.</typeparam>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <returns>
        /// A dynamic object with the resulting data.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException"/>
        public T Get<T>(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return (T)this.Api(path, parameters, typeof(T), HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="parameters">Dynamic object of the request parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public T Get<T>(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            return (T)this.Api(null, parameters, typeof(T), HttpMethod.Get);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public object Post(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return this.Api(path, parameters, null, HttpMethod.Post);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public object Post(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            return this.Api(null, parameters, null, HttpMethod.Post);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public object Post(object parameters)
        {
            Contract.Requires(parameters != null);

            return this.Post(FacebookUtils.ToDictionary(parameters));
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public object Post(string path, object parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return this.Post(path, FacebookUtils.ToDictionary(parameters));
        }

#endif

        #endregion

        #region Async Api Methods
        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void ApiAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
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
        public void ApiAsync(string path, FacebookAsyncCallback callback, object state)
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
        public void ApiAsync(string path, HttpMethod httpMethod, FacebookAsyncCallback callback, object state)
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
        public void ApiAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
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

            this.ApiAsync(path, parameters, httpMethod, callback, state);
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
        public virtual void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            parameters = parameters ?? new Dictionary<string, object>();

            path = ParseQueryParametersToDictionary(path, parameters);

            if (parameters.ContainsKey("method"))
            {
                if (String.IsNullOrEmpty((string)parameters["method"]))
                {
                    throw new ArgumentException("You must specify a value for the method parameter.");
                }
                this.RestServerAsync(parameters, httpMethod, null, callback, state);
            }
            else
            {
                this.GraphAsync(path, parameters, httpMethod, null, callback, state);
            }
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
        public virtual void ApiAsync<T>(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, FacebookAsyncCallback<T> callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            parameters = parameters ?? new Dictionary<string, object>();

            path = ParseQueryParametersToDictionary(path, parameters);

            var callback2 = new FacebookAsyncCallback((ar) =>
            {
                callback(new FacebookAsyncResult<T>(ar.Result, ar.AsyncState, ar.AsyncWaitHandle, ar.CompletedSynchronously, ar.IsCompleted, ar.Error));
            });

            if (parameters.ContainsKey("method"))
            {
                if (String.IsNullOrEmpty((string)parameters["method"]))
                {
                    throw new ArgumentException("You must specify a value for the method parameter.");
                }
                this.RestServerAsync(parameters, httpMethod, typeof(T), callback2, state);
            }
            else
            {
                this.GraphAsync(path, parameters, httpMethod, typeof(T), callback2, state);
            }

        }

        #endregion

        #region Async Api Get/Post/Delete Methods

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void DeleteAsync(string path, FacebookAsyncCallback callback)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.ApiAsync(path, null, HttpMethod.Delete, callback, null);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void DeleteAsync(string path, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.ApiAsync(path, null, HttpMethod.Delete, callback, state);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void DeleteAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(path, parameters, HttpMethod.Delete, callback, null);
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
        public void DeleteAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(path, parameters, HttpMethod.Delete, callback, state);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void GetAsync(string path, FacebookAsyncCallback callback)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.ApiAsync(path, null, HttpMethod.Get, callback, null);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void GetAsync(string path, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.ApiAsync(path, null, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void GetAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(path, parameters, HttpMethod.Get, callback, null);
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
        public void GetAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(path, parameters, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="parameters">object of url parameters.</param>
        /// <param name="callback">The async callback.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void GetAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(callback != null);
            Contract.Requires(parameters != null);

            this.ApiAsync(null, parameters, HttpMethod.Get, callback, null);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void GetAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(parameters != null);

            this.ApiAsync(null, parameters, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void GetAsync<T>(string path, FacebookAsyncCallback<T> callback)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.ApiAsync<T>(path, null, HttpMethod.Get, callback, null);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void GetAsync<T>(string path, FacebookAsyncCallback<T> callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.ApiAsync<T>(path, null, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void GetAsync<T>(string path, IDictionary<string, object> parameters, FacebookAsyncCallback<T> callback)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync<T>(path, parameters, HttpMethod.Get, callback, null);
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
        public void GetAsync<T>(string path, IDictionary<string, object> parameters, FacebookAsyncCallback<T> callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync<T>(path, parameters, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="parameters">object of url parameters.</param>
        /// <param name="callback">The async callback.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void GetAsync<T>(IDictionary<string, object> parameters, FacebookAsyncCallback<T> callback)
        {
            Contract.Requires(callback != null);
            Contract.Requires(parameters != null);

            this.ApiAsync<T>(null, parameters, HttpMethod.Get, callback, null);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void GetAsync<T>(IDictionary<string, object> parameters, FacebookAsyncCallback<T> callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(parameters != null);

            this.ApiAsync<T>(null, parameters, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void PostAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(path, parameters, HttpMethod.Post, callback, null);
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
        public void PostAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(path, parameters, HttpMethod.Post, callback, state);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="parameters">object of url parameters.</param>
        /// <param name="callback">The async callback.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void PostAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(callback != null);
            Contract.Requires(parameters != null);

            this.ApiAsync(null, parameters, HttpMethod.Post, callback, null);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="parameters">object of url parameters.</param>
        public void PostAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(parameters != null);

            this.ApiAsync(null, parameters, HttpMethod.Post, callback, state);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The async callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void PostAsync(string path, object parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));
            Contract.Requires(callback != null);

            this.PostAsync(path, FacebookUtils.ToDictionary(parameters), callback, state);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The async callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void PostAsync(object parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(parameters != null);
            Contract.Requires(callback != null);

            this.PostAsync(FacebookUtils.ToDictionary(parameters), callback, state);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The async callback.
        /// </param>
        public void PostAsync(object parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(parameters != null);
            Contract.Requires(callback != null);

            this.PostAsync(FacebookUtils.ToDictionary(parameters), callback, null);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The async callback.
        /// </param>
        public void PostAsync(string path, object parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));
            Contract.Requires(callback != null);

            this.PostAsync(path, FacebookUtils.ToDictionary(parameters), callback, null);
        }

        #endregion

#if (!SILVERLIGHT)
        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <returns>
        /// The FQL query result.
        /// </returns>
        public object Query(string fql)
        {
            Contract.Requires(!String.IsNullOrEmpty(fql));

            var parameters = new Dictionary<string, object>();
            parameters["query"] = fql;
            parameters["method"] = "fql.query";
            return this.Get(parameters);
        }

        /// <summary>
        /// Executes a FQL multiquery.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <returns>
        /// A collection of the FQL query results.
        /// </returns>
        public object Query(params string[] fql)
        {
            Contract.Requires(fql != null);

            var queryDict = new Dictionary<string, object>();
            for (int i = 0; i < fql.Length; i++)
            {
                queryDict.Add(string.Concat("query", i), fql[i]);
            }

            var parameters = new Dictionary<string, object>();
            parameters["queries"] = queryDict;
            parameters["method"] = "fql.multiquery";
            return this.Get(parameters);
        }
#endif

        /// <summary>
        /// Executes a FQL query asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void QueryAsync(string fql, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(!String.IsNullOrEmpty(fql));

            var parameters = new Dictionary<string, object>();
            parameters["query"] = fql;
            parameters["method"] = "fql.query";
            this.GetAsync(parameters, callback, state);
        }

        /// <summary>
        /// Executes a FQL query asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void QueryAsync(string fql, FacebookAsyncCallback callback)
        {
            this.QueryAsync(fql, callback, null);
        }

        /// <summary>
        /// Executes a FQL multiquery asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void QueryAsync(string[] fql, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(fql != null);
            Contract.Requires(fql.Length > 0);

            var queryDict = new Dictionary<string, object>();
            for (int i = 0; i < fql.Length; i++)
            {
                queryDict.Add(string.Concat("query", i), fql[i]);
            }

            var parameters = new Dictionary<string, object>();
            parameters["queries"] = queryDict;
            parameters["method"] = "fql.multiquery";
            this.GetAsync(parameters, callback, state);
        }

        /// <summary>
        /// Executes a FQL multiquery asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void QueryAsync(string[] fql, FacebookAsyncCallback callback)
        {
            this.QueryAsync(fql, callback, null);
        }

#if !SILVERLIGHT

        /// <summary>
        /// Invoke the old restserver.php endpoint.
        /// </summary>
        /// <param name="parameters">
        /// The parameters of the method call.
        /// </param>
        /// <param name="httpMethod">
        /// The http method for the request. Default is 'GET'.
        /// </param>
        /// <param name="resultType">
        /// The result type.
        /// </param>
        /// <returns>
        /// The decoded response object.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException">
        /// </exception>
        protected object RestServer(IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            // Set the format to json
            parameters["format"] = "json-strings";
            Uri uri = this.GetApiUrl(parameters["method"].ToString());
            return this.OAuthRequest(uri, parameters, httpMethod, resultType, true);
        }

        /// <summary>
        /// Make a graph api call.
        /// </summary>
        /// <param name="path">
        /// The path of the url to call such as 'me/friends'.
        /// </param>
        /// <param name="parameters">
        /// JsonObject of url parameters.
        /// </param>
        /// <param name="httpMethod">
        /// The http method for the request.
        /// </param>
        /// <param name="resultType">
        /// The result type.
        /// </param>
        /// <returns>
        /// A dynamic object with the resulting data.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException">
        /// </exception>
        protected object Graph(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            var uri = this.GetGraphRequestUri(path);

            return this.OAuthRequest(uri, parameters, httpMethod, resultType, false);
        }

        /// <summary>
        /// Make a OAuth Request
        /// </summary>
        /// <param name="uri">
        /// The url to make the request.
        /// </param>
        /// <param name="parameters">
        /// The parameters of the request.
        /// </param>
        /// <param name="httpMethod">
        /// The http method for the request.
        /// </param>
        /// <param name="resultType">
        /// The result type.
        /// </param>
        /// <param name="restApi">
        /// The rest Api.
        /// </param>
        /// <returns>
        /// The decoded response object.
        /// </returns>
        protected object OAuthRequest(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, bool restApi)
        {
            Uri requestUrl;
            string contentType;
            byte[] postData = BuildRequestData(uri, parameters, httpMethod, this.AccessToken, out requestUrl, out contentType);

            return WithMirrorRetry<object>(() => { return MakeRequest(httpMethod, requestUrl, postData, contentType, resultType, restApi); });
        }

#endif

        /// <summary>
        /// Invoke the old restserver.php endpoint.
        /// </summary>
        /// <param name="parameters">
        /// The parameters of the method call.
        /// </param>
        /// <param name="httpMethod">
        /// The http method for the request.
        /// </param>
        /// <param name="resultType">
        /// The result type.
        /// </param>
        /// <param name="callback">
        /// The async callback.
        /// </param>
        /// <param name="state">
        /// The async state.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException">
        /// </exception>
        protected void RestServerAsync(IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, FacebookAsyncCallback callback, object state)
        {
            // Set the format to json
            parameters["format"] = "json-strings";
            Uri uri = this.GetApiUrl(parameters["method"].ToString());

            this.OAuthRequestAsync(uri, parameters, httpMethod, resultType, true, callback, state);
        }

        /// <summary>
        /// Make a graph api call.
        /// </summary>
        /// <param name="path">
        /// The path of the url to call such as 'me/friends'.
        /// </param>
        /// <param name="parameters">
        /// JsonObject of url parameters.
        /// </param>
        /// <param name="httpMethod">
        /// The http method for the request.
        /// </param>
        /// <param name="resultType">
        /// The result type.
        /// </param>
        /// <param name="callback">
        /// The async callback.
        /// </param>
        /// <param name="state">
        /// The async state.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException">
        /// </exception>
        protected void GraphAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, FacebookAsyncCallback callback, object state)
        {
            var uri = this.GetGraphRequestUri(path);

            this.OAuthRequestAsync(uri, parameters, httpMethod, resultType, false, callback, state);
        }

        /// <summary>
        /// Make a OAuth Request
        /// </summary>
        /// <param name="uri">
        /// The url to make the request.
        /// </param>
        /// <param name="parameters">
        /// The parameters of the request.
        /// </param>
        /// <param name="httpMethod">
        /// The http method of the request.
        /// </param>
        /// <param name="resultType">
        /// The result type.
        /// </param>
        /// <param name="restApi">
        /// The rest Api.
        /// </param>
        /// <param name="callback">
        /// The async callback.
        /// </param>
        /// <param name="state">
        /// The async state.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException">
        /// </exception>
        protected void OAuthRequestAsync(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, bool restApi, FacebookAsyncCallback callback, object state)
        {
            Uri requestUrl;
            string contentType;
            byte[] postData = BuildRequestData(uri, parameters, httpMethod, this.AccessToken, out requestUrl, out contentType);

            MakeRequestAsync(httpMethod, requestUrl, postData, contentType, resultType, restApi, callback, state);
        }

        /// <summary>
        /// This method invokes the supplied delegate with retry logic wrapped around it and returns the value of the delegate.
        /// If the delegate raises recoverable Facebook server or client errors, then the supplied delegate is reinvoked after
        /// a certain amount of delay until the retry limit is exceeded, at which point the exception is rethrown. Other
        /// exceptions are not caught and will be visible to callers.
        /// </summary>
        /// <typeparam name="TReturn">The type of object being returned</typeparam>
        /// <param name="body">The delegate to invoke within the retry logic which will produce the value to return</param>
        /// <returns>The value the delegate returns</returns>
        protected TReturn WithMirrorRetry<TReturn>(Func<TReturn> body)
        {
            Contract.Requires(body != null);

            int retryCount = 0;

            while (true)
            {
                try
                {
                    return body();
                }
                catch (FacebookApiException ex)
                {
                    if (!this.RetryErrorTypes.Contains(ex.ErrorType))
                    {
                        throw;
                    }
                    else
                    {
                        if (retryCount >= this.maxRetries)
                        {
                            throw;
                        }
                    }
                }
                catch (WebException)
                {
                    if (retryCount >= this.maxRetries)
                    {
                        throw;
                    }
                }

                // Sleep for the retry delay before we retry again
                System.Threading.Thread.Sleep(this.retryDelay);
                retryCount += 1;
            }
        }



        /// <summary>
        /// Builds the request post data and request uri based on the given parameters.
        /// </summary>
        /// <param name="uri">The request uri.</param>
        /// <param name="parameters">The request parameters.</param>
        /// <param name="httpMethod">The http method.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="requestUrl">The outputted request uri.</param>
        /// <param name="contentType">The request content type.</param>
        /// <returns>The request post data.</returns>
        private static byte[] BuildRequestData(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, string accessToken, out Uri requestUrl, out string contentType)
        {
            Contract.Requires(uri != null);
            Contract.Requires(parameters != null);

            if (!parameters.ContainsKey("access_token") && !String.IsNullOrEmpty(accessToken))
            {
                parameters["access_token"] = accessToken;
            }

            var requestUrlBuilder = new UriBuilder(uri);

            // Set the default content type
            contentType = "application/x-www-form-urlencoded";
            byte[] postData = null;
            string queryString = string.Empty;

            if (httpMethod == HttpMethod.Get)
            {
                queryString = FacebookUtils.ToJsonQueryString(parameters);
            }
            else
            {
                queryString = string.Concat("access_token=", parameters["access_token"]);
                parameters.Remove("access_token");

                var containsMediaObject = parameters.Where(p => p.Value is FacebookMediaObject).Count() > 0;
                if (containsMediaObject)
                {
                    string boundary = DateTime.Now.Ticks.ToString("x", CultureInfo.InvariantCulture);
                    postData = BuildMediaObjectPostData(parameters, boundary);
                    contentType = String.Concat("multipart/form-data; boundary=", boundary);
                }
                else
                {
                    postData = Encoding.UTF8.GetBytes(FacebookUtils.ToJsonQueryString(parameters));
                }
            }

            requestUrlBuilder.Query = queryString;
            requestUrl = requestUrlBuilder.Uri;

            return postData;
        }

        /// <summary>
        /// Builds the request post data if the request contains a media object
        /// such as an image or video to upload.
        /// </summary>
        /// <param name="parameters">The request parameters.</param>
        /// <param name="boundary">The multipart form request boundary.</param>
        /// <returns>The request post data.</returns>
        internal static byte[] BuildMediaObjectPostData(IDictionary<string, object> parameters, string boundary)
        {
            FacebookMediaObject mediaObject = null;

            // Build up the post message header
            var sb = new StringBuilder();
            foreach (var kvp in parameters)
            {
                if (kvp.Value is FacebookMediaObject)
                {
                    // Check to make sure the file upload hasn't already been set.
                    if (mediaObject != null)
                    {
                        throw ExceptionFactory.CannotIncludeMultipleMediaObjects;
                    }

                    mediaObject = kvp.Value as FacebookMediaObject;
                }
                else
                {
                    sb.Append(Prefix).Append(boundary).Append(NewLine);
                    sb.Append("Content-Disposition: form-data; name=\"").Append(kvp.Key).Append("\"");
                    sb.Append(NewLine);
                    sb.Append(NewLine);
                    sb.Append(kvp.Value);
                    sb.Append(NewLine);
                }
            }

            Debug.Assert(mediaObject != null, "The mediaObject is null.");

            if (mediaObject.ContentType == null || mediaObject.GetValue() == null || mediaObject.FileName == null)
            {
                throw ExceptionFactory.MediaObjectMustHavePropertiesSet;
            }

            sb.Append(Prefix).Append(boundary).Append(NewLine);
            sb.Append("Content-Disposition: form-data; filename=\"").Append(mediaObject.FileName).Append("\"").Append(NewLine);
            sb.Append("Content-Type: ").Append(mediaObject.ContentType).Append(NewLine).Append(NewLine);

            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] fileData = mediaObject.GetValue();
            byte[] boundaryBytes = Encoding.UTF8.GetBytes(String.Concat(NewLine, Prefix, boundary, Prefix, NewLine));

            // Combine all bytes to post
            var postData = new byte[postHeaderBytes.Length + fileData.Length + boundaryBytes.Length];
            Buffer.BlockCopy(postHeaderBytes, 0, postData, 0, postHeaderBytes.Length);
            Buffer.BlockCopy(fileData, 0, postData, postHeaderBytes.Length, fileData.Length);
            Buffer.BlockCopy(boundaryBytes, 0, postData, postHeaderBytes.Length + fileData.Length, boundaryBytes.Length);

            return postData;
        }

#if !SILVERLIGHT

        /// <summary>
        /// Make the API Request
        /// </summary>
        /// <param name="httpMethod">
        /// The http method to use. GET, POST, DELETE.
        /// </param>
        /// <param name="requestUrl">
        /// The uri of the request.
        /// </param>
        /// <param name="postData">
        /// The request data.
        /// </param>
        /// <param name="contentType">
        /// The request content type.
        /// </param>
        /// <param name="resultType">
        /// The result type.
        /// </param>
        /// <param name="restApi">
        /// The rest api.
        /// </param>
        /// <returns>
        /// The decoded response object.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException">
        /// </exception>
        private static object MakeRequest(HttpMethod httpMethod, Uri requestUrl, byte[] postData, string contentType, Type resultType, bool restApi)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = FacebookUtils.ConvertToString(httpMethod); // Set the http method GET, POST, etc.

            if (postData != null)
            {
                request.ContentLength = postData.Length;
                request.ContentType = contentType;
                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(postData, 0, postData.Length);
                }
            }

            object result = null;
            FacebookApiException exception = null;
            try
            {
                var responseData = String.Empty;
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    responseData = streamReader.ReadToEnd();
                }

                response.Close();

                // If we are using the REST API we need to check for an exception
                if (resultType == null || restApi)
                {
                    result = JsonSerializer.DeserializeObject(responseData);
                    if (restApi)
                    {
                        exception = ExceptionFactory.GetRestException(result);
                    }
                }

                if (exception != null)
                {
                    throw exception; // Thow the FacebookApiException
                }

                // Deserialize the final result if the result type is set
                if (resultType != null)
                {
                    result = JsonSerializer.DeserializeObject(responseData, resultType);
                }
            }
            catch (FacebookApiException)
            {
                // This is a rest api error thrown above, just rethrow
                throw;
            }
            catch (WebException ex)
            {
                // Graph API Errors or general web exceptions
                exception = ExceptionFactory.GetGraphException(ex);
                if (exception != null)
                {
                    throw exception;
                }

                throw;
            }

            return result;
        }

#endif

        /// <summary>
        /// Make the API Request
        /// </summary>
        /// <param name="httpMethod">
        /// The http method to use. GET, POST, DELETE.
        /// </param>
        /// <param name="requestUrl">
        /// The uri of the request.
        /// </param>
        /// <param name="postData">
        /// The request data.
        /// </param>
        /// <param name="contentType">
        /// The request content type.
        /// </param>
        /// <param name="resultType">
        /// The result type.
        /// </param>
        /// <param name="restApi">
        /// The rest api.
        /// </param>
        /// <param name="callback">
        /// The async callback.
        /// </param>
        /// <param name="state">
        /// The async state.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException">
        /// </exception>
        private static void MakeRequestAsync(HttpMethod httpMethod, Uri requestUrl, byte[] postData, string contentType, Type resultType, bool restApi, FacebookAsyncCallback callback, object state)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = FacebookUtils.ConvertToString(httpMethod); // Set the http method GET, POST, etc.
            if (httpMethod == HttpMethod.Post)
            {
                request.ContentType = contentType;
                request.BeginGetRequestStream((ar) => { RequestCallback(ar, postData, callback, state); }, request);
            }
            else
            {
                request.BeginGetResponse((ar) => { ResponseCallback(ar, callback, state); }, request);
            }
        }

        /// <summary>
        /// The asynchronous web request callback.
        /// </summary>
        /// <param name="asyncResult">The asynchronous result.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="callback">The callback method.</param>
        /// <param name="state">The asynchronous state.</param>
        private static void RequestCallback(IAsyncResult asyncResult, byte[] postData, FacebookAsyncCallback callback, object state)
        {
            var request = (HttpWebRequest)asyncResult.AsyncState;
            using (Stream stream = request.EndGetRequestStream(asyncResult))
            {
                stream.Write(postData, 0, postData.Length);
            }

            request.BeginGetResponse((ar) => { ResponseCallback(ar, callback, state); }, request);
        }

        /// <summary>
        /// The asynchronous web response callback.
        /// </summary>
        /// <param name="asyncResult">The asynchronous result.</param>
        /// <param name="callback">The callback method.</param>
        /// <param name="state">The asynchronous state.</param>
        private static void ResponseCallback(IAsyncResult asyncResult, FacebookAsyncCallback callback, object state)
        {
            object result = null;
            FacebookApiException exception = null;
            try
            {
                var request = (HttpWebRequest)asyncResult.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(asyncResult);

                using (Stream responseStream = response.GetResponseStream())
                {
                    result = JsonSerializer.DeserializeObject(responseStream);
                }
            }
            catch (FacebookApiException)
            {
                // Rest API Errors
                throw;
            }
            catch (WebException ex)
            {
                // Graph API Errors or general web exceptions
                exception = ExceptionFactory.GetGraphException(ex);
                if (exception != null)
                {
                    // Thow the FacebookApiException
                    throw exception;
                }

                throw;
            }
            finally
            {
                // Check to make sure there hasn't been an exception.
                // If there has, we want to pass null to the callback.
                object data = null;
                if (exception == null)
                {
                    data = result;
                }
#if SILVERLIGHT
                callback(new FacebookAsyncResult(data, state, null, asyncResult.CompletedSynchronously, asyncResult.IsCompleted, exception));
#else
                callback(new FacebookAsyncResult(data, state, asyncResult.AsyncWaitHandle, asyncResult.CompletedSynchronously, asyncResult.IsCompleted, exception));
#endif
            }
        }

        #region Url Helpers
        
        /// <summary>
        /// Gets the graph request url in the proper format.
        /// </summary>
        /// <param name="path">The request url path.</param>
        /// <returns>The fully qualified uri for the request.</returns>
        private Uri GetGraphRequestUri(string path)
        {
            if (!String.IsNullOrEmpty(path) && path.StartsWith("/", StringComparison.Ordinal))
            {
                path = path.Substring(1, path.Length - 1);
            }

            var uri = this.GetUrl("graph", path);
            return uri;
        }

        /// <summary>
        /// Cleans the URL or known Facebook querystring values.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// Returns the url.
        /// </returns>
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
                            if (!this.DropQueryParameters.Contains(key))
                            {
                                string value = keyValuePair.Length > 1 ? keyValuePair[1] : null;
                                querystring.Add(key, value);
                            }
                        }
                    }
                }
                builder.Query = FacebookUtils.ToJsonQueryString(querystring);
            }
            return builder.Uri;
        }

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

            if (method.Equals("video.upload"))
            {
                name = "api_video";
            }
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

            if (_domainMaps[name] == null)
            {
                throw new ArgumentException("Invalid url name.");
            }

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
                if (!String.IsNullOrEmpty(path))
                {
                    uri.Path = FacebookUtils.UrlEncode(path);
                }
            }
            if (parameters != null)
            {
                uri.Query = FacebookUtils.ToJsonQueryString(parameters);
            }
            return uri.Uri;
        }

        /// <summary>
        /// Removes the querystring parameters from the path value and adds them
        /// to the parameters dictionary.
        /// </summary>
        /// <param name="path">The path to parse.</param>
        /// <param name="parameters">The dictionary</param>
        /// <returns></returns>
        internal static string ParseQueryParametersToDictionary(string path, IDictionary<string, object> parameters)
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
    


        #endregion

        /// <summary>
        /// The code contracts invariant object method.
        /// </summary>
        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code contracts invarient method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Code contracts invarient method.")]
        private void InvarientObject()
        {
            Contract.Invariant(this.maxRetries >= 0);
            Contract.Invariant(this.retryDelay >= 0);
            Contract.Invariant(retryErrorTypes != null);
            Contract.Invariant(_domainMaps != null);
            Contract.Invariant(_dropQueryParameters != null);
            Contract.Invariant(_readOnlyCalls != null);
        }
    }
}