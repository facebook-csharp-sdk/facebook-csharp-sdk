using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook.Web;

namespace Facebook.Samples.AspWebsite
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var fbApp = new FacebookApp();
            Authorizer auth = new Authorizer(fbApp);
            auth.Perms = "email,offline_access,manage_pages";
            if (!auth.IsAuthorized())
            {
                this.Response.Redirect("~/Connect.aspx?returnUrl=/Profile.aspx");
            }
            LoadProfile(fbApp);
        }

        protected void LoadProfile(FacebookApp fbApp)
        {
            try
            {
                dynamic result = fbApp.Api("/me");
                LabelFirstName.Text = result.first_name;
            }
            catch (FacebookOAuthException ex)
            {
                // Invalid access token, reauthenticate.
                this.Response.Redirect("~/Connect.aspx?returnUrl=/Profile.aspx");
            }
        }
    }
}