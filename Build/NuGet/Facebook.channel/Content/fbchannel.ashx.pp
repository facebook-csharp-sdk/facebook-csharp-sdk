 <%@ WebHandler Language="C#" Class="$rootnamespace$.FacebookChannelHandler" %>
 
namespace $rootnamespace$
{
    using System;
    using System.Web;

    /// <summary>
    /// Channel file for Facebook to address issues with cross domain communication in certain browsers.
    /// </summary>
    /// <remarks>
    /// http://developers.facebook.com/docs/reference/javascript
    /// 
    /// The channel file addresses some issues with cross domain communication in certain browsers. 
    /// The contents of the channel.html file can be just a single line:
    /// 
    ///     &lt;script src="//connect.facebook.net/en_US/all.js">&lt;/script>
    /// 
    /// It is important for the channel file to be cached for as long as possible. 
    /// When serving this file, you must send valid Expires headers with a long expiration period.
    /// This will ensure the channel file is cached by the browser which is important for a smooth user experience.
    /// Without proper caching, cross domain communication will become very slow and users will suffer a severely
    /// degraded experience.
    /// 
    /// The channelUrl parameter is optional, but recommended. 
    /// Providing a channel file can help address three specific known issues. 
    /// First, pages that include code to communicate across frames may cause Social Plugins to show up as blank
    /// without a channelUrl. Second, if no channelUrl is provided and a page includes auto-playing audio or video,
    /// the user may hear two streams of audio because the page has been loaded a second time in the background for
    /// cross domain communication. Third, a channel file will prevent inclusion of extra hits in your server-side logs.
    /// If you do not specify a channelUrl, you can remove page views containing fb_xd_bust or fb_xd_fragment parameters
    /// from your logs to ensure proper counts.
    /// 
    /// The channelUrl must be a fully qualified URL matching the page on which you include the SDK.
    /// In other words, the channel file domain must include www if your site is served using www,
    /// and if you modify document.domain on your page you must make the same document.domain change in the channel.html
    /// file as well. The protocols must also match. If your page is served over https, your channelUrl must also be https.
    /// Remember to use the matching protocol for the script src as well. The sample code above uses protocol-relative
    /// URLs which should handle most https cases properly.
    /// </remarks>
    public class FacebookChannelHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            response.ClearHeaders();

            const int cacheExpires = 60 * 60 * 24 * 365;
            response.AppendHeader("Pragma", "public");
            response.AppendHeader("Cache-Control", "maxage=" + cacheExpires);
            response.AppendHeader("Expires", DateTime.Now.ToUniversalTime().AddSeconds(cacheExpires).ToString("r"));
            context.Response.ContentType = "text/html";
            context.Response.Write("<script src=\"//connect.facebook.net/en_US/all.js\"></script>");
        }

        public bool IsReusable { get { return false; } }
    }
}