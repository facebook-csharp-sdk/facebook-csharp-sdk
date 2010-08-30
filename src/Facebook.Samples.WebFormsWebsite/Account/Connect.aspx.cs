using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Facebook.Samples.AspNetWebApplication.Account {
    public partial class Connect : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            FacebookApp app = new FacebookApp();
            if (app.Session != null) {
                FormsAuthentication.SetAuthCookie(app.UserId.ToString(), false);

                // Here is where you can check if the user has logged 
                // in before. You can redirect the user to a page asking 
                // them to register or just save their userid.

                if (Request.QueryString.AllKeys.Contains("returnUrl")) {
                    string returnUrl = Request.QueryString["returnUrl"];
                    Response.Redirect(returnUrl);
                } else {
                    Response.Redirect("~/Profile/Index.aspx");
                }
            }
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}