namespace Facebook.Web.Mvc
{
    using System.Web.Mvc;

    public class SubscriptionVerifyAttribute : ActionFilterAttribute
    {
        public string VerificationToken { get; set; }

        public SubscriptionVerifyAttribute(string verificationToken)
        {
            this.VerificationToken = verificationToken;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.ContentType = "text/plain";
            var request = filterContext.HttpContext.Request;
            var modelState = filterContext.Controller.ViewData.ModelState;

            string errorMessage;

            if (request.HttpMethod == "GET")
            {
                if (string.IsNullOrEmpty(this.VerificationToken))
                {
                    errorMessage = "Verification Token is empty.";
                }
                else
                {
                    if (FacebookWebUtils.VerifyGetSubscription(request, this.VerificationToken, out errorMessage))
                    {
                        return;
                    }
                }
            }
            else
            {
                errorMessage = "Invalid http method.";
            }

            modelState.AddModelError("facebook-c#-sdk", errorMessage);

            filterContext.HttpContext.Response.StatusCode = 401;
        }
    }
}
