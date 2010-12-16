namespace Facebook.TestUtils.Fakes
{
    using System.Web;

    public class FakeHttpResponse : HttpResponseBase
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
