using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace Mvc3Website.Models
{
    public class FacebookUser : IIdentity
    {
        long facebookId;
        string fullName;

        public FacebookUser(long facebookId, string fullName, string accessToken, DateTime expiresOn)
        {
            this.facebookId = facebookId;
            this.fullName = fullName;
            this.AccessToken = accessToken;
            this.ExpiresOn = expiresOn;
        }

        public string Name
        {
            get { return facebookId.ToString(); }
        }

        public string AuthenticationType
        {
            get { return "faceook"; }
        }

        public bool IsAuthenticated
        {
            get { return ExpiresOn < DateTime.UtcNow; }
        }

        public DateTime ExpiresOn
        {
            get;
            private set;
        }

        public string AccessToken
        {
            get;
            private set;
        }

    }
}