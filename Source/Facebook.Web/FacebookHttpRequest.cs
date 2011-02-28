﻿// --------------------------------
// <copyright file="Authorizer.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Represents the Facebook authorizer class.
    /// </summary>
    public class FacebookHttpRequest
    {

        /// <summary>
        /// The facebook settings.
        /// </summary>
        private readonly IFacebookApplication m_facebookApplication;

        /// <summary>
        /// The http context.
        /// </summary>
        private readonly HttpContextBase m_httpContext;

        /// <summary>
        /// The facebook session.
        /// </summary>
        private FacebookSession m_session;

        /// <summary>
        /// The facebook signed request.
        /// </summary>
        private FacebookSignedRequest m_signedRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookCanvasRequest"/> class.
        /// </summary>
        public FacebookHttpRequest()
            : this(FacebookContext.Current, new HttpContextWrapper(System.Web.HttpContext.Current))
        {
        }

        public FacebookHttpRequest(IFacebookApplication settings)
            : this(settings, new HttpContextWrapper(System.Web.HttpContext.Current)) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookCanvasRequest"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        public FacebookHttpRequest(IFacebookApplication settings, HttpContextBase httpContext)
        {
            Contract.Requires(settings != null);
            Contract.Requires(!string.IsNullOrEmpty(settings.AppId));
            Contract.Requires(!string.IsNullOrEmpty(settings.AppSecret));
            Contract.Requires(httpContext != null);
            Contract.Requires(httpContext.Request != null);
            Contract.Requires(httpContext.Request.Params != null);
            Contract.Requires(httpContext.Response != null);

            this.m_facebookApplication = settings;
            this.m_httpContext = httpContext;
        }

        public static FacebookHttpRequest Current
        {
            get { return new FacebookHttpRequest(); }
        }

        /// <summary>
        /// Gets the Facebook application settings.
        /// </summary>
        public IFacebookApplication Settings
        {
            get
            {
                Contract.Ensures(Contract.Result<IFacebookApplication>() != null);
                return this.m_facebookApplication;
            }
        }

        public long UserId
        {
            get
            {
                if (this.Session != null)
                {
                    return this.Session.UserId;
                }
                return 0;
            }
        }

        public string AccessToken
        {
            get
            {
                if (this.Session != null)
                {
                    return this.Session.AccessToken;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        public virtual FacebookSession Session
        {
            get
            {
                return this.m_session ??
                       (this.m_session = FacebookSession.GetSession(this.Settings.AppId, this.Settings.AppSecret, this.HttpContext));
            }
        }

        public FacebookSignedRequest SignedRequest
        {
            get
            {
                return this.m_signedRequest ??
                    (this.m_signedRequest = FacebookSignedRequest.GetSignedRequest(this.Settings.AppSecret, this.HttpContext));
            }
        }

        /// <summary>
        /// Gets the http context.
        /// </summary>
        public HttpContextBase HttpContext
        {
            get
            {
                Contract.Ensures(Contract.Result<HttpContextBase>() != null);
                return this.m_httpContext;
            }
        }

        /// <summary>
        /// Check whether the user has the specified permissions.
        /// </summary>
        /// <param name="permissions">
        /// The permissions.
        /// </param>
        /// <returns>
        /// Returns the list of allowed permissions.
        /// </returns>
        public virtual string[] HasPermissions(params string[] permissions)
        {
            Contract.Requires(permissions != null);
            Contract.Ensures(Contract.Result<string[]>() != null);

            if (this.Session == null || this.Session.UserId == 0)
            {
                return new string[0];
            }

            return HasPermissions(this.Settings.AppId, this.Settings.AppSecret, this.Session.UserId, permissions);
        }

        /// <summary>
        /// Check whether the user has the specified permissions.
        /// </summary>
        /// <param name="permission">
        /// The permission.
        /// </param>
        /// <returns>
        /// Returns true if the user has permission otherwise false.
        /// </returns>
        public virtual bool HasPermission(string permission)
        {
            return this.HasPermissions(new[] { permission }).Length == 1;
        }

        public bool IsAuthorized()
        {
            return this.IsAuthorized(null);
        }

        /// <summary>
        /// Checks if the user is authenticated and the application has all the specified permissions.
        /// </summary>
        /// <returns>
        /// Return true if the user is authenticated and the application has all the specified permissions.
        /// </returns>
        public virtual bool IsAuthorized(params string[] permissions)
        {
            bool isAuthenticated = this.Session != null;

            if (isAuthenticated && permissions != null)
            {
                var currentPerms = this.HasPermissions(permissions);
                foreach (var perm in permissions)
                {
                    if (!currentPerms.Contains(perm))
                    {
                        return false;
                    }
                }
            }

            return isAuthenticated;
        }

        public void DeleteAuthCookie()
        {
            string sessionCookieName = FacebookSession.GetCookieName(this.Settings.AppId);
            foreach (var cookieName in this.HttpContext.Request.Cookies.AllKeys)
            {
                if (cookieName == sessionCookieName)
                {
                    var cookie = this.HttpContext.Request.Cookies[sessionCookieName];
                    cookie.Expires = DateTime.UtcNow.AddDays(-1);
                    cookie.Value = null;
                    this.HttpContext.Response.Cookies.Set(cookie);
                }
            }
        }

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

                var fb = new FacebookClient(string.Concat(appId, "|", appSecret));
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

        /// <summary>
        /// The code contracts invariant object method.
        /// </summary>
        [ContractInvariantMethod]
        private void InvarientObject()
        {
            Contract.Invariant(this.m_facebookApplication != null);
            Contract.Invariant(this.m_httpContext != null);
            Contract.Invariant(this.m_httpContext.Request != null);
            Contract.Invariant(this.m_httpContext.Request.Params != null);
            Contract.Invariant(this.HttpContext.Response != null);
            Contract.Invariant(this.HttpContext.Request != null);
            Contract.Invariant(this.HttpContext.Request.Params != null);
        }
    }
}