namespace MvcApplication1.Controllers
{
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //[NCanvasAuthorize(Permissions = "user_about_me", CancelUrlPath = "/", ReturnUrlPath = "/")]
        //[NCanvasAuthorize(Permissions = "user_about_me")]
        [CanvasSoftAuthorize(Permissions = "user_about_me")]
        public ActionResult CurrentFacebookContext()
        {
            return View(FacebookContext.Current);
        }

    }
}
