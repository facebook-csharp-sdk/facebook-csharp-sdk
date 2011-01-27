namespace CSMvc3FacebookApp.Controllers
{
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web;
    using Facebook.Web.Mvc;

    [HandleError]
    public class HomeController : Controller
    {
        public FacebookSession FacebookSession
        {
            get { return (new CanvasAuthorizer().Session); }
        }
        public ActionResult Index()
        {
            return View();
        }

        [CanvasAuthorize(Permissions = "user_about_me")]
        public ActionResult About()
        {
            FacebookApp fbApp = new FacebookApp(this.FacebookSession.AccessToken);

            dynamic result = fbApp.Get("me");

            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;


            return View();
        }
    }
}
