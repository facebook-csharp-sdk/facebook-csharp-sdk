using System;
using System.Web.Routing;

namespace Facebook.Web.Mvc
{
    public sealed class FacebookAuthorizeInfo
    {
        private RouteValueDictionary _routeValues;


        public FacebookAuthorizeInfo() { }

        public FacebookAuthorizeInfo(Uri authorizeUrl, string permissions, bool isCancelUrl, RouteValueDictionary routeValues)
        {
            this.AuthorizeUrl = authorizeUrl;
            this.Permissions = permissions;
            this.IsCancelReturn = isCancelUrl;
            this._routeValues = routeValues;
        }

        public Uri AuthorizeUrl { get; set; }
        public string Permissions { get; set; }
        public bool IsCancelReturn { get; set; }
        public RouteValueDictionary RouteValues
        {
            get
            {
                if (_routeValues == null)
                {
                    _routeValues = new RouteValueDictionary();
                }
                return _routeValues;
            }
        }

    }
}
