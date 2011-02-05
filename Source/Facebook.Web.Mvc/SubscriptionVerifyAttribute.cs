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

            if (request.HttpMethod == "GET")
            {
                if (request.Params["hub.mode"] == "subscribe")
                {
                    if (request.Params["hub.verify_token"] != this.VerificationToken)
                    {
                        modelState.AddModelError("hub.verify_token", "Invalid verification token.");
                    }
                }
                else
                {
                    modelState.AddModelError("HUB_MODE", "Invalid hub mode");
                }
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        /*
       public override void OnActionExecuting(ActionExecutingContext filterContext)
       {
           var request = filterContext.HttpContext.Request;
           if (request.HttpMethod == "GET" && request.Params["hub.mode"] == "subscribe" &&
               request.Params["hub.verify_token"] == this.VerificationToken)
           {
               filterContext.Result = new SubscriptionVerifiedResult();
           }
           else if (request.HttpMethod == "POST" && !string.IsNullOrEmpty(FacebookContext.Current.AppSecret))
           {
               if (request.Params.AllKeys.Contains("HTTP_X_HUB_SIGNATURE"))
               {
                   // signatures looks somewhat like "sha1=4594ae916543cece9de48e3289a5ab568f514b6a"
                   var signature = request.Params["HTTP_X_HUB_SIGNATURE"];

                   if (!string.IsNullOrEmpty(signature) && signature.StartsWith("sha1=") && signature.Length > 5)
                   {
                       signature = signature.Substring(5);

                       var reader = new System.IO.StreamReader(request.InputStream);
                       var jsonString = reader.ReadToEnd();

                       var sha1 = FacebookWebUtils.ComputeHmacSha1Hash(Encoding.UTF8.GetBytes(jsonString), Encoding.UTF8.GetBytes(FacebookContext.Current.AppSecret));

                       var hashString = new StringBuilder();
                       foreach (var b in sha1)
                       {
                           hashString.Append(b.ToString("x2"));
                       }

                       if (signature == hashString.ToString())
                       {
                           var jsonObject = JsonSerializer.DeserializeObject(jsonString);
                           filterContext.ActionParameters.Add("subscription", jsonObject);
                       }
                   }
               }
           }
       }
   */
    }
}
