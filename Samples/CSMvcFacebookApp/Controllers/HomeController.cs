namespace FacebookDemo.Web.Controllers
{
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web;
    using Facebook.Web.Mvc;

    [HandleError]
    public class HomeController : Controller
    {
        public FacebookSession CurrentSession
        {
            get { return (new CanvasAuthorizer()).Session; }
        }

        public ActionResult Index()
        {
            return View();
        }

        [CanvasAuthorize(Permissions = "user_about_me")]
        public ActionResult About()
        {
            var fbApp = new FacebookApp(this.CurrentSession.AccessToken);

            dynamic result = fbApp.Get("me");
            ViewData["Firstname"] = (string)result.first_name;
            ViewData["Lastname"] = (string)result.last_name;

            return View();
        }
    }
}