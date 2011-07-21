// --------------------------------
// <copyright file="HttpWebRequestCreatedEventArgs.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

#if TPL

namespace Facebook
{
    using System;

    class HttpWebRequestCreatedEventArgs : EventArgs
    {
        private readonly object _userToken;
        private readonly HttpWebRequestWrapper _httpWebRequestWrapper;

        public HttpWebRequestCreatedEventArgs(object userToken, HttpWebRequestWrapper httpWebRequestWrapper)
        {
            _userToken = userToken;
            _httpWebRequestWrapper = httpWebRequestWrapper;
        }

        public HttpWebRequestWrapper HttpWebRequest
        {
            get { return _httpWebRequestWrapper; }
        }

        public object UserState
        {
            get { return _userToken; }
        }
    }
}

#endif