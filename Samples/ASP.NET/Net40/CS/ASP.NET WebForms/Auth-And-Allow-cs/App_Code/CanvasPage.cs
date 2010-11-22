using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using Facebook;
using Facebook.Web;

/// <summary>
/// You will want to create your own base class for your pages.
/// This is an example page you can use as a starting point, but
/// you will probably have additional things in common.
/// </summary>
public partial class CanvasPage : System.Web.UI.Page
{
    protected string requiredAppPermissions = "user_about_me";

    public CanvasPage()
    {
        fbApp = new FacebookApp();

        authorizer = new CanvasAuthorizer(fbApp);
        authorizer.Perms = requiredAppPermissions;
    }

    protected FacebookApp fbApp;
    protected CanvasAuthorizer authorizer;

    /// <summary>
    /// Performs a canvas redirect.
    /// </summary>
    /// <param name="controller">The controller.</param>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
        Justification = "Instance method for consistency with other helpers.")]
    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#",
        Justification = "Response.Redirect() takes its URI as a string parameter.")]
    [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "controller",
        Justification = "Extension method")]
    public void CanvasRedirect(string url)
    {
        Contract.Requires(url != null);

        var content = CanvasUrlBuilder.GetCanvasRedirectHtml(url);

        Response.ContentType = "text/html";
        Response.Write(content);
    }
}