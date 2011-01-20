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
        var auth = new CanvasAuthorizer();

        if (auth.Authorize())
        {
            ShowFacebookContent();
        }
    }

    private void ShowFacebookContent()
    {
        var fb = new FacebookApp(this.CurrentSession);
        dynamic myInfo = fb.Get("me");
        lblName.Text = myInfo.name;
        pnlHello.Visible = true;
    }

}