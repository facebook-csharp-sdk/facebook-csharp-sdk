using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Graph
{
    public class UserGraphService : GraphService
    {

        public UserGraphService()
            : base()
        {
        }

        public UserGraphService(FacebookApp app)
            : base(app)
        {
        }

        public UserInfo GetCurrentUserInfo()
        {
            return this.App.Api<UserInfo>("/me");
        }

        public UserInfo GetUserInfo(long facebookId)
        {
            return this.App.Api<UserInfo>(facebookId.ToString());
        }

        public UserInfo GetUserInfo(string username)
        {
            return this.App.Api<UserInfo>(username);
        }

    }
}
