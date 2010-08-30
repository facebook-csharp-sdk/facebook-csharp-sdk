using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Facebook.Samples.AspNetWebApplication.Profile {
    public partial class Index : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            // https://graph.facebook.com/me

            FacebookApp app = new FacebookApp();
            dynamic result = app.Api("me");
            name.Text = result.name;
            about.Text = result.about;
        }
    }
}