namespace Facebook.Tests.FakeWebClients
{
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Text;

    public class FakeWebClientForDownloadData : IWebClient
    {
        private readonly byte[] returnData;

        public FakeWebClientForDownloadData(byte[] returnData)
        {
            this.returnData = returnData;
        }

        public void Dispose()
        {
        }

        public WebHeaderCollection Headers
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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
            throw new NotImplementedException();
        }
    }
}