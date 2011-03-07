using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace Mvc3Website.Models
{
    public class FacebookPrincipal : IPrincipal
    {

        List<string> extendedPermissions;

        public FacebookPrincipal(FacebookUser user)
        {
            this.extendedPermissions = new List<string>();
            this.Identity = user;
        }

        public FacebookPrincipal(FacebookUser user, string[] extendedPermissions)
        {
            this.extendedPermissions = new List<string>(extendedPermissions);
            this.Identity = user;
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}