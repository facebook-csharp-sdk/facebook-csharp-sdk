using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc3Website.Models
{
    public class InMemoryUserStore
    {

        private static System.Collections.Concurrent.ConcurrentBag<FacebookUser> users = new System.Collections.Concurrent.ConcurrentBag<FacebookUser>();

        public static void Add(FacebookUser user)
        {
            if (users.SingleOrDefault(u => u.FacebookId == user.FacebookId) != null)
            {
                throw new InvalidOperationException("User already exists.");
            }

            users.Add(user);
        }

        public static FacebookUser Get(long facebookId)
        {
            return users.SingleOrDefault(u => u.FacebookId == facebookId);
        }

    }
}