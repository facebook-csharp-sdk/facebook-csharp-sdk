namespace Facebook
{
    using System;
    using System.Net;

    internal interface IWebClient : IDisposable
    {
        WebHeaderCollection Headers { get; set; }

#if !SILVERLIGHT

        IWebProxy Proxy { get; set; }

        byte[] DownloadData(Uri address);

        byte[] UploadData(Uri address, string method, byte[] data);

#endif

        void DownloadDataAsync(Uri address, object userToken);

        void UploadDataAsync(Uri address, string method, byte[] data, object userToken);

        void CancelAsync();

        Action<object, DownloadDataCompletedEventArgsWrapper> DownloadDataCompleted { get; set; }

        Action<object, UploadDataCompletedEventArgsWrapper> UploadDataCompleted { get; set; }
    }
}