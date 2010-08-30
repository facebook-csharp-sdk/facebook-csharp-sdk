// --------------------------------
// <copyright file="FacebookRedirectResult.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Facebook.Web.Mvc.Canvas
{
    public class CanvasRedirectResult : RedirectResult
    {

        public CanvasRedirectResult(string url) : base(url) { }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var content = FacebookUrlBuilder.GetCanvasRedirectHtml(this.Url);

            context.Controller.TempData.Keep();

            context.HttpContext.Response.ContentType = "text/html";
            context.HttpContext.Response.Write(content);
        }
    }
}
