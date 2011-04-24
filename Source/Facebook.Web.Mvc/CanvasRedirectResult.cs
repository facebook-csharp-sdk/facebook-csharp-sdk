// --------------------------------
// <copyright file="CanvasRedirectResult.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Controls the processing of application actions by redirecting to a specified canvas iFrame URI.
    /// </summary>
    public class CanvasRedirectResult : RedirectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRedirectResult"/> class.
        /// </summary>
        /// <param name="url">
        /// The target url.
        /// </param>
        public CanvasRedirectResult(string url)
            : base(url)
        {
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context"/> parameter is null.</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            var content = CanvasUrlBuilder.GetCanvasRedirectHtml(new Uri(Url));

            context.Controller.TempData.Keep();

            context.HttpContext.Response.ContentType = "text/html";
            context.HttpContext.Response.Write(content);
        }
    }
}