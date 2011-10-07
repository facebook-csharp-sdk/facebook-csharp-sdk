// --------------------------------
// <copyright file="FacebookSubscriptionsHttpHandler.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System.Diagnostics.CodeAnalysis;
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
            var subContext = new FacebookSubscriptionContext
                                 {
                                     HttpContext = new HttpContextWrapper(context),
                                     FacebookApplication = FacebookApplication.Current,
                                 };

            OnVerifying(subContext);
            VerifyCore(subContext);
        }

        /// <summary>
        /// Set your verification token here.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public abstract void OnVerifying(FacebookSubscriptionContext context);

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContextWrapper"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public virtual void VerifyCore(FacebookSubscriptionContext context)
        {
            context.HttpContext.Response.ContentType = "text/plain";
            var request = context.HttpContext.Request;

            string errorMessage;

            switch (request.HttpMethod)
            {
                case "GET":
                    if (!string.IsNullOrEmpty(context.VerificationToken) &&
                        FacebookSubscriptionVerifier.VerifyGetSubscription(request, context.VerificationToken, out errorMessage))
                    {
                        context.HttpContext.Response.Write(request.Params["hub.challenge"]);
                    }
                    else
                    {
                        HandleUnauthorizedRequest(context);
                    }

                    break;
                case "POST":
                    {
                        var reader = new System.IO.StreamReader(request.InputStream);
                        var jsonString = reader.ReadToEnd();

                        var secret = context.FacebookApplication.AppSecret;
                        if (!string.IsNullOrEmpty(secret) &&
                            FacebookSubscriptionVerifier.VerifyPostSubscription(request, secret, jsonString, out errorMessage))
                        {
                            // might need to put try catch when desterilizing object
                            var jsonObject = JsonSerializer.Current.DeserializeObject(jsonString);
                            ProcessSubscription(context, jsonObject);
                        }
                        else
                        {
                            HandleUnauthorizedRequest(context);
                        }
                    }

                    break;
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
        public abstract void ProcessSubscription(FacebookSubscriptionContext context, object result);

        /// <summary>
        /// Handles unauthorized requests.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public virtual void HandleUnauthorizedRequest(FacebookSubscriptionContext context)
        {
            context.HttpContext.Response.StatusCode = 401;
        }
    }

    /// <summary>
    /// Represents the subscription context.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public class FacebookSubscriptionContext
    {
        /// <summary>
        /// Gets or sets the verify_token.
        /// </summary>
        public string VerificationToken { get; set; }

        /// <summary>
        /// Gets or sets the Facebook application.
        /// </summary>
        public IFacebookApplication FacebookApplication { get; set; }

        /// <summary>
        /// Gets or sets the HttpContext.
        /// </summary>
        public HttpContextBase HttpContext { get; set; }
    }
}