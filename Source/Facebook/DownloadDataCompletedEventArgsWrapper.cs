namespace Facebook
{
    using System;
    using System.ComponentModel;

    internal class DownloadDataCompletedEventArgsWrapper : AsyncCompletedEventArgs
    {
        private readonly byte[] result;

        public DownloadDataCompletedEventArgsWrapper(Exception error, bool cancelled, object userState, byte[] result)
            : base(error, cancelled, userState)
        {
            this.result = result;
        }

        public byte[] Result
        {
            get { return this.result; }
        }
    }
}