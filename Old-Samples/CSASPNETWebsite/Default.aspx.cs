using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using Facebook.Web;

namespace CSASPNETWebsite
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckIfFacebookAppIsSetupCorrectly();

            var fbWebContext = FacebookWebContext.Current;
            if (fbWebContext.IsAuthorized())
            {
                var fb = new FacebookWebClient(fbWebContext);
                dynamic result = fb.Get("/me");

                lblMessage.Text = "Hi " + result.name;
            }
        }

        internal static void CheckIfFacebookAppIsSetupCorrectly()
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
                    current.AppSecret != "{app secret}")
                {
                    isSetup = true;
                }
            }

            if (!isSetup)
            {
                HttpContext.Current.Response.Redirect("~/GettingStarted.aspx");
            }
        }
    }
}
