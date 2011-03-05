namespace Facebook.Tests.FakeWebClients
{
    using System;
    using System.Collections.Specialized;
    using System.Net;

    internal class FakeWebClientForDownloadAndUploadData : IWebClient
    {
        private readonly byte[] returnData;
        private WebHeaderCollection headers;

        public FakeWebClientForDownloadAndUploadData(byte[] returnData)
        {
            this.returnData = returnData;
            this.headers = new WebHeaderCollection();
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
            return this.returnData;
        }

        public void CancelAsync()
        {
            throw new NotImplementedException();
        }

        public Action<object, DownloadDataCompletedEventArgsWrapper> DownloadDataCompleted { get; set; }

        public Action<object, UploadDataCompletedEventArgsWrapper> UploadDataCompleted { get; set; }

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
            return this.returnData;
        }
    }
}