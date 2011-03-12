// --------------------------------
// <copyright file="FacebookWebContext.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Represents the Facebook authorizer class.
    /// </summary>
    public class FacebookWebContext
    {
        /// <summary>
        /// The facebook settings.
        /// </summary>
        private readonly IFacebookApplication _facebookApplication;

        /// <summary>
        /// The http context.
        /// </summary>
        private readonly HttpContextBase _httpContext;

        /// <summary>
        /// The facebook session.
        /// </summary>
        private FacebookSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebContext"/> class.
        /// </summary>
        public FacebookWebContext()
            : this(FacebookApplication.Current, new HttpContextWrapper(System.Web.HttpContext.Current))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebContext"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public FacebookWebContext(IFacebookApplication settings)
            : this(settings, new HttpContextWrapper(System.Web.HttpContext.Current))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookWebContext"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        public FacebookWebContext(IFacebookApplication settings, HttpContextBase httpContext)
        {
            Contract.Requires(settings != null);
            Contract.Requires(!string.IsNullOrEmpty(settings.AppId));
            Contract.Requires(!string.IsNullOrEmpty(settings.AppSecret));
            Contract.Requires(httpContext != null);
            Contract.Requires(httpContext.Request != null);
            Contract.Requires(httpContext.Request.Params != null);
            Contract.Requires(httpContext.Response != null);

            _facebookApplication = settings;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Gets the current facebook web context.
        /// </summary>
        public static FacebookWebContext Current
        {
            get
            {
                Contract.Ensures(Contract.Result<FacebookWebContext>() != null);
                return new FacebookWebContext();
            }
        }

        /// <summary>
        /// Gets the Facebook application settings.
        /// </summary>
        public IFacebookApplication Settings
        {
            get
            {
                Contract.Ensures(Contract.Result<IFacebookApplication>() != null);
                return _facebookApplication;
            }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public long UserId
        {
            get
            {
                return Session != null ? Session.UserId : 0;
            }
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        public string AccessToken
        {
            get
            {
                return Session != null ? Session.AccessToken : null;
            }
        }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        public virtual FacebookSession Session
        {
            get
            {
                return _session ??
                       (_session = FacebookSession.GetSession(Settings.AppId, Settings.AppSecret, HttpContext));
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
                return _httpContext;
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

            if (Session == null || Session.UserId == 0)
            {
                return new string[0];
            }

            return HasPermissions(Settings.AppId, Settings.AppSecret, Session.UserId, permissions);
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
            return HasPermissions(new[] { permission }).Length == 1;
        }

        public bool IsAuthenticated()
        {
            return Session != null && Session.Expires > DateTime.UtcNow;
        }

        public bool IsAuthorized()
        {
            return IsAuthorized(null);
        }

        /// <summary>
        /// Checks if the user is authenticated and the application has all the specified permissions.
        /// </summary>
        /// <returns>
        /// Return true if the user is authenticated and the application has all the specified permissions.
        /// </returns>
        public virtual bool IsAuthorized(params string[] permissions)
        {
            bool isAuthorized = IsAuthenticated();

            if (isAuthorized && permissions != null)
            {
                if (permissions.Length == 1 && permissions[0] == null)
                {
                    return true;
                }

                var currentPerms = HasPermissions(permissions);
                foreach (var perm in permissions)
                {
                    if (!currentPerms.Contains(perm))
                    {
                        return false;
                    }
                }
            }

            return isAuthorized;
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
        /// Deletes all Facebook authentication cookies found in the current request.
        /// </summary>
        public void DeleteAuthCookie()
        {
            string sessionCookieName = FacebookSession.GetCookieName(Settings.AppId);
            foreach (var cookieName in HttpContext.Request.Cookies.AllKeys)
            {
                if (cookieName == sessionCookieName)
                {
                    var cookie = HttpContext.Request.Cookies[sessionCookieName];
                    cookie.Expires = DateTime.UtcNow.AddDays(-1);
                    cookie.Value = null;
                    HttpContext.Response.Cookies.Set(cookie);
                }
            }
        }


        /// <summary>
        /// The code contracts invariant object method.
        /// </summary>
        [ContractInvariantMethod]
        private void InvarientObject()
        {
            Contract.Invariant(_facebookApplication != null);
            Contract.Invariant(_httpContext != null);
            Contract.Invariant(_httpContext.Request != null);
            Contract.Invariant(_httpContext.Request.Params != null);
            Contract.Invariant(HttpContext.Response != null);
            Contract.Invariant(HttpContext.Request != null);
            Contract.Invariant(HttpContext.Request.Params != null);
        }
    }
}