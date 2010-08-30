// --------------------------------
// <copyright file="XfbmlHelpers.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Facebook.Web.Mvc.Xhtml {

    // NOTE: For supported XFBML in the new Javascript SDK see:
    // http://github.com/facebook/connect-js

    public static class XfbmlHelpers {

        public static MvcHtmlString Comments(this HtmlHelper xfbml)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString Fan(this HtmlHelper xfbml)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString LiveStream(this HtmlHelper xfbml) {
            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = @"This is in line with facebook naming conventions.")]
        public static MvcHtmlString LoginButton(this HtmlHelper xfbml)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString Name(this HtmlHelper xfbml)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ProfilePicture(this HtmlHelper xfbml)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ServerFbml(this HtmlHelper xfbml)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ShareButton(this HtmlHelper xfbml)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString FacebookTag(string tagName, object htmlAttributes)
        {
            return FacebookTag(tagName, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString FacebookTag(string tagName, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("fb:" + tagName);
            tagBuilder.MergeAttributes(htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}
