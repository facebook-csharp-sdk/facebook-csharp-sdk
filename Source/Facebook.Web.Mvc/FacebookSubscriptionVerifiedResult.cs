// --------------------------------
// <copyright file="FacebookSubscriptionVerifiedResult.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System.Web.Mvc;

    /// <summary>
    /// Represents the Facebook subscription verified result.
    /// </summary>
    public class FacebookSubscriptionVerifiedResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            // Make result
            context.HttpContext.Response.Write(context.HttpContext.Request.Params["hub.challenge"]);
        }
    }
}
