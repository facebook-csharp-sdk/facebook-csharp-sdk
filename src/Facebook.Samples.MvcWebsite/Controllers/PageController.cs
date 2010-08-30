using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using System.IO;

namespace Facebook.Samples.MvcWebsite.Controllers
{
    public class PageController : Controller
    {


        public ActionResult PublishStreamRest()
        {
            return View("PublishStream");
        }

        [HttpPost]
        public ActionResult PublishStreamRest(string uid, string message, string linkText, string linkUrl, string privacy)
        {
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.method = "stream.publish";
            parameters.message = message;
            parameters.uid = uid; //page id

            if (Request.Files.AllKeys.Contains("attachment"))
            {
                HttpPostedFileBase file = Request.Files["attachment"];
                StreamReader reader = new StreamReader(file.InputStream);
                parameters.attachment = reader.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(linkUrl))
            {
                dynamic link = new ExpandoObject();
                link.text = linkText;
                link.href = linkUrl;
                parameters.action_links = new object[] { link };
            }
            if (!string.IsNullOrEmpty(privacy))
            {
                parameters.privacy = privacy;
            }
            dynamic result = app.Api(parameters);
            return this.Content(result.ToString());
        }

        public ActionResult PublishStreamGraph(string message)
        {
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.message = message;
            // parameters.picture
            // parameters.link
            // parameters.name 
            // parameters.caption
            // parameters.description
            dynamic result = app.Api(string.Format("/{0}/feed", "129896133710529"), parameters, HttpMethod.Post);
            return Content(result.ToString());
        }

    }
}
