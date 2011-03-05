namespace Facebook
{
    using System;
    using System.ComponentModel;

    internal class UploadDataCompletedEventArgsWrapper : AsyncCompletedEventArgs
    {
        private readonly byte[] result;

        public UploadDataCompletedEventArgsWrapper(Exception error, bool cancelled, object userState, byte[] result)
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