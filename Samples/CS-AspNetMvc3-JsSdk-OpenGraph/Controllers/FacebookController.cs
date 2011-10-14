using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook.Web.Mvc;
using Facebook;
using Facebook.Web;
using System.Configuration;

namespace CS_AspNetMvc3_JsSdk_OpenGraph.Controllers
{
    public class FacebookController : Controller
    {
        //
        // GET: /Facebook/

        public ActionResult Cookie(string id)
        {
            ViewBag.IsAuthorized = FacebookWebContext.Current.IsAuthorized();
            return View();
        }

        [HttpPost]
        [FacebookAuthorize(LoginUrl = "/Facebook/LogOn?ReturnUrl=~/Facebook/Cookie")]
        public ActionResult PostCook()
        {
            var ns = ConfigurationManager.AppSettings["FBAppNamespace"];

            var fb = new FacebookWebClient();
            dynamic result = fb.Post(string.Format("/me/{0}:{1}", ns, "cook"),
                new Dictionary<string, object> {
                    {"recipe", Url.Action("Cookie", "Facebook", null, Request.Url.Scheme)}
                });

            return RedirectToAction("Cookie", new { id = result.id });
        }

        //
        // GET: /Facebook/LogOn
        public ActionResult LogOn(string returnUrl)
        {
            var fbWebContext = new FacebookWebContext(FacebookApplication.Current, ControllerContext.HttpContext); // or FacebookWebContext.Current;

            if (fbWebContext.IsAuthorized())
            {
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return new RedirectResult(returnUrl);
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

    }
}
