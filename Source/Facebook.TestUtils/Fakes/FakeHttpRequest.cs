namespace Facebook.TestUtils.Fakes
{
    using System;
    using System.Collections.Specialized;
    using System.Web;

    public class FakeHttpRequest : HttpRequestBase
    {
        private Uri url;
        private System.Collections.Specialized.NameValueCollection headersCollection;
        private HttpCookieCollection cookies;
        private NameValueCollection paramsCollection;
        private string applicationPath;

        public FakeHttpRequest(Uri url)
        {
            this.url = url;
            this.headersCollection = new NameValueCollection();
            this.cookies = new HttpCookieCollection();
            this.paramsCollection = new NameValueCollection();
            this.applicationPath = "/";
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
                return this.headersCollection;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return this.cookies;
            }
        }

        public override System.Collections.Specialized.NameValueCollection Params
        {
            get
            {
                return this.paramsCollection;
            }
        }

        public override string ApplicationPath
        {
            get
            {
                return this.applicationPath;
            }
        }

    }
}
