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
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Provides access to the Facebook Platform.
    /// </summary>
    public partial class FacebookClient
#if !SILVERLIGHT
 : IDisposable
#endif
    {
        private WebClient m_webClient = new WebClient();

        private bool m_isBeta = FacebookContext.Current.IsBeta;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class. 
        /// </summary>
        public FacebookClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class. 
        /// </summary>
        /// <param name="accessToken">
        /// The Facebook access token.
        /// </param>
        public FacebookClient(string accessToken)
        {
            Contract.Requires(!String.IsNullOrEmpty(accessToken));
            this.AccessToken = accessToken;
        }

        public event EventHandler<FacebookApiEventArgs> DeleteCompleted;
        public event EventHandler<FacebookApiEventArgs> PostCompleted;
        public event EventHandler<FacebookApiEventArgs> GetCompleted;

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsBeta.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsBeta
        {
            get { return this.m_isBeta; }
            set { this.m_isBeta = value; }
        }

        /// <summary>
        /// Gets the list of query parameters that get automatically dropped when rebuilding the current URL.
        /// </summary>
        protected virtual ICollection<string> DropQueryParameters
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<string>>() != null);
                return FacebookUtils.DropQueryParameters;
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
                return this.IsBeta ? FacebookUtils.DomainMapsBeta : FacebookUtils.DomainMaps;
            }
        }

        #region Get/Post/Delete Methods

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

            return this.Api(path, null, HttpMethod.Delete);
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
            return this.Api(path, parameters, HttpMethod.Delete);
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

            return this.Api(path, null, HttpMethod.Get);
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

            return this.Api(path, parameters, HttpMethod.Get);
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

            return this.Api(null, parameters, HttpMethod.Get);
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

            return this.Api<T>(path, null, HttpMethod.Get);
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

            return this.Api<T>(path, parameters, HttpMethod.Get);
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

            return this.Api<T>(null, parameters, HttpMethod.Get);
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

            return this.Api(path, parameters, HttpMethod.Post);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public object Post(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            return this.Api(null, parameters, HttpMethod.Post);
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

        #region Async Get/Post/Delete Methods

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void DeleteAsync(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.DeleteAsync(path, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void DeleteAsync(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.DeleteAsync(path, parameters, (object)null);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void DeleteAsync(string path, IDictionary<string, object> parameters, object userToken)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(path, parameters, HttpMethod.Delete, userToken);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void GetAsync(string path)
        {
            Contract.Requires(!String.IsNullOrEmpty(path));

            this.GetAsync(path, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void GetAsync(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.GetAsync(path, parameters, (object)null);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void GetAsync(string path, IDictionary<string, object> parameters, object userToken)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(path, parameters, HttpMethod.Get, null);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="parameters">object of url parameters.</param>
        /// <param name="callback">The async callback.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void GetAsync(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            this.GetAsync(null, parameters);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void PostAsync(string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.PostAsync(path, parameters, (object)null);
        }

        /// <summary>
        /// Make an api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">object of url parameters.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public void PostAsync(string path, IDictionary<string, object> parameters, object userToken)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.ApiAsync(path, parameters, HttpMethod.Post, null);
        }

        /// <summary>
        /// Make an API call.
        /// </summary>
        /// <param name="parameters">object of url parameters.</param>
        /// <param name="callback">The async callback.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        public void PostAsync(IDictionary<string, object> parameters)
        {
            Contract.Requires(parameters != null);

            this.PostAsync(null, parameters);
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
        public void PostAsync(string path, object parameters)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.PostAsync(path, FacebookUtils.ToDictionary(parameters));
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
        public void PostAsync(string path, object parameters, object userToken)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));

            this.PostAsync(path, FacebookUtils.ToDictionary(parameters), userToken);
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
        public void PostAsync(object parameters)
        {
            Contract.Requires(parameters != null);

            this.PostAsync(FacebookUtils.ToDictionary(parameters));
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
        public void QueryAsync(string fql)
        {
            Contract.Requires(!String.IsNullOrEmpty(fql));

            var parameters = new Dictionary<string, object>();
            parameters["query"] = fql;
            parameters["method"] = "fql.query";
            this.GetAsync(parameters);
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
        public void QueryAsync(string[] fql)
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
            this.GetAsync(parameters);
        }

        public void CancelAsync()
        {
            this.m_webClient.CancelAsync();
        }

#if (!SILVERLIGHT) // Silverlight should only have async calls

        protected object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            return Api(path, parameters, httpMethod, null);
        }

        protected T Api<T>(string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            return (T)Api(path, parameters, httpMethod, typeof(T));
        }

        protected virtual object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            var mergedParameters = FacebookUtils.Merge(null, parameters);

            if (!mergedParameters.ContainsKey("access_token") && !String.IsNullOrEmpty(this.AccessToken))
            {
                mergedParameters["access_token"] = this.AccessToken;
            }

            Uri requestUrl;
            string contentType;
            byte[] postData = BuildRequestData(path, mergedParameters, httpMethod, out requestUrl, out contentType);

            byte[] resultData;
            string method = FacebookUtils.ConvertToString(httpMethod);
            var webClient = this.m_webClient;
            try
            {
                if (httpMethod == HttpMethod.Get)
                {
                    resultData = webClient.DownloadData(requestUrl);
                }
                else
                {
                    webClient.Headers.Add("Content-Type", contentType);
                    resultData = webClient.UploadData(requestUrl, method, postData);
                }
            }
            catch (WebException ex)
            {
                // Graph API Errors or general web exceptions
                var exception = ExceptionFactory.GetGraphException(ex);
                if (exception != null)
                {
                    throw exception;
                }

                throw;
            }
            string json = Encoding.UTF8.GetString(resultData);

            var restException = ExceptionFactory.CheckForRestException(this.DomainMaps, requestUrl, json);
            if (restException != null)
            {
                throw restException;
            }

            return JsonSerializer.Current.DeserializeObject(json, resultType);
        }

#endif

        protected virtual void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, object userToken)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            if (!parameters.ContainsKey("access_token") && !String.IsNullOrEmpty(this.AccessToken))
            {
                parameters["access_token"] = this.AccessToken;
            }

            Uri requestUrl;
            string contentType;
            byte[] postData = BuildRequestData(path, parameters, httpMethod, out requestUrl, out contentType);

            var tempState = new WebClientTempState
            {
                UserState = userToken,
                Method = httpMethod,
            };

            string method = FacebookUtils.ConvertToString(httpMethod);
            var webClient = this.m_webClient;

#if SILVERLIGHT
            webClient.UploadStringCompleted += UploadStringCompleted;
            webClient.DownloadStringCompleted += DownloadStringCompleted;
            if (httpMethod == HttpMethod.Get)
            {
                webClient.DownloadStringAsync(requestUrl, tempState);
            }
            else
            {
                var data = Encoding.UTF8.GetString(postData, 0, postData.Length);
                webClient.Headers["Content-Type"] = contentType;
                webClient.UploadStringAsync(requestUrl, method, data, tempState);
            }
#else
            webClient.UploadDataCompleted += UploadDataCompleted;
            webClient.DownloadDataCompleted += DownloadDataCompleted;
            if (httpMethod == HttpMethod.Get)
            {
                webClient.DownloadDataAsync(requestUrl, tempState);
            }
            else
            {
                webClient.Headers["Content-Type"] = contentType;
                webClient.UploadDataAsync(requestUrl, method, postData, tempState);
            }
#endif
        }

        protected void OnGetCompleted(FacebookApiEventArgs args)
        {
            if (GetCompleted != null)
            {
                GetCompleted(this, args);
            }
        }

        protected void OnPostCompleted(FacebookApiEventArgs args)
        {
            if (PostCompleted != null)
            {
                PostCompleted(this, args);
            }
        }

        protected void OnDeleteCompleted(FacebookApiEventArgs args)
        {
            if (DeleteCompleted != null)
            {
                DeleteCompleted(this, args);
            }
        }

        #region Url helper methods

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
            if (FacebookUtils.ReadOnlyCalls.Contains(method))
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
        internal virtual Uri GetUrl(string name, string path, IDictionary<string, object> parameters)
        {
            Contract.Requires(!String.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<Uri>() != default(Uri));

            if (this.DomainMaps[name] == null)
            {
                throw new ArgumentException("Invalid url name.");
            }

            UriBuilder uri = new UriBuilder(this.DomainMaps[name]);
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

        #endregion

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
        internal static byte[] BuildRequestData(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, out Uri requestUrl, out string contentType)
        {
            Contract.Requires(uri != null);
            Contract.Requires(parameters != null);

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
                if (parameters.ContainsKey("access_token"))
                {
                    queryString = string.Concat("access_token=", parameters["access_token"]);
                    parameters.Remove("access_token");
                }
                else if (parameters.ContainsKey("oauth_token"))
                {
                    queryString = string.Concat("oauth_token=", parameters["oauth_token"]);
                    parameters.Remove("oauth_token");
                }

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
                    sb.Append(FacebookUtils.MultiPartFormPrefix).Append(boundary).Append(FacebookUtils.MultiPartNewLine);
                    sb.Append("Content-Disposition: form-data; name=\"").Append(kvp.Key).Append("\"");
                    sb.Append(FacebookUtils.MultiPartNewLine);
                    sb.Append(FacebookUtils.MultiPartNewLine);
                    sb.Append(kvp.Value);
                    sb.Append(FacebookUtils.MultiPartNewLine);
                }
            }

            Debug.Assert(mediaObject != null, "The mediaObject is null.");

            if (mediaObject.ContentType == null || mediaObject.GetValue() == null || mediaObject.FileName == null)
            {
                throw ExceptionFactory.MediaObjectMustHavePropertiesSet;
            }

            sb.Append(FacebookUtils.MultiPartFormPrefix).Append(boundary).Append(FacebookUtils.MultiPartNewLine);
            sb.Append("Content-Disposition: form-data; filename=\"").Append(mediaObject.FileName).Append("\"").Append(FacebookUtils.MultiPartNewLine);
            sb.Append("Content-Type: ").Append(mediaObject.ContentType).Append(FacebookUtils.MultiPartNewLine).Append(FacebookUtils.MultiPartNewLine);

            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] fileData = mediaObject.GetValue();
            byte[] boundaryBytes = Encoding.UTF8.GetBytes(String.Concat(FacebookUtils.MultiPartNewLine, FacebookUtils.MultiPartFormPrefix, boundary, FacebookUtils.MultiPartFormPrefix, FacebookUtils.MultiPartNewLine));

            // Combine all bytes to post
            var postData = new byte[postHeaderBytes.Length + fileData.Length + boundaryBytes.Length];
            Buffer.BlockCopy(postHeaderBytes, 0, postData, 0, postHeaderBytes.Length);
            Buffer.BlockCopy(fileData, 0, postData, postHeaderBytes.Length, fileData.Length);
            Buffer.BlockCopy(boundaryBytes, 0, postData, postHeaderBytes.Length + fileData.Length, boundaryBytes.Length);

            return postData;
        }

        private byte[] BuildRequestData(string path, IDictionary<string, object> parameters, HttpMethod method, out Uri requestUrl, out string contentType)
        {
            parameters = parameters ?? new Dictionary<string, object>();

            path = FacebookUtils.ParseQueryParametersToDictionary(path, parameters);

            Uri baseUrl;
            if (parameters.ContainsKey("method"))
            {
                if (String.IsNullOrEmpty((string)parameters["method"]))
                {
                    throw new ArgumentException("You must specify a value for the method parameter.");
                }

                // Set the format to json
                parameters["format"] = "json-strings";
                baseUrl = this.GetApiUrl(parameters["method"].ToString());
            }
            else
            {
                if (!String.IsNullOrEmpty(path) && path.StartsWith("/", StringComparison.Ordinal))
                {
                    path = path.Substring(1, path.Length - 1);
                }
                baseUrl = this.GetUrl("graph", path);
            }

            return BuildRequestData(baseUrl, parameters, method, out requestUrl, out contentType);
        }

        private class WebClientTempState
        {
            public object UserState { get; set; }
            public HttpMethod Method { get; set; }
            public Uri RequestUri { get; set; }
        }

