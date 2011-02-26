// --------------------------------
// <copyright file="FacebookAppBase.cs" company="Facebook C# SDK">
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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Facebook.Web;

    /// <summary>
    /// Represents the core Facebook functionality.
    /// </summary>
    [Obsolete]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignTimeVisible(false)]
    [Browsable(false)]
    [ContractClass(typeof(FacebookAppBaseContracts))]
    public abstract class FacebookAppBase
    {
        private FacebookClient m_facebookClient;

        protected FacebookClient FacebookClient
        {
            get
            {
                if (m_facebookClient == null)
                {
                    m_facebookClient = new FacebookClient(this.AccessToken);
                }
                return m_facebookClient;
            }
        }

        /// <summary>
        /// Gets the Application ID.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets the Application API Secret.
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets the active user session.
        /// </summary>
        /// <value>The session.</value>
        public virtual FacebookSession Session { get; set; }

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
                else if (!String.IsNullOrEmpty(this.AppId) && !String.IsNullOrEmpty(this.AppSecret))
                {
                    return string.Concat(this.AppId, "|", this.AppSecret);
                }
                return null;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return this.Session != null;
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

        #region Api Methods

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

            return this.FacebookClient.Api(path, parameters, resultType, httpMethod);
        }

        #endregion

        #region Api Get/Post/Delete Methods

        public object Delete(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            return this.Api(path, null, null, HttpMethod.Delete);

        }

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

            this.FacebookClient.ApiAsync(path, parameters, httpMethod, callback, state);
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

            this.FacebookClient.ApiAsync<T>(path, parameters, httpMethod, callback, state);

        }

        #endregion

        #region Async Api Get/Post/Delete Methods

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
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
        /// <param name="state">The async state.</param>
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
        /// <param name="state">The async state.</param>
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
        /// <param name="state">The async state.</param>
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
        /// <param name="state">The async state.</param>
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
        /// <param name="state">The async state.</param>
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
        /// <param name="state">The async state.</param>
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
        /// <returns>A dynamic object with the resulting data.</returns>
        public void PostAsync(IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(callback != null);
            Contract.Requires(parameters != null);

            this.ApiAsync(null, parameters, HttpMethod.Post, callback, state);
        }

        #endregion
    }
}