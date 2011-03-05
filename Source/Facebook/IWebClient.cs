namespace Facebook
{
    using System;
    using System.Collections.Specialized;
    using System.Net;

    internal interface IWebClient : IDisposable
    {
        WebHeaderCollection Headers { get; set; }

        NameValueCollection QueryString { get; set; }

        WebHeaderCollection ResponseHeaders { get; }

        IWebProxy Proxy { get; set; }

        byte[] DownloadData(Uri address);

        byte[] UploadData(Uri address, string method, byte[] data);

        void DownloadDataAsync(Uri address, object userToken);

        void UploadDataAsync(Uri address, string method, byte[] data, object userToken);

        void CancelAsync();

        Action<object, DownloadDataCompletedEventArgsWrapper> DownloadDataCompleted { get; set; }

        Action<object, UploadDataCompletedEventArgsWrapper> UploadDataCompleted { get; set; }
    }
}