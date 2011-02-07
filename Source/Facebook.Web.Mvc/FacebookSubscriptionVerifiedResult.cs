namespace Facebook.Web.Mvc
{
    using System.Web.Mvc;

    public class FacebookSubscriptionVerifiedResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            // Make result
            context.HttpContext.Response.Write(context.HttpContext.Request.Params["hub.challenge"]);
        }
    }
}
