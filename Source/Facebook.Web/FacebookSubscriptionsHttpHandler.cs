// --------------------------------
// <copyright file="FacebookSubscriptionsHttpHandler.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Represents the base class for handling incoming Facebook subscriptions.
    /// </summary>
    /// <remarks>
    /// For more information visit http://developers.facebook.com/docs/api/realtime
    /// 1. Getting the Facebook app access token. 
    ///     var oauth = new FacebookOAuthClient { ClientId = "...", ClientSecret = "..." };
    ///     dynamic result = oauth.GetApplicationAccessToken();
    ///     var fb = new FacebookClient(result.access_token);
    /// 2. Subscribing to subscriptions:
    ///     dynamic result = fb.Post(
    ///                string.Format("/{0}/subscriptions", appId), new Dictionary&lt;string, object>
    ///                                                                {
    ///                                                                    { "object","user"},
    ///                                                                    { "fields","name,picture,feed" },
    ///                                                                    { "callback_url", "http://localhost/fbsubscriptions.ashx" },
    ///                                                                    { "verify_token", "abc" }
    ///                                                                });
    ///    dynamic result2 = fb.Post(
    ///          string.Format("/{0}/subscriptions", appId), new Dictionary&lt;string, object>
    ///                                                            {
    ///                                                                { "object","permissions"},
    ///                                                                { "fields","email,read_stream,offline_access" },
    ///                                                                { "callback_url", "http://localhost/fbsubscriptions.ashx" },
    ///                                                                { "verify_token", "abc" }
    ///                                                            });
    /// 3. receiving and processing subscriptions.
    ///     ProcessSubscription is called when the subscription is received.
    ///     result contains the JsonObject.
    /// </remarks>
    //[ContractClass(typeof(FacebookSubscriptionsHttpHandlerCodeContacts))]
    public abstract class FacebookSubscriptionsHttpHandler : IHttpHandler
    {

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.Request != null);
            Contract.Requires(context.Request.Params != null);
            Contract.Requires(context.Response != null);
            var subContext = new SubscriptionContext()
            {
                HttpContext = new HttpContextWrapper(context),
                FacebookApplication = FacebookContext.Current,
            };
            this.OnVerifying(subContext);
            this.VerifyCore(subContext);
        }

        public abstract void OnVerifying(SubscriptionContext context);

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContextWrapper"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public virtual void VerifyCore(SubscriptionContext context)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.HttpContext.Request != null);
            Contract.Requires(context.HttpContext.Request.Params != null);
            Contract.Requires(context.HttpContext.Response != null);

            context.HttpContext.Response.ContentType = "text/plain";
            var request = context.HttpContext.Request;

            if (request.HttpMethod == "GET" && request.Params["hub.mode"] == "subscribe" &&
                request.Params["hub.verify_token"] == context.VerificationToken)
            {
                context.HttpContext.Response.Write(request.Params["hub.challenge"]);
            }
            else if (request.HttpMethod == "POST" && !string.IsNullOrEmpty(context.FacebookApplication.AppSecret))
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

                        var sha1 = FacebookWebUtils.ComputeHmacSha1Hash(Encoding.UTF8.GetBytes(jsonString), Encoding.UTF8.GetBytes(context.FacebookApplication.AppSecret));

                        var hashString = new StringBuilder();
                        foreach (var b in sha1)
                        {
                            hashString.Append(b.ToString("x2"));
                        }

                        if (signature == hashString.ToString())
                        {
                            var jsonObject = JsonSerializer.DeserializeObject(jsonString);
                            this.ProcessSubscription(context, jsonObject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process the subscription.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public abstract void ProcessSubscription(SubscriptionContext context, object result);

        public virtual void HandleUnauthorizedRequest(SubscriptionContext context)
        {
            context.HttpContext.Response.StatusCode = 401;
        }
    }
    /*
    [ContractClassFor(typeof(FacebookSubscriptionsHttpHandler))]
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here."),
     SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
         Justification = "Reviewed. Suppression is OK here.")]
    internal abstract class FacebookSubscriptionsHttpHandlerCodeContacts : FacebookSubscriptionsHttpHandler
    {
        public override string VerificationToken
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return default(string);
            }
        }

        public override void ProcessSubscription(HttpContextWrapper context, object result)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.Request != null);
            Contract.Requires(context.Request.Params != null);
            Contract.Requires(context.Response != null);
        }
    }
    */

    public class SubscriptionContext
    {
        public string VerificationToken { get; set; }
        public IFacebookApplication FacebookApplication { get; set; }
        public HttpContextBase HttpContext { get; set; }
    }
}