using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using Facebook.Web;
using System.Web.Security;

namespace CSASPNETWebsite
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var authorizer = new FacebookAuthorizer();

            if (!authorizer.IsAuthorized())
            {
                Response.Redirect("~/Account/Login.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery));
            }

            var fb = new FacebookClient(authorizer.Session.AccessToken);

            dynamic me = fb.Get("/me");
            lblName.Text = me.name;
        }
    }
}