namespace MvcApplication1
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Facebook;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // filters.Add(new Facebook.Web.Mvc.FacebookAppAttribute("CSharpSamples"));
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        private static void RegisterFacebookApplications(FacebookAppSettingsCollection applications)
        {
            applications.Register("CSharpSamples", new FacebookAppSettings("124973200873702", "3b4a872617be2ae1932baa1d4d240272") { CanvasPage = "http://apps.facebook.com/csharpsamples/", CanvasUrl = "http://localhost:2839/", SiteUrl = "http://localhost:2839/" });
            applications.Register("C# Sample 2", new FacebookAppSettings("131403313556860", "18b8c40f4e48e2616a0c548ec96fdeb2") { CanvasPage = "http://apps.facebook.com/csharpsamplestwo/", CanvasUrl = "http://localhost:4182/", SiteUrl = "http://localhost:4182/" });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterFacebookApplications(FacebookSdk.Applications);

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

    }
}