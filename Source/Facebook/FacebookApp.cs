// --------------------------------
// <copyright file="FacebookApp.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the core Facebook functionality.
    /// </summary>
    [Obsolete("Use FacebookWebClient instead.")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class FacebookApp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/> class.
        /// </summary>
        public FacebookApp()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/> class.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        public FacebookApp(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException("accessToken");

            AccessToken = accessToken;
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
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException("appId");
            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException("appSecret");

            AppId = appId;
            AppSecret = appSecret;
            AccessToken = string.Concat(appId, "|", appSecret);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/> class.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        public FacebookApp(IFacebookApplication facebookApplication)
        {
            if (facebookApplication != null)
            {
                if (!string.IsNullOrEmpty(facebookApplication.AppId) && !string.IsNullOrEmpty(facebookApplication.AppSecret))
                {
                    AppId = facebookApplication.AppId;
                    AppSecret = facebookApplication.AppSecret;
                    AccessToken = string.Concat(facebookApplication.AppId, "|", facebookApplication.AppSecret);
                }
            }
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
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; }

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
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

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
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

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
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

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
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

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
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

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
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

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
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

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
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

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
            if (parameters == null)
                throw new ArgumentNullException("parameters");

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
            if (parameters == null)
                throw new ArgumentNullException("parameters");

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
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

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
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

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
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

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
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

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
            if (parameters == null)
                throw new ArgumentNullException("parameters");

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
            if (parameters == null)
                throw new ArgumentNullException("parameters");

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
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

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
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

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
            if (parameters == null)
                throw new ArgumentNullException("parameters");

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
            if (parameters == null)
                throw new ArgumentNullException("parameters");

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
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

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
            if (parameters == null)
                throw new ArgumentNullException("parameters");

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
            if (parameters == null)
                throw new ArgumentNullException("parameters");

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
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            PostAsync(path, FacebookUtils.ToDictionary(parameters), callback, null);
        }

        #endregion

        #region Query methods

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
            if (string.IsNullOrEmpty(fql))
                throw new ArgumentNullException("fql");

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
            if (fql == null)
                throw new ArgumentNullException("fql");
            if (fql.Length == 0)
                throw new ArgumentException("At least one fql query required.", "fql");

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

        internal protected virtual void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, FacebookAsyncCallback callback, object userToken)
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

        [Obsolete("Marked for removal.")]
        internal protected virtual void ApiAsync<T>(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, FacebookAsyncCallback<T> callback, object state)
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