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
        const string redirectUrl = "http://localhost:5000/account/oauth/";
#else
        const string redirectUrl = "http://www.example.com/account/oauth/";
#endif

        public ActionResult LogOn(string returnUrl)
        {
            var oAuthClient = new FacebookOAuthClient();
            oAuthClient.RedirectUri = new Uri(redirectUrl);
            var loginUri = oAuthClient.GetLoginUrl();
            return Redirect(loginUri.ToString());
        }

        public ActionResult OAuth(string code, string state)
        {
            var oAuthClient = new FacebookOAuthClient();
            oAuthClient.RedirectUri = new Uri(redirectUrl);
            dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
            string accessToken = tokenResult.access_token;

            DateTime expiresOn = tokenResult.expires;

            FacebookClient fbClient = new FacebookClient(accessToken);
            dynamic me = fbClient.Get("me?fields=id,name,first_name,last_name");
            long facebookId = Convert.ToInt64(me.id);

            FormsAuthentication.SetAuthCookie(facebookId.ToString(), false);
            return Redirect(state);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

    }
}
