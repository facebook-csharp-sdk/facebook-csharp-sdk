using System;
using System.Collections.Generic;
using System.Web;
using Facebook;

namespace CS_SL4_InBrowser.Web
{
    public partial class SilverlightFacebookCallback : System.Web.UI.Page
    {
        private const string AppId = ""
        private const string AppSecret = ""
        private const string _silverlightFacebookCallback = "http://localhost:1530/SilverlightFacebookCallback.aspx";

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
                    var result = (IDictionary<string, object>)oauthClient.ExchangeCodeForAccessToken(oauthResult.Code);
                    AccessToken = result["access_token"].ToString();
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