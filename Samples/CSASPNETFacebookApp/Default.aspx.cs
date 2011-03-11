using System;
using System.Configuration;
using System.Web.UI;
using Facebook;
using Facebook.Web;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckIfFacebookAppIsSetupCorrectly();

        var auth = new FacebookCanvasAuthorizer { Permissions = new[] { "user_about_me" } };

        if (auth.Authorize())
        {
            ShowFacebookContent();
        }
    }

    private void ShowFacebookContent()
    {
        var fb = new FacebookWebClient();
        dynamic myInfo = fb.Get("me");
        lblName.Text = myInfo.name;
        pnlHello.Visible = true;

        /* OR */

        //var fbApp = new FacebookWebApp();
        //dynamic info = fbApp.Get("me");
        //lblName.Text = info.name;
        //pnlHello.Visible = true;
    }

    private void CheckIfFacebookAppIsSetupCorrectly()
    {
        // YOU DONT NEED ANY OF THIS IN YOUR APPLICATION
        // THIS METHOD JUST CHECKS TO SEE IF YOU HAVE SETUP
        // THE SAMPLE. IF THE SAMPLE IS NOT SETUP YOU WILL
        // BE SENT TO THE GETTING STARTED PAGE.

        bool isSetup = false;
        var settings = ConfigurationManager.GetSection("facebookSettings");
        if (settings != null)
        {
            var current = settings as IFacebookApplication;
            if (current.AppId != "{app id}" &&
                current.AppSecret != "{app secret}" &&
                current.CanvasPage != "http://apps.facebook.com/{fix path}/")
            {
                isSetup = true;
            }
        }

        if (!isSetup)
        {
            Response.Redirect("~/GettingStarted.aspx");
        }
    }
}