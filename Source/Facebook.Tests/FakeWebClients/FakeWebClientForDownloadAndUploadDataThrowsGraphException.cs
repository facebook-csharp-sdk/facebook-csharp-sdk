namespace Facebook.Tests.FakeWebClients
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;

    internal class FakeWebClientForDownloadAndUploadDataThrowsGraphException : IWebClient
    {
        private readonly WebExceptionWrapper exception;
        private WebHeaderCollection headers;

        public FakeWebClientForDownloadAndUploadDataThrowsGraphException(WebExceptionWrapper exception)
        {
            this.exception = exception;
            this.headers = new WebHeaderCollection();
        }

        public FakeWebClientForDownloadAndUploadDataThrowsGraphException(Stream stream)
            : this(new FakeWebException(stream))
        {
        }

        public FakeWebClientForDownloadAndUploadDataThrowsGraphException(string json)
            : this(new FakeWebException(json))
        {
        }

        public void Dispose()
        {
        }

        public WebHeaderCollection Headers
        {
            get { return this.headers; }
            set { this.headers = value; }
        }

        public NameValueCollection QueryString
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public WebHeaderCollection ResponseHeaders
        {
            get { throw new NotImplementedException(); }
        }

        public IWebProxy Proxy
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public byte[] DownloadData(Uri address)
        {
            throw this.exception;
        }

        public void CancelAsync()
        {
            throw new NotImplementedException();
        }

        public event DownloadDataCompletedEventHandler DownloadDataCompleted;
        public event UploadDataCompletedEventHandler UploadDataCompleted;

        public void UploadDataAsync(Uri address, string method, byte[] data, object userToken)
        {
            throw new NotImplementedException();
        }

        public void DownloadDataAsync(Uri address, object userToken)
        {
            throw new NotImplementedException();
        }

        public byte[] UploadData(Uri address, string method, byte[] data)
        {
            throw this.exception;
        }
    }
}