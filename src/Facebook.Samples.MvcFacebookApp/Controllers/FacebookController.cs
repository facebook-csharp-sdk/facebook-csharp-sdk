// --------------------------------
// <copyright file="FacebookController.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System.Dynamic;
using System.Web.Mvc;
using Facebook.Web.Mvc.Canvas;

namespace Facebook.Samples.MvcFacebookApp.Controllers
{

    [HandleError]
    public class FacebookController : Controller
    {

        public PartialViewResult LoginButton()
        {
            //ViewData["LoginUrl"] = App.GetAppLoginUrl("/Home/Install");
            return PartialView();
        }

        public PartialViewResult InitScript()
        {
            FacebookApp app = new FacebookApp();
            dynamic model = new ExpandoObject();
            model.AppId = app.AppId;
            model.CookieSupport = app.CookieSupport;
            return PartialView(model);
        }

    }
}
