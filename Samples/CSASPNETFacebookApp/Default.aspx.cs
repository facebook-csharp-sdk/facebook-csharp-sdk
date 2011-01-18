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
        var canvasSettings = new CanvasSettings
                                 {
                                     CanvasPageUrl = new Uri("http://apps.facebook.com/csharpsamples/"),
                                     CanvasUrl = new Uri("http://localhost/csharpsamples/")
                                 };

        var fbSettings = new FacebookSettings
                             {
                                 AppId = "124973200873702",
                                 AppSecret = "3b4a872617be2ae1932baa1d4d240272"
                             };

        var context = new HttpContextWrapper(HttpContext.Current);
        var canvasUrlBuilder = new Facebook.Web.New.CanvasUrlBuilder(canvasSettings, context.Request);
        var auth = new Facebook.Web.New.CanvasAuthorizer(fbSettings, canvasSettings, context) { Perms = this.requiredAppPermissions };

        var loginUrl = canvasUrlBuilder.GetLoginUrl(fbSettings, null, null, null);

        a.Text = "Raw url: " + context.Request.RawUrl
                 + "<br/>Original url: " + context.Request.Url.OriginalString
                 + "<br/>Path and Query: " + context.Request.Url.PathAndQuery
                 + "<br/>Application Path: " + context.Request.ApplicationPath
                 + "<br/>Canvas Url: " + canvasUrlBuilder.CanvasUrl
                 + "<br/>Current Canvas Url: " + canvasUrlBuilder.CurrentCanvasUrl
                 + "<br/>Canvas Page: " + canvasUrlBuilder.CanvasPage
                 + "<br/>Current Canvas Page: " + canvasUrlBuilder.CurrentCanvasPage
                 + "<br/>Canvas Application Path: " + canvasUrlBuilder.CanvasPageApplicationPath
                 + "<br/>Current Canvas Path and Query: " + canvasUrlBuilder.CurrentCanvasPathAndQuery
                 + "<br/>GetLoginUrl: " + auth.GetLoginUrl(null)
                 + "<br/>Login url: " + loginUrl;

        if (auth.Authorize())
        {
            fbApp = new FacebookApp();
            fbApp.Session = auth.Session;
            ShowFacebookContent();
        }

        //fbApp = new FacebookApp();
        //authorizer = new CanvasAuthorizer(fbApp);
        //authorizer.Perms = requiredAppPermissions;
        //if (authorizer.Authorize())
        //{
        //    ShowFacebookContent();
        //}
    }

    private void ShowFacebookContent()
    {
        dynamic myInfo = fbApp.Get("me");
        lblName.Text = myInfo.name;
        pnlHello.Visible = true;
    }

}