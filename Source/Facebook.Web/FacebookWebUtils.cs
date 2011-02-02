﻿// --------------------------------
// <copyright file="FacebookWebUtils.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Facebook helper methods for web.
    /// </summary>
    internal class FacebookWebUtils
    {
        /// <summary>
        /// Gets the facebook signed request from the http request.
        /// </summary>
        /// <param name="appSecret">
        /// The app Secret.
        /// </param>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <returns>
        /// Returns the signed request if found otherwise null.
        /// </returns>
        internal static FacebookSignedRequest GetSignedRequest(string appSecret, HttpRequestBase httpRequest)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);

            return httpRequest.Params.AllKeys.Contains("signed_request") ? FacebookSignedRequest.Parse(appSecret, httpRequest.Params["signed_request"]) : null;
        }

        /// <summary>
        /// Gets the facebook session cookie name for the specified facebook appliaction.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <returns>
        /// Returns the name of the cookie name.
        /// </returns>
        internal static string GetSessionCookieName(string appId)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            return string.Concat("fbs_", appId);
        }

        /// <summary>
        /// Gets the facebook session cookie value for the specified application.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="httpRequet">
        /// The http request.
        /// </param>
        /// <returns>
        /// Returns the facebook session cookie value if present otherwise null.
        /// </returns>
        internal static string GetSessionCookieValue(string appId, HttpRequestBase httpRequet)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(httpRequet != null);
            Contract.Requires(httpRequet.Params != null);

            var cookieName = GetSessionCookieName(appId);

            return httpRequet.Params.AllKeys.Contains(cookieName) ? httpRequet.Params[cookieName] : null;
        }

        /// <summary>
        ///  Gets the facebook session from the http request.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <returns>
        /// Returns the facebook session if found, otherwise null.
        /// </returns>
        internal static FacebookSession GetSession(string appId, string appSecret, HttpRequestBase httpRequest)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(!string.IsNullOrEmpty(appSecret));
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);

            // try creating session from signed_request if exists.
            var signedRequest = GetSignedRequest(appSecret, httpRequest);

            if (signedRequest != null)
            {
                return FacebookSession.Create(appSecret, signedRequest);
            }

            // try creating session from cookie if exists.
            var sessionCookieValue = GetSessionCookieValue(appId, httpRequest);
            if (!string.IsNullOrEmpty(sessionCookieValue))
            {
                return FacebookSession.ParseCookieValue(appSecret, sessionCookieValue);
            }

            // no facebook session found.
            return null;
        }

        #region Extendend Permission helper methods

        /// <summary>
        /// Check if the Facebook App has permissions from the specified user.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="permissions">
        /// The list of permissions.
        /// </param>
        /// <returns>
        /// The list of permissions that are allowed from the specified permissions.
        /// </returns>
        internal static string[] HasPermissions(string appId, string appSecret, long userId, string[] permissions)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(!string.IsNullOrEmpty(appSecret));
            Contract.Requires(permissions != null);
            Contract.Requires(userId >= 0);
            Contract.Ensures(Contract.Result<string[]>() != null);

            var result = new string[0];

            if (userId != 0)
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

                var query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM permissions WHERE uid == {1}", perms, userId);
                var parameters = new Dictionary<string, object>();
                parameters["query"] = query;
                parameters["method"] = "fql.query";

                var fb = new FacebookApp(string.Concat(appId, "|", appSecret));
                var data = fb.Get(parameters) as IList<object>;

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

        /// <summary>
        ///  Check if the Facebook App has permission from the specified user.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="permission">
        /// The permission.
        /// </param>
        /// <returns>
        /// Returns true if the facebook app has the specified permission.
        /// </returns>
        internal static bool HasPermission(string appId, string appSecret, long userId, string permission)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            Contract.Requires(!string.IsNullOrEmpty(appSecret));
            Contract.Requires(!string.IsNullOrEmpty(permission));
            Contract.Requires(userId >= 0);

            return HasPermissions(appId, appSecret, userId, new[] { permission }).Length == 1;
        }

        #endregion
    }
}