using System;
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

}