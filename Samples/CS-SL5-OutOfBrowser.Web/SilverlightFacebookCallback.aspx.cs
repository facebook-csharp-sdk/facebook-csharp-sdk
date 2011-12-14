using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;

namespace CS_SL5_OutOfBrowser.Web
{
    public partial class SilverlightFacebookCallback : System.Web.UI.Page
    {
        private const string AppId = ""
        private const string AppSecret = ""
        private const string _silverlightFacebookCallback = "http://localhost:60259/SilverlightFacebookCallback.aspx";

        protected string AccessToken { get; private set; }
        protected string ErrorDescription { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var url = HttpContext.Current.Request.Url;
            FacebookOAuthResult oauthResult;

            var oauthClient = new FacebookOAuthClient { AppId = AppId, AppSecret = AppSecret, RedirectUri = new Uri(_silverlightFacebookCallback) };

            if (oauthClient.TryParseResult(url, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    dynamic result = oauthClient.ExchangeCodeForAccessToken(oauthResult.Code);
                    AccessToken = result.access_token;
                }
                else
                {
                    ErrorDescription = oauthResult.ErrorDescription;
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }
            else
            {
                Response.Redirect("~/");
            }
        }
    }
}