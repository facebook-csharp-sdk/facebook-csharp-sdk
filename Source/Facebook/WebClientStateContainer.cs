// --------------------------------
// <copyright file="WebClientStateContainer.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;

    internal class WebClientStateContainer
    {
        public object UserState { get; set; }
        public HttpMethod Method { get; set; }
        public Uri RequestUri { get; set; }
        public bool IsBatchRequest { get; set; }
    }
}
