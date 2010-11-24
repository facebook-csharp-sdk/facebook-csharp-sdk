﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Web;

namespace Facebook.Web
{
    public class Authorizer
    {
        protected FacebookAppBase FacebookApp { get; set; }

        /// <summary>
        /// Gets or sets the extended permissions.
        /// </summary>
        /// <value>The permissions required.</value>
        public string Perms { get; set; }
        /// <summary>
        /// Gets or sets the cancel URL path.
        /// </summary>
        /// <value>The cancel URL path.</value>
        public string CancelUrlPath { get; set; }
        /// <summary>
        /// Gets or sets the return URL path.
        /// </summary>
        /// <value>The return URL path.</value>
        public string ReturnUrlPath { get; set; }

        public Authorizer(FacebookAppBase facebookApp)
        {
            Contract.Requires(facebookApp != null);

            this.FacebookApp = facebookApp;
        }

        public virtual bool IsAuthorized()
        {
            bool authenticated = this.FacebookApp.Session != null;
            if (authenticated && !string.IsNullOrEmpty(Perms))
            {
                var requiredPerms = Perms.Replace(" ", String.Empty).Split(',');
                var currentPerms = HasPermissions(requiredPerms);
                foreach (var perm in requiredPerms)
                {
                    if (!currentPerms.Contains(perm))
                    {
                        return false;
                    }
                }
            }
            return authenticated;
        }

        public virtual bool HasPermission(string permission)
        {
            return HasPermissions(new string[] { permission }).Length == 1;
        }

        public virtual string[] HasPermissions(string[] permissions)
        {
            Contract.Requires(permissions != null);
            Contract.Ensures(Contract.Result<string[]>() != null);

            var result = new string[0];
            if (FacebookApp.UserId != 0)
            {
                var perms = new StringBuilder();
                for (int i = 0; i < permissions.Length; i++)
                {
                    perms.Append(permissions[i]);
                    if (i < permissions.Length - 1)
                    {
                        perms.Append(",");
                    }
                }
                var query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM permissions WHERE uid == {1}", perms.ToString(), FacebookApp.UserId);
                var parameters = new Dictionary<string, object>();
                parameters["query"] = query;
                parameters["method"] = "fql.query";
                parameters["access_token"] = string.Concat(FacebookApp.AppId, "|", FacebookApp.ApiSecret);
                var data = (JsonArray)FacebookApp.Get(parameters);
                if (data != null && data.Count > 0)
                {
                    var permData = data[0] as IDictionary<string, object>;
                    if (permData != null)
                    {
                        result = (from perm in permData
                                  where perm.Value.ToString() == "1"
                                  select perm.Key).ToArray();
                    }
                }
            }
            return result;

        }

    }
}