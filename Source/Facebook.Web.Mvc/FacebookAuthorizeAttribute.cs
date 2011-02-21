﻿// --------------------------------
// <copyright file="FacebookAuthorizeAttribute.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System.Web.Mvc;

    public class FacebookAuthorizeAttribute : FacebookAuthorizeAttributeBase
    {
        public string LoginUrl { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication facebookApplication)
        {
            var authorizer = new FacebookAuthorizer(facebookApplication, filterContext.HttpContext) { Perms = this.Perms };

            if (!authorizer.IsAuthorized())
            {
                filterContext.Result = new RedirectResult(this.LoginUrl ?? "/");
            }
        }
    }
}