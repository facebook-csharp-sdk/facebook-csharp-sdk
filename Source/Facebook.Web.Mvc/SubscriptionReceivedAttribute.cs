namespace Facebook.Web.Mvc
{
    using System.Linq;
    using System.Web.Mvc;

    public class SubscriptionReceivedAttribute : ActionFilterAttribute
    {
        public string VerificationToken { get; set; }

        public SubscriptionReceivedAttribute(string verificationToken)
        {
            this.VerificationToken = verificationToken;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.ContentType = "text/plain";

            var request = filterContext.HttpContext.Request;
            var modelState = filterContext.Controller.ViewData.ModelState;

            if (request.HttpMethod == "POST")
            {
                if (request.Params.AllKeys.Contains(FacebookWebUtils.HTTP_X_HUB_SIGNATURE_KEY))
                {
                    var appSecret = FacebookContext.Current.AppSecret;
                    if (!string.IsNullOrEmpty(appSecret))
                    {
                        // signatures looks somewhat like "sha1=4594ae916543cece9de48e3289a5ab568f514b6a"
                        var originalSignature = request.Params[FacebookWebUtils.HTTP_X_HUB_SIGNATURE_KEY];

                        var reader = new System.IO.StreamReader(request.InputStream);
                        var jsonString = reader.ReadToEnd();

                        if (FacebookWebUtils.VerifyHttpXHubSignature(appSecret, originalSignature, jsonString))
                        {
                            var jsonObject = JsonSerializer.DeserializeObject(jsonString);
                            filterContext.ActionParameters["subscription"] = jsonObject;
                        }
                        else
                        {
                            modelState.AddModelError("HTTP_X_HUB_SIGNATURE", "Invalid verification token.");
                        }
                    }
                    else
                    {
                        modelState.AddModelError("FacebookContext.Current.AppSecret", "FacebookContext.Current.AppSecret not defined.");
                    }
                }
                else
                {
                    modelState.AddModelError("HTTP_X_HUB_SIGNATURE", "HTTP_X_HUB_SIGNATURE not found");
                }
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

    }
}