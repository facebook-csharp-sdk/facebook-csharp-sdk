using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Facebook.Web.Mvc.Xhtml
{
    public static class LikeButtonHelper
    {
        //public static MvcHtmlString LikeButton(this HtmlHelper html, object htmlAttributes)
        //{
        //    return XfbmlHelpers.FacebookTag("like", htmlAttributes);
        //}

        //public static MvcHtmlString LikeButton(this HtmlHelper html, IDictionary<string, object> htmlAttributes)
        //{
        //    return XfbmlHelpers.FacebookTag("like", htmlAttributes);
        //}

        //public static MvcHtmlString LikeButton(this HtmlHelper html, string urlToLike, object htmlAttributes)
        //{

        //}

        //public static MvcHtmlString LikeButton(this HtmlHelper html, string urlToLike, string layout, string verb, bool showFaces, object htmlAttributes)
        //{
        //    var attrs = new RouteValueDictionary(htmlAttributes);
        //    attrs["href"] = urlToLike;
        //    attrs["layout"] = layout 
        //    return XfbmlHelpers.FacebookTag("like", (IDictionary<string, object>)attrs);
        //}

        public static MvcHtmlString LikeButton(this HtmlHelper html, string urlToLike, string layout, string verb, bool showFaces, int width, int height)
        {
            return LikeButton(html, urlToLike, layout, verb, showFaces, width, height, (IDictionary<string, object>)null);
        }

        public static MvcHtmlString LikeButton(this HtmlHelper html, string urlToLike, string layout, string verb, bool showFaces, int width, int height, object htmlAttributes)
        {
            return XfbmlHelpers.FacebookTag("like", htmlAttributes);
        }

        public static MvcHtmlString LikeButton(this HtmlHelper html, string urlToLike, string layout, string verb, bool showFaces, int width, int height, IDictionary<string, object> htmlAttributes)
        {
            return XfbmlHelpers.FacebookTag("like", htmlAttributes);
        }


    }
}
