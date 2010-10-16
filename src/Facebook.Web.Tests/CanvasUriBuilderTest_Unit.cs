using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Collections.Specialized;

namespace Facebook.Web.Tests
{
    [TestClass]
    public partial class CanvasUriBuilderTest
    {

    }

    public class HttpRequestMock : HttpRequestBase
    {
        Uri uri;
        NameValueCollection headers;

        public HttpRequestMock(Uri uri) : this(uri, null) { }

        public HttpRequestMock(Uri uri, string host)
        {
            this.uri = uri;
            this.headers = new NameValueCollection();
            if (!String.IsNullOrEmpty(host))
            {
                this.headers.Add("HOST", host);
            }
        }

        public override Uri Url
        {
            get
            {
                return this.uri;
            }
        }

        public override System.Collections.Specialized.NameValueCollection Headers
        {
            get
            {
                return this.headers;
            }
        }
    }
}
