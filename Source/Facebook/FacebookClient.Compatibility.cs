namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Text;

    public partial class FacebookClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/>.
        /// </summary>
        /// <param name="appId">The Facebook application id.</param>
        /// <param name="appSecret">The Facebook application secret.</param>
        [Obsolete("Method marked for removal.")]
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
        [Obsolete("Method marked for removal.")]
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
        /// <param name="resultType">The type of the API request result to deserialize the response data.</param>
        /// <param name="httpMethod">The http method for the request. Default is 'GET'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public virtual object Api(string path, IDictionary<string, object> parameters, Type resultType, HttpMethod httpMethod)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            return this.Api(path, parameters, httpMethod, resultType);
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
        public virtual void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            throw new NotImplementedException();

            var mergedParameters = FacebookUtils.Merge(null, parameters);

            if (!mergedParameters.ContainsKey("access_token") && !String.IsNullOrEmpty(this.AccessToken))
            {
                mergedParameters["access_token"] = this.AccessToken;
            }

            Uri requestUrl;
            string contentType;
            byte[] postData = BuildRequestData(path, parameters, httpMethod, out requestUrl, out contentType);

            var tempState = new WebClientTempState
            {
                UserState = state,
                Method = httpMethod,
            };

            string method = FacebookUtils.ConvertToString(httpMethod);

            var webClient = new WebClient();
            webClient.UploadDataCompleted += (o, e) => { };
            webClient.DownloadDataCompleted += (o, e) =>
                                                   {
                                                       var json = Encoding.UTF8.GetString(e.Result);
                                                   };

            if (httpMethod == HttpMethod.Get)
            {
                webClient.DownloadDataAsync(requestUrl, tempState);
            }
            else
            {
                throw new NotImplementedException();
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
        [Obsolete("Marked for removal.")]
        public virtual void ApiAsync<T>(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, FacebookAsyncCallback<T> callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            throw new NotImplementedException();
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
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
        [Obsolete("Marked for removal.")]
        public void PostAsync(string path, object parameters, FacebookAsyncCallback callback)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));
            Contract.Requires(callback != null);

            this.PostAsync(path, FacebookUtils.ToDictionary(parameters), callback, null);
        }

        #endregion
    }
}