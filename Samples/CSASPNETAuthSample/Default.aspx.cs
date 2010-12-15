using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;

public partial class _Default : CanvasPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!authorizer.IsAuthorized())
        {
            var authurl = authorizer.GetLoginUrl(new HttpRequestWrapper(Request));
            CanvasRedirect(authurl.ToString());
        }
        else
        {
            LoggedIn();
        }
    }

    private void LoggedIn()
    {
        dynamic myInfo = fbApp.Get("me");
        lblName.Text = myInfo.name;
        pnlHello.Visible = true;
    }
}