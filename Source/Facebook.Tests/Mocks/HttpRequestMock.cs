using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Facebook.Tests.Mocks
{
    public class HttpRequestMock : HttpRequestBase
    {

        public override Uri Url
        {
            get
            {
                return base.Url;
            }
        }

        public override System.Collections.Specialized.NameValueCollection Headers
        {
            get
            {
                return base.Headers;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return base.Cookies;
            }
        }

        public override System.Collections.Specialized.NameValueCollection Params
        {
            get
            {
                return base.Params;
            }
        }

    }
}