#if !SILVERLIGHT
        public void Dispose()
        {
            if (this.m_webClient != null) this.m_webClient.Dispose();
        }

        private void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            var json = Encoding.UTF8.GetString(e.Result);
            OnDownloadDataCompleted(e, json);
        }

        private void UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            var json = Encoding.UTF8.GetString(e.Result);
            OnUploadDataCompleted(e, json);
        }
#endif

        private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            OnDownloadDataCompleted(e, e.Result);
        }

        private void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            OnUploadDataCompleted(e, e.Result);
        }

        private void OnDownloadDataCompleted(AsyncCompletedEventArgs e, string json)
        {
            HttpMethod method;
            var args = GetApiEventArgs(e, json, out method);
            OnGetCompleted(args);
        }

        private void OnUploadDataCompleted(AsyncCompletedEventArgs e, string json)
        {
            HttpMethod method;
            var args = GetApiEventArgs(e, json, out method);
            if (method == HttpMethod.Post)
            {
                OnPostCompleted(args);
            }
            else if (method == HttpMethod.Delete)
            {
                OnDeleteCompleted(args);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private FacebookApiEventArgs GetApiEventArgs(AsyncCompletedEventArgs e, string json, out HttpMethod httpMethod)
        {
            var state = e.UserState as WebClientTempState;
            httpMethod = state.Method;

            var cancelled = e.Cancelled;
            var userState = state.UserState;
            var error = e.Error;

            // Check for Graph Exception
            WebException webException = error as WebException;
            if (webException != null)
            {
                error = ExceptionFactory.GetGraphException(webException);
            }

            if (error == null)
            {
                error = ExceptionFactory.CheckForRestException(this.DomainMaps, state.RequestUri, json) ?? error;
            }

            var args = new FacebookApiEventArgs(error, cancelled, userState, json);
            return args;
        }
    }
}