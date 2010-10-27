using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Facebook.Tests.Mocks
{
    public class HttpResponseMock : HttpResponseBase
    {

        public override HttpCookieCollection Cookies
        {
            get
            {
                return base.Cookies;
            }
        }

    }
}
