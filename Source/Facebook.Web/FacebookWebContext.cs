// --------------------------------
// <copyright file="FacebookWebContext.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Represents the Facebook authorizer class.
    /// </summary>
    public class FacebookWebContext
    {
        /// <summary>
        /// The Facebook settings.
        /// </summary>
        private readonly IFacebookApplication _facebookApplication;

        /// <summary>
        /// The http context.
        /// </summary>
        private readonly HttpContextBase _httpContext;

        /// <summary>
        /// The Facebook session.
        /// </summary>
        private FacebookSession _session;

        /// <summary>
        /// The Facebook signed request.
        /// </summary>
        private FacebookSignedRequest _signedRequest;

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
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (string.IsNullOrEmpty(settings.AppId))
                throw new Exception("settings.AppId is null.");
            if (string.IsNullOrEmpty(settings.AppSecret))
                throw new Exception("settings.AppSecret is null.");
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            _facebookApplication = settings;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Gets the current Facebook web context.
        /// </summary>
        public static FacebookWebContext Current
        {
            get
            {
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
        /// Gets the Facebook session.
        /// </summary>
        public virtual FacebookSession Session
        {
            get
            {
                return _session ??
                       (_session = FacebookSession.GetSession(Settings, HttpContext));
            }
        }

        /// <summary>
        /// Gets the signed request.
        /// </summary>
        public FacebookSignedRequest SignedRequest
        {
            get
            {
                return _signedRequest ??
                    (_signedRequest = FacebookSignedRequest.GetSignedRequest(Settings.AppId, Settings.AppSecret, HttpContext));
            }
        }

        /// <summary>
        /// Gets the http context.
        /// </summary>
        public HttpContextBase HttpContext
        {
            get
            {
                return _httpContext;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the request is a secure connection or not.
        /// </summary>
        public bool IsSecureConnection
        {
            get { return _httpContext.Request.Url.Scheme == "https"; }
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
            if (permissions == null)
                throw new ArgumentNullException("permissions");

            if (Session == null || Session.UserId == 0)
            {
                return new string[0];
            }

            return HasPermissions(this, Session.AccessToken, Settings.AppId, Session.UserId, permissions) ?? new string[0];
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
            if (string.IsNullOrEmpty(permission))
                throw new ArgumentNullException("permission");

            var result = HasPermissions(new[] { permission });
            return result != null && result.Length == 1;
        }

        /// <summary>
        /// Checks whether the user is authenticated or not.
        /// </summary>
        /// <returns>Returns true if the user is authenticated else false.</returns>
        public bool IsAuthenticated()
        {
            return Session != null && Session.Expires > DateTime.UtcNow;
        }

        /// <summary>
        /// Checks if the user is authorized or not.
        /// </summary>
        /// <returns>Returns true if the user is authorized else false.</returns>
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

            if (isAuthorized)
            {
                var currentPerms = HasPermissions(this, AccessToken, Settings.AppId, UserId, permissions);
                if (currentPerms == null)
                    return false;

                if (permissions != null)
                {
                    foreach (var perm in permissions)
                    {
                        if (!currentPerms.Contains(perm))
                        {
                            return false;
                        }
                    }
                }
            }

            return isAuthorized;
        }

        /// <summary>
        /// Check if the Facebook App has permissions from the specified user.
        /// </summary>
        /// <param name="context">
        /// The Facebook web context.
        /// </param>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        /// <param name="appId">
        /// The app id.
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
        internal static string[] HasPermissions(FacebookWebContext context, string accessToken, string appId, long userId, string[] permissions)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException("appId");
            if (userId < 0)
                throw new ArgumentOutOfRangeException("userId", "userId must be equal or greater than 0");

            if (userId != 0 && !string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    var fb = new FacebookWebClient(context) { AccessToken = accessToken };
                    var remoteResult = ((IDictionary<string, object>)fb.Get("me/permissions"));
                    if (remoteResult != null && remoteResult.ContainsKey("data"))
                    {
                        var data = remoteResult["data"] as IList<object>;

                        if (data != null && data.Count > 0)
                        {
                            var permData = data[0] as IDictionary<string, object>;
                            if (permData == null)
                                return new string[0];
                            else
                            {
                                return (from perm in permData
                                        where perm.Value.ToString() == "1"
                                        select perm.Key).ToArray();
                            }
                        }
                    }
                }
                catch (FacebookOAuthException)
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Deletes all Facebook authentication cookies found in the current request.
        /// </summary>
        public void DeleteAuthCookie()
        {
            string sessionCookieName = FacebookSession.GetCookieName(Settings.AppId);
            string signedRequestCookieName = FacebookSignedRequest.GetSignedRequestCookieName(Settings.AppId);

            foreach (var cookieName in HttpContext.Request.Cookies.AllKeys)
            {
                if (cookieName == signedRequestCookieName)
                {
                    var cookie = HttpContext.Request.Cookies[signedRequestCookieName];
                    cookie.Expires = DateTime.UtcNow.AddDays(-1);
                    cookie.Value = null;
                    HttpContext.Response.Cookies.Set(cookie);
                }
                if (cookieName == sessionCookieName)
                {
                    var cookie = HttpContext.Request.Cookies[sessionCookieName];
                    cookie.Expires = DateTime.UtcNow.AddDays(-1);
                    cookie.Value = null;
                    HttpContext.Response.Cookies.Set(cookie);
                }
            }
        }
    }
}