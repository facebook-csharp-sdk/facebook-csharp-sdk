// --------------------------------
// <copyright file="FacebookAsyncResult.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;

namespace Facebook
{
    public delegate void FacebookAsyncCallback(FacebookAsyncResult ar);

    public class FacebookAsyncResult : IAsyncResult
    {
        dynamic result;
        object asyncState;
        System.Threading.WaitHandle asyncWaitHandle;
        bool completedSynchronously;
        bool isCompleted;
        FacebookApiException error;

        public FacebookAsyncResult(
            dynamic result,
            object asyncState,
            System.Threading.WaitHandle asyncWaitHandle,
            bool completedSynchronously,
            bool isCompleted,
            FacebookApiException error)
        {
            this.result = result;
            this.asyncState = asyncState;
            this.asyncWaitHandle = asyncWaitHandle;
            this.completedSynchronously = completedSynchronously;
            this.isCompleted = isCompleted;
            this.error = error;
        }

        public FacebookApiException Error
        {
            get { return this.error; }
        }

        public dynamic Result
        {
            get { return this.result; }
        }

        public object AsyncState
        {
            get { return this.asyncState; }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { return this.asyncWaitHandle; }
        }

        public bool CompletedSynchronously
        {
            get { return this.completedSynchronously; }
        }

        public bool IsCompleted
        {
            get { return this.isCompleted; }
        }
    }
}
