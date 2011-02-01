using System;
using System.Configuration;
using System.Web.UI;
using Facebook;
using Facebook.Web;

public partial class _Default : Page
{
    /// <summary>
    /// Gets the current canvas facebook session.
    /// </summary>
    public FacebookSession CurrentSession
    {
        get { return (new CanvasAuthorizer()).Session; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckIfFacebookAppIsSetupCorrectly();

        var auth = new CanvasAuthorizer { Perms = "user_about_me" };

        if (auth.Authorize())
        {
            ShowFacebookContent();
        }
    }

    private void ShowFacebookContent()
    {
        var fb = new FacebookApp(this.CurrentSession.AccessToken);
        dynamic myInfo = fb.Get("me");
        lblName.Text = myInfo.name;
        pnlHello.Visible = true;
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
                current.CanvasUrl != "http://apps.facebook.com/{fix this path}/")
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