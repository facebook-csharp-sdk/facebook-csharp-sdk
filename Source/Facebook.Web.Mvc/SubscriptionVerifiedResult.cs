using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Facebook.Web.Mvc
{
    public class SubscriptionVerifiedResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            // Make result
            context.HttpContext.Response.Write(context.HttpContext.Request.Params["hub.challenge"]);
        }
    }
}
