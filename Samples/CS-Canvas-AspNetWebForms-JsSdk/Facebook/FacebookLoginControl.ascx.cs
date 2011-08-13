using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CS_Canvas_AspNetWebForms_JsSdk.Facebook
{
    public partial class FacebookLoginControl : System.Web.UI.UserControl
    {
        protected string[] ExtendedPermissions = ConfigurationManager.AppSettings["extendedPermissions"].Split(',');

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}