using System;
using Facebook.Web;

namespace CS_AspNetWebForms_RegistrationForm.Facebook
{
    public partial class RegistrationCallback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var signedRequest = FacebookWebContext.Current.SignedRequest;
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