using System;
using System.Web.Mvc;
using Facebook;
using Facebook.Web.Mvc;

namespace FacebookDemo.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [CanvasAuthorize(Perms = "user_about_me")]
        public ActionResult About()
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