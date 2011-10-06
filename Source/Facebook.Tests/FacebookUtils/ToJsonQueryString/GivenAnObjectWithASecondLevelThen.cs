
namespace Facebook.Tests.FacebookUtils.ToJsonQueryString
{
    using Facebook;
    using System.Collections.Generic;
    using System.Dynamic;
    using Xunit;

    public class GivenAnObjectWithASecondLevelThen
    {
        [Fact]
        public void ShouldSerializeItCorrectly()
        {
            dynamic attachment = new ExpandoObject();
            attachment.name = "my attachment";
            attachment.href = "http://apps.facebook.com/canvas";

            dynamic parameters = new ExpandoObject();
            parameters.method = "stream.publish";
            parameters.message = "my message";
            parameters.attachment = attachment;

#if SILVERLIGHT
            string result = FacebookUtils.ToJsonQueryString((IDictionary<string, object>)parameters);
#else
            string result = FacebookUtils.ToJsonQueryString(parameters);
#endif
            Assert.Equal("method=stream.publish&message=my%20message&attachment=%7B%22name%22%3A%22my%20attachment%22%2C%22href%22%3A%22http%3A%2F%2Fapps.facebook.com%2Fcanvas%22%7D", result);
        }
    }
}