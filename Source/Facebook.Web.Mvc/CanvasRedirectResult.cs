// --------------------------------
// <copyright file="FacebookRedirectResult.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System.Web.Mvc;

namespace Facebook.Web.Mvc
{
    /// <summary>
    /// Controls the processing of application actions by redirecting to a specified canvas iFrame URI.
    /// </summary>
    public class CanvasRedirectResult : RedirectResult
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRedirectResult"/> class.
        /// </summary>
        /// <param name="url">The target URL.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="url"/> parameter is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public CanvasRedirectResult(string url) : base(url) { }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context"/> parameter is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public override void ExecuteResult(ControllerContext context)
        {
            var content = CanvasUrlBuilder.GetCanvasRedirectHtml(this.Url);

            context.Controller.TempData.Keep();

            context.HttpContext.Response.ContentType = "text/html";
            context.HttpContext.Response.Write(content);
        }
    }
}
