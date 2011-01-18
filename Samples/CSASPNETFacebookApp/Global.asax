<%@ Application Language="C#" %>
<script RunAt="server">

    static void FacebookAspNetWebFormsCanvasFormPostFix(HttpContext context)
    {
        var appPath = context.Request.ApplicationPath;
        var requestPath = context.Request.Path;

        if (requestPath.EndsWith("/") && appPath != "/")
        {
            requestPath = requestPath.Substring(0, requestPath.Length - 1);
        }

        if (appPath == requestPath)
        {
            var query = context.Request.Url.Query ?? "";
            if (query.StartsWith("?"))
            {
                query = query.Substring(1);
            }

            context.RewritePath(requestPath + "/default.aspx", string.Empty, query);
        }
    }

    void Application_BeginRequest(Object Sender, EventArgs e)
    {
        // Incase you are running in cassini (visual studio webserver) make sure you call this method,
        // for apps running under iis this doesn't need to be called.
        // FacebookAspNetWebFormsCanvasFormPostFix(HttpContext.Current);
    }

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
