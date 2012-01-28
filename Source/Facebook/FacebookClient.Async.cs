// --------------------------------
// <copyright file="FacebookClient.Async.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
// ---------------------------------

namespace Facebook
{
    using System;
#if FLUENTHTTP_CORE_TPL
    using System.ComponentModel;
#endif
    using System.IO;
    using System.Net;

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
#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use ApiTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        protected virtual void ApiAsync(HttpMethod httpMethod, string path, object parameters, Type resultType, object userState)
        {
            Stream input;
            bool containsEtag;
            bool processBatchResponse = false;
            var httpHelper = PrepareRequest(httpMethod, path, parameters, resultType, out input, out containsEtag, out processBatchResponse);
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
                    FacebookApiEventArgs args;
                    if (e.Cancelled)
                    {
                        args = new FacebookApiEventArgs(e.Error, true, userState, null);
                    }
                    else if (e.Error == null)
                    {
                        string responseString = null;

                        try
                        {
                            using (var stream = e.Result)
                            {
#if WINRT
                                bool compressed = false;
                                var contentEncoding = httpHelper.HttpWebResponse.Headers[HttpRequestHeader.ContentEncoding];
                                if (contentEncoding != null)
                                {
                                    if (contentEncoding.IndexOf("gzip") != -1)
                                    {
                                        using (var uncompressedStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress))
                                        {
                                            using (var reader = new StreamReader(uncompressedStream))
                                            {
                                                responseString = reader.ReadToEnd();
                                            }
                                        }

                                        compressed = true;
                                    }
                                    else if (contentEncoding.IndexOf("deflate") != -1)
                                    {
                                        using (var uncompressedStream = new System.IO.Compression.DeflateStream(stream, System.IO.Compression.CompressionMode.Decompress))
                                        {
                                            using (var reader = new StreamReader(uncompressedStream))
                                            {
                                                responseString = reader.ReadToEnd();
                                            }
                                        }

                                        compressed = true;
                                    }
                                }

                                if (!compressed)
                                {
                                    using (var reader = new StreamReader(stream))
                                    {
                                        responseString = reader.ReadToEnd();
                                    }
                                }
#else
                                using (var reader = new StreamReader(stream))
                                {
                                    responseString = reader.ReadToEnd();
                                }
#endif
                            }

                            try
                            {
                                object result = ProcessResponse(httpHelper, responseString, resultType, containsEtag, processBatchResponse);
                                args = new FacebookApiEventArgs(null, false, userState, result);
                            }
                            catch (Exception ex)
                            {
                                args = new FacebookApiEventArgs(ex, false, userState, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            args = httpHelper.HttpWebRequest.IsCancelled ? new FacebookApiEventArgs(ex, true, userState, null) : new FacebookApiEventArgs(ex, false, userState, null);
                        }
                    }
                    else
                    {
                        var webEx = e.Error as WebExceptionWrapper;
                        if (webEx == null)
                        {
                            args = new FacebookApiEventArgs(e.Error, httpHelper.HttpWebRequest.IsCancelled, userState, null);
                        }
                        else
                        {
                            if (webEx.GetResponse() == null)
                            {
                                args = new FacebookApiEventArgs(webEx, false, userState, null);
                            }
                            else
                            {
                                var response = httpHelper.HttpWebResponse;
                                if (response.StatusCode == HttpStatusCode.NotModified)
                                {
                                    var jsonObject = new JsonObject();
                                    var headers = new JsonObject();

                                    foreach (var headerName in response.Headers.AllKeys)
                                        headers[headerName] = response.Headers[headerName];

                                    jsonObject["headers"] = headers;
                                    args = new FacebookApiEventArgs(null, false, userState, jsonObject);
                                }
                                else
                                {
                                    httpHelper.OpenReadAsync();
                                    return;
                                }
                            }
                        }
                    }

                    OnCompleted(httpMethod, args);
                };

            if (input == null)
            {
                httpHelper.OpenReadAsync();
            }
            else
            {
                // we have a request body so write
                httpHelper.OpenWriteCompleted +=
                    (o, e) =>
                    {
                        FacebookApiEventArgs args;
                        if (e.Cancelled)
                        {
                            input.Dispose();
                            args = new FacebookApiEventArgs(e.Error, true, userState, null);
                        }
                        else if (e.Error == null)
                        {
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

                                httpHelper.OpenReadAsync();
                                return;
                            }
                            catch (Exception ex)
                            {
                                args = new FacebookApiEventArgs(ex, httpHelper.HttpWebRequest.IsCancelled, userState, null);
                            }
                            finally
                            {
                                input.Dispose();
                            }
                        }
                        else
                        {
                            input.Dispose();
                            var webExceptionWrapper = e.Error as WebExceptionWrapper;
                            if (webExceptionWrapper != null)
                            {
                                var ex = webExceptionWrapper;
                                if (ex.GetResponse() != null)
                                {
                                    httpHelper.OpenReadAsync();
                                    return;
                                }
                            }

                            args = new FacebookApiEventArgs(e.Error, false, userState, null);
                        }

                        OnCompleted(httpMethod, args);
                    };

                httpHelper.OpenWriteAsync();
            }
        }

        #region Events

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
#endif
        protected virtual void OnGetCompleted(FacebookApiEventArgs args)
        {
            if (GetCompleted != null)
                GetCompleted(this, args);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
#endif
        protected virtual void OnPostCompleted(FacebookApiEventArgs args)
        {
            if (PostCompleted != null)
                PostCompleted(this, args);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
#endif
        protected virtual void OnDeleteCompleted(FacebookApiEventArgs args)
        {
            if (DeleteCompleted != null)
                DeleteCompleted(this, args);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
#endif
        protected void OnUploadProgressChanged(FacebookUploadProgressChangedEventArgs args)
        {
            if (UploadProgressChanged != null)
                UploadProgressChanged(this, args);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
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

#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use GetTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(string path)
        {
            GetAsync(path, null, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use GetTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(object parameters)
        {
            GetAsync(null, parameters, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use GetTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(string path, object parameters)
        {
            GetAsync(path, parameters, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use GetTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(string path, object parameters, object userState)
        {
            ApiAsync(HttpMethod.Get, path, parameters, null, userState);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use PostTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void PostAsync(object parameters)
        {
            PostAsync(null, parameters, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use PostTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void PostAsync(string path, object parameters)
        {
            PostAsync(path, parameters, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use PostTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void PostAsync(string path, object parameters, object userState)
        {
            ApiAsync(HttpMethod.Post, path, parameters, null, userState);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete("Use DeleteTaskAsync instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void DeleteAsync(string path)
        {
            DeleteAsync(path, null, null);
        }

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