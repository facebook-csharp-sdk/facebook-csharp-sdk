using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook
{

    public class UserGraphInfo : FluentGraphInfo
    {

    }

    public static class UserHelper
    {

        public static UserGraphInfo GetUser(this FacebookAppBase app)
        {
            throw new NotImplementedException();
        }

        public static UserGraphInfo GetUser<T>(this FacebookAppBase app)
        {
            throw new NotImplementedException();
        }

        public static UserGraphInfo GetMe(this FacebookAppBase app)
        {
            throw new NotImplementedException();
        }

        public static UserGraphInfo GetMe<T>(this FacebookAppBase app)
        {
            throw new NotImplementedException();
        }

    }
}
