namespace Facebook
{
    using System;
    using System.ComponentModel;

    internal class UploadDataCompletedEventArgsWrapper : AsyncCompletedEventArgs
    {
        private readonly byte[] _result;

        public UploadDataCompletedEventArgsWrapper(Exception error, bool cancelled, object userState, byte[] result)
            : base(error, cancelled, userState)
        {
            result = result;
        }

        public byte[] Result
        {
            get { return _result; }
        }
    }
}