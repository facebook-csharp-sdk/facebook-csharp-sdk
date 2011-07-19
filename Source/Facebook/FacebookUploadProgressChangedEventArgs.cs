// --------------------------------
// <copyright file="FacebookApiUploadProgressChangedEventArgs.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System.ComponentModel;

    /// <summary>
    /// Represents Facebook api upload progress changed event args.
    /// </summary>
    public class FacebookUploadProgressChangedEventArgs : ProgressChangedEventArgs
    {
        private readonly long _received;
        private readonly long _sent;
        private readonly long _totalRecived;
        private readonly long _totalSend;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookUploadProgressChangedEventArgs"/> class.
        /// </summary>
        /// <param name="bytesReceived">Bytes received.</param>
        /// <param name="totalBytesToReceive">Total bytes to receive.</param>
        /// <param name="bytesSent">Bytes sent.</param>
        /// <param name="totalBytesToSend">Total bytes to send.</param>
        /// <param name="progressPercentage">Progress percentage.</param>
        /// <param name="userToken">User token.</param>
        public FacebookUploadProgressChangedEventArgs(long bytesReceived, long totalBytesToReceive, long bytesSent, long totalBytesToSend, int progressPercentage, object userToken)
            : base(progressPercentage, userToken)
        {
            _received = bytesReceived;
            _totalRecived = totalBytesToReceive;
            _sent = bytesSent;
            _totalSend = totalBytesToSend;
        }

        /// <summary>
        /// Bytes received.
        /// </summary>
        public long BytesReceived
        {
            get { return _received; }
        }

        /// <summary>
        /// Total bytes to receive.
        /// </summary>
        public long TotalBytesToReceive
        {
            get { return _totalRecived; }
        }

        /// <summary>
        /// Bytes sent.
        /// </summary>
        public long BytesSent
        {
            get { return _sent; }
        }

        /// <summary>
        /// Total bytes to send.
        /// </summary>
        public long TotalBytesToSend
        {
            get { return _totalSend; }
        }
    }

}