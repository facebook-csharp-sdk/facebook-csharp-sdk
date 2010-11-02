using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Facebook.Tests.Mocks;
using System.Web;

namespace Facebook.Web.Tests
{
    [TestClass]
    public class CanvasUrlBuilderTests
    {
        [TestMethod]
        public void Url_With_Canvas_Page_Subfolder_Produces_Correct_Urls()
        {
            var settings = new CanvasSettings();
            settings.CanvasPage = new Uri("http://apps.facebook.com/facebooksdktest");
            settings.CanvasUrl = new Uri("http://www.facebooksdk.net/area/client/controller");
            var request = new HttpRequestMock(new Uri("http://www.facebooksdk.net/area/client/controller/action"));
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(request, settings);

            Assert.AreEqual("http://apps.facebook.com/facebooksdktest/action", urlBuilder.CurrentCanvasPage.ToString());

            Assert.AreEqual("http://apps.facebook.com/facebooksdktest", urlBuilder.CanvasPage.ToString());

            Assert.AreEqual("/facebooksdktest", urlBuilder.CanvasPageApplicationPath);

            Assert.AreEqual("http://www.facebooksdk.net/area/client/controller", urlBuilder.CanvasUrl.ToString());

            Assert.AreEqual("http://apps.facebook.com/facebooksdktest/action", urlBuilder.CurrentCanvasPage.ToString());

            Assert.AreEqual("/action", urlBuilder.CurrentCanvasPathAndQuery);

            Assert.AreEqual("http://www.facebooksdk.net/area/client/controller/action", urlBuilder.CurrentCanvasUrl.ToString());
        }

        [TestMethod]
        public void Url_With_Canvas_Page_Produces_Correct_Urls()
        {
            var settings = new CanvasSettings();
            settings.CanvasPage = new Uri("http://apps.facebook.com/facebooksdktest");
            settings.CanvasUrl = new Uri("http://www.facebooksdk.net");
            var request = new HttpRequestMock(new Uri("http://www.facebooksdk.net/action"));
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(request, settings);

            Assert.AreEqual("http://apps.facebook.com/facebooksdktest/action", urlBuilder.CurrentCanvasPage.ToString());

            Assert.AreEqual("http://apps.facebook.com/facebooksdktest", urlBuilder.CanvasPage.ToString());

            Assert.AreEqual("/facebooksdktest", urlBuilder.CanvasPageApplicationPath);

            Assert.AreEqual("http://www.facebooksdk.net/", urlBuilder.CanvasUrl.ToString());

            Assert.AreEqual("http://apps.facebook.com/facebooksdktest/action", urlBuilder.CurrentCanvasPage.ToString());

            Assert.AreEqual("/action", urlBuilder.CurrentCanvasPathAndQuery);

            Assert.AreEqual("http://www.facebooksdk.net/action", urlBuilder.CurrentCanvasUrl.ToString());
        }
    }
}
