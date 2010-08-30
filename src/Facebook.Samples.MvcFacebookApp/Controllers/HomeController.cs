// --------------------------------
// <copyright file="HomeController.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook.Web.Mvc;
using Facebook.Web.Mvc.Canvas;

namespace Facebook.Samples.MvcFacebookApp.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {

        
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }


        public ActionResult About()
        {
            return this.CanvasRedirectToAction("Index");
        }

    }
}
