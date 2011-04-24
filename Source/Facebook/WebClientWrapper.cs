// --------------------------------
// <copyright file="WebClientWrapper.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Net;

    internal class WebClientWrapper : IWebClient
    {
        private readonly WebClient _webClient = new WebClient();

        public WebClientWrapper()
            : this(new WebClient())
        {
        }

        public WebClientWrapper(WebClient webClient)
        {
            _webClient = webClient;

#if SILVERLIGHT
            _webClient.UploadStringCompleted +=
#else
            _webClient.UploadDataCompleted +=
#endif
                (o, e) =>
                {
                    if (UploadDataCompleted == null)
                    {
                        return;
                    }

                    if (e == null)
                    {
                        UploadDataCompleted(o, null);
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

                        UploadDataCompleted(o, new UploadDataCompletedEventArgsWrapper(error, e.Cancelled, e.UserState, result));
                    }
                };

#if SILVERLIGHT
            _webClient.DownloadStringCompleted +=
#else
            _webClient.DownloadDataCompleted +=
#endif

                (o, e) =>
                {
                    if (DownloadDataCompleted == null)
                    {
                        return;
                    }

                    if (e == null)
                    {
                        DownloadDataCompleted(o, null);
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

                        DownloadDataCompleted(o, new DownloadDataCompletedEventArgsWrapper(error, e.Cancelled, e.UserState, result));
                    }
                };
        }

        public WebHeaderCollection Headers
        {
            get { return _webClient.Headers; }
            set { _webClient.Headers = value; }
        }

#if !SILVERLIGHT
        public IWebProxy Proxy
        {
            get { return _webClient.Proxy; }
            set { _webClient.Proxy = value; }
        }

        public byte[] DownloadData(Uri address)
        {
            try
            {
                return _webClient.DownloadData(address);
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
                return _webClient.UploadData(address, method, data);
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
            _webClient.DownloadStringAsync(address, userToken);
#else
            _webClient.DownloadDataAsync(address, userToken);
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

            _webClient.UploadStringAsync(address, method, str, userToken);
#else
            _webClient.UploadDataAsync(address, method, data, userToken);
#endif
        }

        public void CancelAsync()
        {
            _webClient.CancelAsync();
        }

        public Action<object, DownloadDataCompletedEventArgsWrapper> DownloadDataCompleted { get; set; }

        public Action<object, UploadDataCompletedEventArgsWrapper> UploadDataCompleted { get; set; }

        public void Dispose()
        {
#if !SILVERLIGHT
            _webClient.Dispose();
#endif
        }
    }
}