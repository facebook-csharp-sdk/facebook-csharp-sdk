using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using Facebook.Web;
using System.Web.Security;

namespace CSASPNETWebsite.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _Default.CheckIfFacebookAppIsSetupCorrectly();

            var authorizer = new FacebookAuthorizer();

            if (authorizer.IsAuthorized())
            {
                // check if return url is local.
                Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["returnUrl"] ?? "/"));
            }
        }
    }
}
