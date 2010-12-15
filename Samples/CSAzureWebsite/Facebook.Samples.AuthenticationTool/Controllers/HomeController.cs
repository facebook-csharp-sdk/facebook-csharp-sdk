using System;
using System.Web.Mvc;
using Facebook;
using Facebook.Web.Mvc;

namespace Auth_And_Allow.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [CanvasAuthorize(Perms = "user_about_me")]
        public ActionResult Index()
        {
            FacebookApp fbApp = new FacebookApp();
            if (fbApp.Session != null)
            {
                dynamic result = fbApp.Get("me");

                ViewData["Firstname"] = result.first_name;
                ViewData["Lastname"] = result.last_name;
            }

            return View();
        }
    }
}