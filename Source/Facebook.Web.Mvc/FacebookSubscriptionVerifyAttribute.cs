namespace Facebook.Web.Mvc
{
    using System.Web.Mvc;

    public class FacebookSubscriptionVerifyAttribute : ActionFilterAttribute
    {
        public string VerificationToken { get; set; }

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
