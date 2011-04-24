// --------------------------------
// <copyright file="IWebClient.cs" company="Thuzi LLC (www.thuzi.com)">
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