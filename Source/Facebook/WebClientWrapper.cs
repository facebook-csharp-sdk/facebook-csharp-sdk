
namespace Facebook
{
    using System;
    using System.Net;

    internal class WebClientWrapper : IWebClient
    {
        private WebClient webClient = new WebClient();

        public WebClientWrapper()
            : this(new WebClient())
        {
        }

        public WebClientWrapper(WebClient webClient)
        {
            this.webClient = webClient;

#if SILVERLIGHT
            this.webClient.UploadStringCompleted +=
#else
            this.webClient.UploadDataCompleted +=
#endif
                (o, e) =>
                {
                    if (this.UploadDataCompleted == null)
                    {
                        return;
                    }

                    if (e == null)
                    {
                        this.UploadDataCompleted(o, null);
                    }
                    else
                    {
                        byte[] result = null;
                        var error = e.Error;

                        if (error == null)
                        {
#if SILVERLIGHT
                            if(!string.IsNullOrEmpty(e.Result))
                            {
                                result = System.Text.Encoding.UTF8.GetBytes(e.Result);
                            }
#else
                            result = e.Result;
#endif
                        }
                        else if (error is WebException)
                        {
                            error = new WebExceptionWrapper((WebException)error);
                        }

                        this.UploadDataCompleted(o, new UploadDataCompletedEventArgsWrapper(error, e.Cancelled, e.UserState, result));
                    }
                };

#if SILVERLIGHT
            this.webClient.DownloadStringCompleted +=
#else
            this.webClient.DownloadDataCompleted +=
#endif

                (o, e) =>
                {
                    if (this.DownloadDataCompleted == null)
                    {
                        return;
                    }

                    if (e == null)
                    {
                        this.DownloadDataCompleted(o, null);
                    }
                    else
                    {
                        byte[] result = null;
                        var error = e.Error;

                        if (error == null)
                        {
#if SILVERLIGHT
                            if(!string.IsNullOrEmpty(e.Result))
                            {
                                result = System.Text.Encoding.UTF8.GetBytes(e.Result);
                            }
#else
                            result = e.Result;
#endif
                        }
                        else if (error is WebException)
                        {
                            error = new WebExceptionWrapper((WebException)error);
                        }

                        this.DownloadDataCompleted(o, new DownloadDataCompletedEventArgsWrapper(error, e.Cancelled, e.UserState, result));
                    }
                };
        }

        public WebHeaderCollection Headers
        {
            get { return this.webClient.Headers; }
            set { this.webClient.Headers = value; }
        }

#if !SILVERLIGHT
        public IWebProxy Proxy
        {
            get { return this.webClient.Proxy; }
            set { this.webClient.Proxy = value; }
        }

        public byte[] DownloadData(Uri address)
        {
            try
            {
                return this.webClient.DownloadData(address);
            }
            catch (WebException webException)
            {
                throw new WebExceptionWrapper(webException);
            }
        }

        public byte[] UploadData(Uri address, string method, byte[] data)
        {
            try
            {
                return this.webClient.UploadData(address, method, data);
            }
            catch (WebException webException)
            {
                throw new WebExceptionWrapper(webException);
            }
        }
#endif

        public void DownloadDataAsync(Uri address, object userToken)
        {
#if SILVERLIGHT
            this.webClient.DownloadStringAsync(address, userToken);
#else
            this.webClient.DownloadDataAsync(address, userToken);
#endif
        }

        public void UploadDataAsync(Uri address, string method, byte[] data, object userToken)
        {
#if SILVERLIGHT
            string str = null;
            if (data != null)
            {
                str = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
            }

            this.webClient.UploadStringAsync(address, method, str, userToken);
#else
            this.webClient.UploadDataAsync(address, method, data, userToken);
#endif
        }

        public void CancelAsync()
        {
            webClient.CancelAsync();
        }

        public Action<object, DownloadDataCompletedEventArgsWrapper> DownloadDataCompleted { get; set; }

        public Action<object, UploadDataCompletedEventArgsWrapper> UploadDataCompleted { get; set; }

        public void Dispose()
        {
#if !SILVERLIGHT
            webClient.Dispose();
#endif
        }
    }
}