using System;
using System.Configuration;
using System.Linq;
using System.Web;
using Facebook.Web;

namespace CS_AspNetWebForms_JsSdk.Facebook
{
    public partial class Logon : System.Web.UI.Page
    {
        protected string[] ExtendedPermissions = ConfigurationManager.AppSettings["extendedPermissions"].Split(',');

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FacebookWebContext.Current.IsAuthorized(ExtendedPermissions))
            {
                if (Request.QueryString.AllKeys.Contains("ReturnUrl"))
                {
                    var returnUrl = Request.QueryString["ReturnUrl"];

                    // prevent open redirection attacks
                    if (IsUrlLocalToHost(Request, returnUrl))
                    {
                        Response.Redirect(returnUrl);
                        return;
                    }
                }

                Response.Redirect("~/");
            }
        }

        public static bool IsUrlLocalToHost(HttpRequest request, string url)
        {
            // http://www.asp.net/mvc/tutorials/preventing-open-redirection-attacks
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            Uri absoluteUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                return String.Equals(request.Url.Host, absoluteUri.Host,
                            StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                bool isLocal = !url.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
                    && !url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
                    && Uri.IsWellFormedUriString(url, UriKind.Relative);
                return isLocal;
            }
        }
    }
}