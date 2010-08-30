// --------------------------------
// <copyright file="OAuthUtility.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebookgraphtoolkit.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using Facebook.Utilities;

namespace Facebook.Api
{
    public class OAuthUtility
    {

        FacebookApp app;

        public OAuthUtility(FacebookApp app)
        {
            this.app = app;
        }

        public Uri GetOAuthLoginUrl()
        {
            return GetOAuthLoginUrl(new ExpandoObject());
        }

        public Uri GetOAuthLoginUrl(dynamic parameters)
        {
            if (parameters != null && !(parameters is IDictionary<string, object>))
            {
                throw new ArgumentException("The argument must be null or cast to IDictionary<string,object>.", "parameters");
            }

            var uri = app.CurrentUrl;
            string currentUrl = null;
            if (uri != null)
            {
                currentUrl = uri.ToString();
            }

            dynamic defaultParams = new ExpandoObject();
            defaultParams.client_id = app.AppId;
            defaultParams.display = "popup";
            defaultParams.redirect_uri = currentUrl;

            return app.GetUrl(
                "graph",
                "oauth/authorize",
                DynamicHelper.Merge(defaultParams, parameters));
        }


    }
}
