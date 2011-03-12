using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Facebook;
using Mvc3Website.Models;

namespace Mvc3Website.Controllers
{
    public class AccountController : Controller
    {
#if DEBUG
        const string logoffUrl = "http://localhost:5000/";
        const string redirectUrl = "http://localhost:5000/account/oauth/";
        private const string appId = "{appId}";
        private const string appSecret = "{appSecret}";
#else
        const string logoffUrl = "http://www.example.com/";
        const string redirectUrl = "http://www.example.com/account/oauth/";
#endif

        public ActionResult LogOn(string returnUrl)
        {
            var oAuthClient = new FacebookOAuthClient();
            oAuthClient.AppId = appId;
            oAuthClient.RedirectUri = new Uri(redirectUrl);
            var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object> { { "state", returnUrl } });
            return Redirect(loginUri.AbsoluteUri);
        }

        public ActionResult OAuth(string code, string state)
        {
            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(Request.Url, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    var oAuthClient = new FacebookOAuthClient();
                    oAuthClient.AppId = appId;
                    oAuthClient.AppSecret = appSecret;
                    oAuthClient.RedirectUri = new Uri(redirectUrl);
                    dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
                    string accessToken = tokenResult.access_token;

                    DateTime expiresOn = DateTime.MaxValue;

                    if (tokenResult.ContainsKey("expires"))
                    {
                        DateTimeConvertor.FromUnixTime(tokenResult.expires);
                    }

                    FacebookClient fbClient = new FacebookClient(accessToken);
                    dynamic me = fbClient.Get("me?fields=id,name");
                    long facebookId = Convert.ToInt64(me.id);

                    InMemoryUserStore.Add(new FacebookUser
                    {
                        AccessToken = accessToken,
                        Expires = expiresOn,
                        FacebookId = facebookId,
                        Name = (string)me.name,
                    });

                    FormsAuthentication.SetAuthCookie(facebookId.ToString(), false);

                    // prevent open redirection attack by checking if the url is local.
                    if (Url.IsLocalUrl(state))
                    {
                        return Redirect(state);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            var oAuthClient = new FacebookOAuthClient();
            oAuthClient.RedirectUri = new Uri(logoffUrl);
            var logoutUrl = oAuthClient.GetLogoutUrl();
            return Redirect(logoutUrl.AbsoluteUri);
        }

    }
}
