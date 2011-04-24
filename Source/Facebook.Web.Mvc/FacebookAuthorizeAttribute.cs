// --------------------------------
// <copyright file="FacebookAuthorizeAttribute.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Web.Mvc;

    public class FacebookAuthorizeAttribute : FacebookAuthorizeAttributeBase
    {
        public string LoginUrl { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication facebookApplication)
        {
            var authorizer = new FacebookWebContext(facebookApplication, filterContext.HttpContext);

            if (!string.IsNullOrEmpty(Permissions) && Permissions.IndexOf(" ") != -1)
            {
                throw new ArgumentException("Permissions cannot contain whitespace.");
            }

            if (!authorizer.IsAuthorized(ToArrayString(Permissions)))
            {
                filterContext.Result = new RedirectResult(this.LoginUrl ?? "/");
            }
        }
    }
}