// --------------------------------
// <copyright file="UploadDataCompletedEventArgsWrapper.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

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
            _result = result;
        }

        public byte[] Result
        {
            get { return _result; }
        }
    }
}