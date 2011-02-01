namespace Facebook.Web.Mvc
{
    using System.Web.Mvc;

    public class FacebookAuthorizeAttribute : FacebookAuthorizeAttributeBase
    {
        public string LoginUrl { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext, IFacebookApplication facebookApplication)
        {
            var authorizer = new Authorizer(facebookApplication, filterContext.HttpContext) { Perms = this.Permissions };

            if (!authorizer.IsAuthorized())
            {
                filterContext.Result = new RedirectResult(this.LoginUrl ?? "/");
            }
        }
    }
}