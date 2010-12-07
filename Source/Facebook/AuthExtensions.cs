using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Facebook
{
    public static class AuthExtensions
    {

        public static bool IsAuthorized(this FacebookAppBase app)
        {
            throw new NotImplementedException();
        }

        public static bool HasPermissions(this FacebookAppBase app, params string[] permissions)
        {
            throw new NotImplementedException();
        }

#if !SILVERLIGHT && !CLIENTPROFILE

        public static bool Authorize(this FacebookAppBase app)
        {
            Contract.Requires(app != null);

            return Authorize(app, System.Web.HttpContext.Current);
        }

        public static bool Authorize(this FacebookAppBase app, System.Web.HttpContext httpContext)
        {
            Contract.Requires(app != null);
            Contract.Requires(httpContext != null);

            return Authorize(app, new System.Web.HttpContextWrapper(httpContext));
        }

        public static bool Authorize(this FacebookAppBase app, System.Web.HttpContextBase httpContext)
        {
            Contract.Requires(app != null);
            Contract.Requires(httpContext != null);

            throw new NotImplementedException();
        }

#else

        public static bool Authorize(this FacebookAppBase app, string oauthUrl)
        {
            Contract.Requires(app != null);

            return Authorize(app, System.Web.HttpContext.Current);
        }

        public static bool Authorize(this FacebookAppBase app, Uri oauthUrl)
        {
            Contract.Requires(app != null);

            return Authorize(app, System.Web.HttpContext.Current);
        }


#endif
    }
}
