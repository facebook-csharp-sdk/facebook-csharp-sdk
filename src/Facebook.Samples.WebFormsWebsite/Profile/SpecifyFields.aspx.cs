using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Dynamic;

namespace Facebook.Samples.AspNetWebApplication.Profile {
    public partial class SpecifyFields : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            // https://graph.facebook.com/me?fields=id,name,picture

            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.fields = "id,name,about";
            dynamic result = app.Api("me", parameters);
            name.Text = result.name;
            about.Text = result.about;

        }
    }
}