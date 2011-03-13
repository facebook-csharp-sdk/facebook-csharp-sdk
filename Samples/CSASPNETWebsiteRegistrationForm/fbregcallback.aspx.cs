using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using Facebook.Web;

namespace CSASPNETWebsiteRegistrationForm
{
    public partial class fbregcallback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var fbApp = new FacebookApp();
            var signedRequest = fbApp.SignedRequest;
            if (signedRequest == null)
            {
                // there is no signed request here.
                Response.Redirect("~/");
            }
            else
            {
                dynamic jsonSignedRequest = signedRequest.Data;
                this.RegistrationData = jsonSignedRequest.registration;
            }
        }

        protected dynamic RegistrationData { get; private set; }
    }
}