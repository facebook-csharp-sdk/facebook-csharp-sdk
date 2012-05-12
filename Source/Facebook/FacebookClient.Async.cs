//-----------------------------------------------------------------------
// <copyright file="FacebookClient.Async.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
#if FLUENTHTTP_CORE_TPL
    using System.ComponentModel;
#endif
    using System.Diagnostics.CodeAnalysis;
#if NETFX_CORE
    using System.Linq;
#endif
    using System.IO;
    using System.Net;

    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public partial class FacebookClient
    {
        private HttpWebRequestWrapper _httpWebRequest;
        private object _httpWebRequestLocker = new object();

        /// <summary>
        /// Event handler for get completion.
        /// </summary>
        public event EventHandler<FacebookApiEventArgs> GetCompleted;

        /// <summary>
        /// Event handler for post completion.
        /// </summary>
        public event EventHandler<FacebookApiEventArgs> PostCompleted;

        /// <summary>
        /// Event handler for delete completion.
        /// </summary>
        public event EventHandler<FacebookApiEventArgs> DeleteCompleted;

        /// <summary>
        /// Event handler for upload progress changed.
        /// </summary>
        public event EventHandler<FacebookUploadProgressChangedEventArgs> UploadProgressChanged;

#if FLUENTHTTP_CORE_TPL

        /// <summary>
        /// Event handler when http web request wrapper is created for async api only.
        /// (used internally by TPL for cancellation support)
        /// </summary>
        private event EventHandler<HttpWebRequestCreatedEventArgs> HttpWebRequestWrapperCreated;

#endif

        /// <summary>
        /// Cancels asynchronous requests.
        /// </summary>
        /// <remarks>
        /// Does not cancel requests created using XTaskAsync methods.
        /// </remarks>
        public virtual void CancelAsync()
        {
            lock (_httpWebRequestLocker)
            {
                if (_httpWebRequest != null)
                    _httpWebRequest.Abort();
            }
        }

        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="resultType">The type of deserialize object into.</param>
        /// <param name="userState">The user state.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use ApiTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        protected virtual void ApiAsync(HttpMethod httpMethod, string path, object parameters, Type resultType, object userState)
        {
            Stream input;
            bool containsEtag;
            IList<int> batchEtags = null;
            var httpHelper = PrepareRequest(httpMethod, path, parameters, resultType, out input, out containsEtag, out batchEtags);
            _httpWebRequest = httpHelper.HttpWebRequest;

#if FLUENTHTTP_CORE_TPL
            if (HttpWebRequestWrapperCreated != null)
                HttpWebRequestWrapperCreated(this, new HttpWebRequestCreatedEventArgs(userState, httpHelper.HttpWebRequest));
#endif

            var uploadProgressChanged = UploadProgressChanged;
            bool notifyUploadProgressChanged = uploadProgressChanged != null && httpHelper.HttpWebRequest.Method == "POST";

            httpHelper.OpenReadCompleted +=
                (o, e) =>
                {
                    if (e.Cancelled)
                    {
                        OnCompleted(httpMethod, new FacebookApiEventArgs(null, true, userState, null));
                    }
                    else if (e.Error != null)
                    {
                        OnCompleted(httpMethod, new FacebookApiEventArgs(e.Error, false, userState, null));
                    }
                    else
                    {
                        string responseString;
                        try
                        {
                            using (var stream = e.Result)
                            {
                                var response = httpHelper.HttpWebResponse;
                                if (response.StatusCode == HttpStatusCode.NotModified)
                                {
                                    OnCompleted(httpMethod, new FacebookApiEventArgs(null, false, userState, NotModifiedResponse(response)));
                                    return;
                                }
                                else
                                {
                                    using (var reader = new StreamReader(stream))
                                    {
                                        responseString = reader.ReadToEnd();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            OnCompleted(httpMethod, httpHelper.HttpWebRequest.IsCancelled
                                                        ? new FacebookApiEventArgs(null, true, userState, null)
                                                        : new FacebookApiEventArgs(ex, false, userState, null));
                            return;
                        }

                        try
                        {
                            var result = ProcessResponse(httpHelper, responseString, resultType, containsEtag, batchEtags);
                            OnCompleted(httpMethod, new FacebookApiEventArgs(null, false, userState, result));
                        }
                        catch (Exception ex)
                        {
                            OnCompleted(httpMethod, new FacebookApiEventArgs(ex, false, userState, null));
                        }
                    }
                };

            if (input != null)
            {
                httpHelper.OpenWriteCompleted +=
                    (o, e) =>
                    {
                        if (e.Cancelled)
                        {
                            var args = new FacebookApiEventArgs(null, true, userState, null);
                            OnCompleted(httpMethod, args);
                        }
                        else if (e.Error != null)
                        {
                            var args = new FacebookApiEventArgs(e.Error, false, userState, null);
                            OnCompleted(httpMethod, args);
                        }
                        else
                        {
                            bool finallyError = false;
                            try
                            {
                                using (var stream = e.Result)
                                {
                                    // write input to requestStream
                                    var buffer = new byte[BufferSize];
                                    int nread;

                                    if (notifyUploadProgressChanged)
                                    {
                                        long totalBytesToSend = input.Length;
                                        long bytesSent = 0;

                                        while ((nread = input.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            stream.Write(buffer, 0, nread);
                                            stream.Flush();

                                            // notify upload progress changed
                                            bytesSent += nread;
                                            OnUploadProgressChanged(new FacebookUploadProgressChangedEventArgs(0, 0, bytesSent, totalBytesToSend, ((int)(bytesSent * 100 / totalBytesToSend)), userState));
                                        }
                                    }
                                    else
                                    {
                                        while ((nread = input.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            stream.Write(buffer, 0, nread);
                                            stream.Flush();
                                        }
                                    }
                                }
                            }
                            catch (WebExceptionWrapper ex)
                            {
                                if (httpHelper.HttpWebRequest.IsCancelled)
                                    OnCompleted(httpMethod, new FacebookApiEventArgs(null, true, userState, null));
                                else if (ex.GetResponse() == null)
                                    OnCompleted(httpMethod, new FacebookApiEventArgs(ex, false, userState, null));
                                return;
                            }
                            catch (Exception ex)
                            {
                                OnCompleted(httpMethod,
                                            httpHelper.HttpWebRequest.IsCancelled
                                                ? new FacebookApiEventArgs(null, true, userState, null)
                                                : new FacebookApiEventArgs(ex, false, userState, null));
                                return;
                            }
                            finally
                            {
                                try
                                {
                                    input.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    finallyError = true;
                                    OnCompleted(httpMethod, new FacebookApiEventArgs(ex, false, userState, null));
                                }
                            }

                            if (!finallyError)
                                httpHelper.OpenReadAsync();
                        }
                    };

                httpHelper.OpenWriteAsync();
            }
            else
            {
                httpHelper.OpenReadAsync();
            }
        }

        #region Events

        /// <summary>
        /// Raise OnGetCompleted event handler.
        /// </summary>
        /// <param name="args">The <see cref="FacebookApiEventArgs"/>.</param>
#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [SuppressMessage("Microsoft.Design", "CA1041:ProvideObsoleteAttributeMessage")]
#endif
        protected virtual void OnGetCompleted(FacebookApiEventArgs args)
        {
            if (GetCompleted != null)
                GetCompleted(this, args);
        }

        /// <summary>
        /// Raise OnPostCompleted event handler.
        /// </summary>
        /// <param name="args">The <see cref="FacebookApiEventArgs"/>.</param>
#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [SuppressMessage("Microsoft.Design", "CA1041:ProvideObsoleteAttributeMessage")]
#endif
        protected virtual void OnPostCompleted(FacebookApiEventArgs args)
        {
            if (PostCompleted != null)
                PostCompleted(this, args);
        }

        /// <summary>
        /// Raise OnDeletedCompleted event handler.
        /// </summary>
        /// <param name="args">The <see cref="FacebookApiEventArgs"/>.</param>
#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [SuppressMessage("Microsoft.Design", "CA1041:ProvideObsoleteAttributeMessage")]
#endif
        protected virtual void OnDeleteCompleted(FacebookApiEventArgs args)
        {
            if (DeleteCompleted != null)
                DeleteCompleted(this, args);
        }

        /// <summary>
        /// Raise OnUploadProgressCompleted event handler.
        /// </summary>
        /// <param name="args">The <see cref="FacebookApiEventArgs"/>.</param>
#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [SuppressMessage("Microsoft.Design", "CA1041:ProvideObsoleteAttributeMessage")]
#endif
        protected void OnUploadProgressChanged(FacebookUploadProgressChangedEventArgs args)
        {
            if (UploadProgressChanged != null)
                UploadProgressChanged(this, args);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [SuppressMessage("Microsoft.Design", "CA1041:ProvideObsoleteAttributeMessage")]
#endif
        private void OnCompleted(HttpMethod httpMethod, FacebookApiEventArgs args)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    OnGetCompleted(args);
                    break;
                case HttpMethod.Post:
                    OnPostCompleted(args);
                    break;
                case HttpMethod.Delete:
                    OnDeleteCompleted(args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("httpMethod");
            }
        }

        #endregion

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <returns>The json result.</returns>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use GetTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(string path)
        {
            GetAsync(path, null, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>The json result.</returns>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use GetTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(object parameters)
        {
            GetAsync(null, parameters, null);
        }
        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>The json result.</returns>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use GetTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(string path, object parameters)
        {
            GetAsync(path, parameters, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="userState">The user state.</param>
        /// <returns>The json result.</returns>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use GetTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(string path, object parameters, object userState)
        {
            ApiAsync(HttpMethod.Get, path, parameters, null, userState);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>The json result.</returns>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use PostTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void PostAsync(object parameters)
        {
            PostAsync(null, parameters, null);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>The json result.</returns>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use PostTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void PostAsync(string path, object parameters)
        {
            PostAsync(path, parameters, null);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="userState">The user state.</param>
        /// <returns>The json result.</returns>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use PostTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void PostAsync(string path, object parameters, object userState)
        {
            ApiAsync(HttpMethod.Post, path, parameters, null, userState);
        }

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <returns>The json result.</returns>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use DeleteTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void DeleteAsync(string path)
        {
            DeleteAsync(path, null, null);
        }

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="userState">The user state.</param>
        /// <returns>The json result.</returns>
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use DeleteTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void DeleteAsync(string path, object parameters, object userState)
        {
            ApiAsync(HttpMethod.Delete, path, parameters, null, userState);
        }
    }
}