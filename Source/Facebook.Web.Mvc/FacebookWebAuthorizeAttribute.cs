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
    using System;
    using System.ComponentModel;

    public class FacebookWebAuthorizeAttribute : FacebookAuthorizeAttributeBase
    {
        public string LoginUrl { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication facebookApplication)
        {
            var authorizer = new FacebookWebContext(facebookApplication, filterContext.HttpContext);

            if (!authorizer.IsAuthorized(this.Permissions))
            {
                filterContext.Result = new RedirectResult(this.LoginUrl ?? "/");
            }
        }
    }

    [Obsolete("Use FacebookWebAuthorizeAttribute instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FacebookAuthorizeAttributeBase : FacebookWebAuthorizeAttribute
    {

    }
}