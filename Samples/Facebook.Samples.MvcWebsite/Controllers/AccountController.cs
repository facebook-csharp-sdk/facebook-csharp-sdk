using System.Web.Mvc;

namespace Facebook.Samples.MvcWebsite.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {

        public ActionResult Index(string perms)
        {
            ViewData["perms"] = perms;
            return View("Permissions");
        }

        // **************************************
        // URL: /Account/Connect
        // **************************************

        public ActionResult Connect(string returnUrl)
        {
            FacebookApp app = new FacebookApp();
            if (app.Session != null)
            {

                // Here is where you can check if the user has logged 
                // in before. You can redirect the user to a page asking 
                // them to register or just save their userid.

                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
}
