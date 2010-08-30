using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Facebook.Web.Mvc
{
    public sealed class FacebookAuthorizeInfo
    {
        public string AuthorizeUrl { get; set; }
        public string Perms { get; set; }
        public bool IsCancelReturn { get; set; }
        public RouteValueDictionary RouteValues { get; set; }

    }
}
