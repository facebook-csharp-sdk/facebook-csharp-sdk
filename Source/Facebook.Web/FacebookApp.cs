// --------------------------------
// <copyright file="FacebookWebApp.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Facebook.Web;

    /// <summary>
    /// Represents the core Facebook functionality for web applications.
    /// </summary>
    public class FacebookApp
    {
        /// <summary>
        /// The current http context.
        /// </summary>
        private readonly FacebookWebContext _request;

        /// <summary>
        /// The current facebook session.
        /// </summary>
        private FacebookSession _session;

        /// <summary>
        /// The current facebook signed request.
        /// </summary>
        private FacebookSignedRequest _signedRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/> class.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public FacebookApp(FacebookWebContext request, string accessToken)
        {
            Contract.Requires(!string.IsNullOrEmpty(accessToken));

            _request = request;
            AccessToken = accessToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/> class.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public FacebookApp(string accessToken)
            : this(FacebookWebContext.Current, accessToken)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/> class.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        public FacebookApp(FacebookWebContext request)
            : this(request, request.AccessToken)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/> class.
        /// </summary>
        public FacebookApp()
            : this(FacebookWebContext.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/> class.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        public FacebookApp(string appId, string appSecret)
            : this(FacebookWebContext.Current)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(!string.IsNullOrEmpty(appSecret));

            AppId = appId;
            AppSecret = appSecret;
            AccessToken = string.Concat(appId, "|", appSecret);
        }

        /// <summary>
        /// Gets or sets the Application ID.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the Application API Secret.
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Gets the signed request.
        /// </summary>
        public FacebookSignedRequest SignedRequest
        {
            get
            {
                if (_signedRequest == null)
                {
                    if (_request.HttpContext.Request.Params.AllKeys.Contains("signed_request"))
                    {
                        _signedRequest = FacebookSignedRequest.Parse(AppSecret, _request.HttpContext.Request.Params["signed_request"]);
                    }
                }

                return _signedRequest;
            }
        }

        /// <summary>
        /// Gets or sets the active user session.
        /// </summary>
        public FacebookSession Session
        {
            get { return _session ?? (_session = _request.Session); }
            set { _session = value; }
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken
        {
            get
            {
                if (Session != null)
                {
                    return Session.AccessToken;
                }
                else if (!string.IsNullOrEmpty(AppId) && string.IsNullOrEmpty(AppSecret))
                {
                    return string.Concat(AppId, "|", AppSecret);
                }

                return null;
            }

            set
            {
                Session = new FacebookSession(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current session is authenticated or not.
        /// </summary>
        public bool IsAuthenticated
        {
            get { return Session != null; }
        }

        #region Api Get/Post/Delete methods

#if (!SILVERLIGHT)

        /// <summary>
        /// Makes a DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public object Delete(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return Api(path, null, null, HttpMethod.Delete);
        }

        /// <summary>
        /// Makes a DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public object Delete(string path, IDictionary<string, object> parameters)
        {
            return Api(path, parameters, null, HttpMethod.Delete);
        }

        /// <summary>
        /// Makes a GET requst to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException"/>
        public object Get(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return Api(path, null, null, HttpMethod.Get);
        }

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public object Get(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return Api(path, parameters, null, HttpMethod.Get);
        }

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public object Get(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            return Api(null, parameters, null, HttpMethod.Get);
        }

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <typeparam name="T">
        /// The result of the API call.
        /// </typeparam>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public T Get<T>(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return Api<T>(path, null, HttpMethod.Get);
        }

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="T">
        /// The result of the API call.
        /// </typeparam>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public T Get<T>(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return Api<T>(path, parameters, HttpMethod.Get);
        }

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="T">
        /// The result of the API call.
        /// </typeparam>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public T Get<T>(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            return Api<T>(null, parameters, HttpMethod.Get);
        }

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The jon result.
        /// </returns>
        public object Post(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return Api(path, parameters, null, HttpMethod.Post);
        }

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public object Post(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            return Api(null, parameters, null, HttpMethod.Post);
        }


        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public object Post(object parameters)
        {
            Contract.Requires(parameters != null);

            return Post(FacebookUtils.ToDictionary(parameters));
        }

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public object Post(string path, object parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return Post(path, FacebookUtils.ToDictionary(parameters));
        }

#endif
        #endregion

        #region Async Get/Post/Delete methods

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void DeleteAsync(string path, FacebookAsyncCallback callback)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            ApiAsync(path, null, HttpMethod.Delete, callback, null);
        }

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void DeleteAsync(string path, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            ApiAsync(path, null, HttpMethod.Delete, callback, state);
        }

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void DeleteAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            ApiAsync(path, parameters, HttpMethod.Delete, callback, null);
        }

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void DeleteAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            ApiAsync(path, parameters, HttpMethod.Delete, callback, state);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void GetAsync(string path, FacebookAsyncCallback callback)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            ApiAsync(path, null, HttpMethod.Get, callback, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void GetAsync(string path, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            ApiAsync(path, null, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void GetAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            ApiAsync(path, parameters, HttpMethod.Get, callback, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void GetAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            ApiAsync(path, parameters, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void GetAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(parameters != null);

            ApiAsync(null, parameters, HttpMethod.Get, callback, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void GetAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(parameters != null);

            ApiAsync(null, parameters, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void GetAsync<T>(string path, FacebookAsyncCallback<T> callback)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            ApiAsync<T>(path, null, HttpMethod.Get, callback, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void GetAsync<T>(string path, FacebookAsyncCallback<T> callback, object state)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            ApiAsync<T>(path, null, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void GetAsync<T>(string path, IDictionary<string, object> parameters, FacebookAsyncCallback<T> callback)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            ApiAsync<T>(path, parameters, HttpMethod.Get, callback, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void GetAsync<T>(string path, IDictionary<string, object> parameters, FacebookAsyncCallback<T> callback, object state)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            ApiAsync<T>(path, parameters, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void GetAsync<T>(IDictionary<string, object> parameters, FacebookAsyncCallback<T> callback)
        {
            Contract.Requires(parameters != null);

            ApiAsync<T>(null, parameters, HttpMethod.Get, callback, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void GetAsync<T>(IDictionary<string, object> parameters, FacebookAsyncCallback<T> callback, object state)
        {
            Contract.Requires(parameters != null);

            ApiAsync<T>(null, parameters, HttpMethod.Get, callback, state);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void PostAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            ApiAsync(path, parameters, HttpMethod.Post, callback, null);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void PostAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            ApiAsync(path, parameters, HttpMethod.Post, callback, state);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void PostAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(parameters != null);

            ApiAsync(null, parameters, HttpMethod.Post, callback, null);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void PostAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(parameters != null);

            ApiAsync(null, parameters, HttpMethod.Post, callback, state);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void PostAsync(string path, object parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            PostAsync(path, FacebookUtils.ToDictionary(parameters), callback, state);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void PostAsync(object parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(parameters != null);

            PostAsync(FacebookUtils.ToDictionary(parameters), callback, state);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void PostAsync(object parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(parameters != null);

            PostAsync(FacebookUtils.ToDictionary(parameters), callback, null);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void PostAsync(string path, object parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            PostAsync(path, FacebookUtils.ToDictionary(parameters), callback, null);
        }

        #endregion

        #region Query methods

#if !SILVERLIGHT

        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The FQL query result.
        /// </returns>
        public object Query(string fql)
        {
            Contract.Requires(!String.IsNullOrEmpty(fql));

            var parameters = new Dictionary<string, object>();
            parameters["query"] = fql;
            parameters["method"] = "fql.query";

            return Get(parameters);
        }

        /// <summary>
        /// Executes a FQL multiquery.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
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

            return Get(parameters);
        }

        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="query">
        /// The FQL query.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The FQL query result.
        /// </returns>
        [Obsolete("You should use Query rather than this method. This method will be removed in the next version.")]
        public object Fql(string query)
        {
            Contract.Requires(!String.IsNullOrEmpty(query));
            return Query(query);
        }

        /// <summary>
        /// Executes a FQL multiquery.
        /// </summary>
        /// <param name="queries">
        /// The FQL queries.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// A collection of the FQL query results.
        /// </returns>
        [Obsolete("You should use Query rather than this method. This method will be removed in the next version.")]
        public object Fql(params string[] queries)
        {
            Contract.Requires(queries != null);

            return Query(queries);
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

            GetAsync(parameters, callback, state);
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

        #endregion

        /// <summary>
        /// Gets the facebook client
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="FacebookClient"/>.
        /// </returns>
        protected FacebookClient GetFacebookClient()
        {
            // make this a method so others can easily mock the internal FacebookClient.
            return string.IsNullOrEmpty(AccessToken) ? new FacebookClient() : new FacebookClient(AccessToken);
        }

        #region ApiMethods

#if(!SILVERLIGHT) // Silverlight should only have async calls

        public T Api<T>(string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            return (T)Api(path, parameters, typeof(T), httpMethod);
        }

        public virtual object Api(string path, IDictionary<string, object> parameters, Type resultType, HttpMethod httpMethod)
        {
            try
            {
                var facebookClient = GetFacebookClient();
                return facebookClient.Api(path, parameters, httpMethod, resultType);
            }
            catch (FacebookOAuthException)
            {
                try
                {
                    _request.DeleteAuthCookie();
                }
                catch { }
                throw;
            }
        }

        public virtual object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return this.Api(path, parameters, null, httpMethod);
        }

        public object Api(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return this.Api(path, parameters, HttpMethod.Get);
        }

        public object Api(IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(parameters != null);

            return this.Api(null, parameters, httpMethod);
        }

        public object Api(string path, HttpMethod httpMethod)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return this.Api(path, null, httpMethod);
        }

        public object Api(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return this.Api(path, null, HttpMethod.Get);
        }

        public object Api(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            return this.Api(null, parameters, HttpMethod.Get);
        }

#endif
        public virtual void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, FacebookAsyncCallback callback, object userToken)
        {
            var facebookClient = GetFacebookClient();

            if (callback != null)
            {
                switch (httpMethod)
                {
                    case HttpMethod.Get:
                        facebookClient.GetCompleted += (o, e) => callback(new FacebookAsyncResult(e.GetResultData(), e.UserState, null, false, true, e.Error as FacebookApiException));
                        break;
                    case HttpMethod.Post:
                        facebookClient.PostCompleted += (o, e) => callback(new FacebookAsyncResult(e.GetResultData(), e.UserState, null, false, true, e.Error as FacebookApiException));
                        break;
                    case HttpMethod.Delete:
                        facebookClient.DeleteCompleted += (o, e) => callback(new FacebookAsyncResult(e.GetResultData(), e.UserState, null, false, true, e.Error as FacebookApiException));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("httpMethod");
                }
            }

            facebookClient.ApiAsync(path, parameters, httpMethod, userToken);
        }

        public virtual void ApiAsync(FacebookAsyncCallback callback, object state, string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(path, parameters, httpMethod, callback, state);
        }

        public void ApiAsync(string path, IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(callback, state, path, parameters, HttpMethod.Get);
        }

        public void ApiAsync(string path, HttpMethod httpMethod, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.ApiAsync(callback, state, path, null, httpMethod);
        }

        public void ApiAsync(string path, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.ApiAsync(callback, state, path, null, HttpMethod.Get);
        }

        public void ApiAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(parameters != null);

            this.ApiAsync(callback, state, null, parameters, HttpMethod.Get);
        }

        public virtual void ApiAsync<T>(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, FacebookAsyncCallback<T> callback, object state)
        {
            ApiAsync(
                path,
                parameters,
                httpMethod,
                ar =>
                {
                    if (callback != null)
                    {
                        callback(new FacebookAsyncResult<T>(ar.Result, ar.AsyncState, ar.AsyncWaitHandle, ar.CompletedSynchronously, ar.IsCompleted, ar.Error));
                    }
                },
                state);
        }

        #endregion
    }
}