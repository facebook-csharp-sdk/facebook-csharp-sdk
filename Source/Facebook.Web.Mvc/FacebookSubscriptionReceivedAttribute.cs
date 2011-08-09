// --------------------------------
// <copyright file="FacebookSubscriptionReceivedAttribute.cs" company="Thuzi LLC (www.thuzi.com)">
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
    /// Represents the Facebook subscription received attribute.
    /// </summary>
    public class FacebookSubscriptionReceivedAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The parameter name.
        /// </summary>
        public virtual string ParameterName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.ContentType = "text/plain";
            var request = filterContext.HttpContext.Request;
            var modelState = filterContext.Controller.ViewData.ModelState;
            var appSecret = FacebookApplication.Current.AppSecret;

            var parameterName = ParameterName;
            if (string.IsNullOrEmpty(parameterName))
            {
                parameterName = "subscription";
            }

            filterContext.ActionParameters[parameterName] = null;

            string errorMessage;
            if (request.HttpMethod == "POST")
            {
                if (string.IsNullOrEmpty(appSecret))
                {
                    errorMessage = "FacebookContext.Current.AppSecret is null or empty.";
                }
                else
                {
                    var reader = new System.IO.StreamReader(request.InputStream);
                    var jsonString = reader.ReadToEnd();

                    if (FacebookSubscriptionVerifier.VerifyPostSubscription(request, appSecret, jsonString, out errorMessage))
                    {
                        var jsonObject = JsonSerializer.Current.DeserializeObject(jsonString);
                        filterContext.ActionParameters[parameterName] = jsonObject;

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