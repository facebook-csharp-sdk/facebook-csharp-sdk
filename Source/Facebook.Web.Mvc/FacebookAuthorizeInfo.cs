// --------------------------------
// <copyright file="FacebookAuthorizeInfo.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Web.Routing;

    /// <summary>
    /// Represents the authorization info needed to access the currently request resource.
    /// </summary>
    public sealed class FacebookAuthorizeInfo
    {
        private RouteValueDictionary _routeValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthorizeInfo"/> class.
        /// </summary>
        public FacebookAuthorizeInfo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthorizeInfo"/> class.
        /// </summary>
        /// <param name="authorizeUrl">The authorize URL.</param>
        /// <param name="permissions">The permissions.</param>
        /// <param name="isCancelUrl">if set to <c>true</c> [is cancel URL].</param>
        /// <param name="routeValues">The route values.</param>
        public FacebookAuthorizeInfo(Uri authorizeUrl, string permissions, bool isCancelUrl, RouteValueDictionary routeValues)
        {
            AuthorizeUrl = authorizeUrl;
            Permissions = permissions;
            IsCancelReturn = isCancelUrl;
            _routeValues = routeValues;
        }

        /// <summary>
        /// Gets or sets the authorize URL.
        /// </summary>
        /// <value>The authorize URL.</value>
        public Uri AuthorizeUrl { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public string Permissions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a cancel return.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is a cancel return; otherwise, <c>false</c>.
        /// </value>
        public bool IsCancelReturn { get; set; }

        /// <summary>
        /// Gets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public RouteValueDictionary RouteValues
        {
            get { return _routeValues ?? (_routeValues = new RouteValueDictionary()); }
        }

    }
}
