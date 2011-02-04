using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;

namespace CSASPNETWebsiteRegistrationForm
{
    public partial class fbregcallback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params.AllKeys.Contains("signed_request"))
            {
                var result = FacebookSignedRequest.Parse(FacebookContext.Current.AppSecret, Request.Params["signed_request"]);
                dynamic signedRequestValue = result.Data;
                this.RegistrationData = signedRequestValue.registration;
            }
            else
            {
                // there is no signed request here.
                Response.Redirect("~/");
            }
        }

        protected dynamic RegistrationData { get; private set; }
    }
}