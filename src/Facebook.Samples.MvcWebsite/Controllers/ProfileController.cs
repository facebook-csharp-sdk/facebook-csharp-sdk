using System;
using System.Dynamic;
using System.Web.Mvc;
using Facebook.Web.Mvc;

namespace Facebook.Samples.MvcWebsite.Controllers
{
    [CanvasAuthorize(Perms = "email")]
    [HandleError]
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {

            // https://graph.facebook.com/me

            FacebookApp app = new FacebookApp();
            dynamic result = app.Api("me");
            return View("Profile", result);
        }

        public ActionResult SpecifyFields()
        {

            // https://graph.facebook.com/me?fields=id,name,picture

            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.fields = "id,name,about";
            dynamic result = app.Api("me", parameters);
            return View("Profile", result);
        }

        public ActionResult RestFacebookPage()
        {

            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.page_ids = "85158793417";
            parameters.method = "pages.getInfo";
            parameters.fields = "name";
            dynamic result = app.Api(parameters);
            return View("FacebookPage", result);
        }

        public ActionResult GetPermission()
        {
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.scope = "publish_stream";
            Uri url = app.GetLoginUrl(parameters);
            return Redirect(url.ToString());
        }

        public ActionResult WallPost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WallPost(string message)
        {
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.message = message;
            // parameters.picture
            // parameters.link
            // parameters.name 
            // parameters.caption
            // parameters.description
            dynamic result = app.Api(string.Format("/{0}/feed", app.UserId), parameters, HttpMethod.Post);
            return Content("posted");
        }
    }
}
