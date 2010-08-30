// --------------------------------
// <copyright file="CanvasAuthorizeAttribute.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Configuration;
using System.Web.Security;
using System.Dynamic;
using System.Web.Routing;

namespace Facebook.Web.Mvc.Canvas
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CanvasAuthorizeAttribute : FacebookAuthorizeAttribute
    {
        public CanvasAuthorizeAttribute() : base() { }

        public CanvasAuthorizeAttribute(FacebookApp app) :base(app) { }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var url = GetLoginUrl(filterContext);
            filterContext.Result = new CanvasRedirectResult(url);
        }

    }
}
