namespace Facebook
{
    using System;
    using System.Collections.Specialized;
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

            webClient.DownloadDataCompleted += DownloadDataCompleted;
            webClient.UploadDataCompleted += UploadDataCompleted;
        }

        public WebHeaderCollection Headers
        {
            get { return webClient.Headers; }
            set { webClient.Headers = value; }
        }

        public NameValueCollection QueryString
        {
            get { return webClient.QueryString; }
            set { webClient.QueryString = value; }
        }

        public WebHeaderCollection ResponseHeaders
        {
            get { return webClient.ResponseHeaders; }
        }

        public IWebProxy Proxy
        {
            get { return webClient.Proxy; }
            set { webClient.Proxy = value; }
        }

        public byte[] DownloadData(Uri address)
        {
            try
            {
                return webClient.DownloadData(address);
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
                return webClient.UploadData(address, method, data);
            }
            catch (WebException webException)
            {
                throw new WebExceptionWrapper(webException);
            }
        }

        public void DownloadDataAsync(Uri address, object userToken)
        {
            webClient.DownloadDataAsync(address, userToken);
        }

        public void UploadDataAsync(Uri address, string method, byte[] data, object userToken)
        {
            webClient.UploadDataAsync(address, method, data, userToken);
        }

        public void CancelAsync()
        {
            webClient.CancelAsync();
        }

        public event DownloadDataCompletedEventHandler DownloadDataCompleted;
        public event UploadDataCompletedEventHandler UploadDataCompleted;

        public void Dispose()
        {
            webClient.Dispose();
        }
    }
}