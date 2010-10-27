using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace Facebook.Tests.Mocks
{
    public class HttpRequestMock : HttpRequestBase
    {
        private Uri url;

        public HttpRequestMock(Uri url)
        {
            this.url = url;
        }

        public override Uri Url
        {
            get
            {
                return this.url;
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
