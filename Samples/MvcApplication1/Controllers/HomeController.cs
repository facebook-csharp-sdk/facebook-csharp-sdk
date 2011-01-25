namespace MvcApplication1.Controllers
{
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web.Mvc;

    [FacebookApp("CSharpSamples")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            this.ViewData["fbApplications"] = FacebookSdk.Applications;

            return View();
        }

        [NCanvasAuthorize(Permissions = "user_about_me")]
        public ActionResult RegisteredFacebookApps()
        {
            return View(FacebookSdk.Applications);
        }

    }
}
