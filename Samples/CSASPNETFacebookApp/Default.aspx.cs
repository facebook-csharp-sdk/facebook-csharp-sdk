using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using Facebook.Web;

public partial class _Default : Page
{
    protected string requiredAppPermissions = "user_about_me";
    protected FacebookApp fbApp;
    protected CanvasAuthorizer authorizer;

    protected void Page_Load(object sender, EventArgs e)
    {
        fbApp = new FacebookApp();
        authorizer = new CanvasAuthorizer(fbApp);
        authorizer.Perms = requiredAppPermissions;
        if (authorizer.Authorize())
        {
            ShowFacebookContent();
        }
    }

    private void ShowFacebookContent()
    {
        dynamic myInfo = fbApp.Get("me");
        lblName.Text = myInfo.name;
        pnlHello.Visible = true;
    }

}