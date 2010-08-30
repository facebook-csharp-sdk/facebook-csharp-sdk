using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Facebook.Samples.AspNetWebApplication.Account {
    public partial class Logout : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            FormsAuthentication.SignOut();
            Response.Redirect("~/");
        }
    }
}