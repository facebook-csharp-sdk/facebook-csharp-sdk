// --------------------------------
// <copyright file="FacebookSubscriptionVerifyAttribute.cs" company="Thuzi LLC (www.thuzi.com)">
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
    /// Represents Facebook Subscription Verifiy attribute.
    /// </summary>
    public class FacebookSubscriptionVerifyAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The verification token.
        /// </summary>
        public virtual string VerificationToken { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookSubscriptionVerifyAttribute"/> class.
        /// </summary>
        /// <param name="verificationToken"></param>
        public FacebookSubscriptionVerifyAttribute(string verificationToken)
        {
            VerificationToken = verificationToken;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.ContentType = "text/plain";
            var request = filterContext.HttpContext.Request;
            var modelState = filterContext.Controller.ViewData.ModelState;

            string errorMessage;

            if (request.HttpMethod == "GET")
            {
                if (string.IsNullOrEmpty(VerificationToken))
                {
                    errorMessage = "Verification Token is empty.";
                }
                else
                {
                    if (FacebookSubscriptionVerifier.VerifyGetSubscription(request, VerificationToken, out errorMessage))
                    {
                        return;
                    }
                }
            }
            else
            {
                errorMessage = "Invalid http method.";
            }

            modelState.AddModelError("facebook-subscription", errorMessage);

            filterContext.HttpContext.Response.StatusCode = 401;
        }
    }
}
