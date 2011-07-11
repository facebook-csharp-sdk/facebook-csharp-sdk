
namespace Facebook.Tests.FacebookClient.Get
{
    using System;
    using Facebook;
    using Moq;
    using Xunit;

    public class PathOnly
    {
        [Fact]
        public void DoesNotCallGetCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.ReturnsJson("{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}");

            var fb = mockFb.Object;
            bool? get = null;

            fb.GetCompleted += (o, e) => get = true;

            fb.Get("/4");

            Assert.Null(get);
        }

        [Fact]
        public void DoesNotCallPostCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.ReturnsJson("{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}");

            var fb = mockFb.Object;
            bool? post = null;

            fb.PostCompleted += (o, e) => post = true;

            fb.Get("/4");

            Assert.Null(post);
        }

        [Fact]
        public void DoesNotCallDeleteCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.ReturnsJson("{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}");

            var fb = mockFb.Object;
            bool? delete = null;

            fb.DeleteCompleted += (o, e) => delete = true;

            fb.Get("/4");

            Assert.Null(delete);
        }

        [Fact]
        public void WhenThereINoInternetConnectionAndFiddlerIsOpen_ThrowsWebExceptionWrapper()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.FiddlerNoInternetConnection();

            var fb = mockFb.Object;

            Exception exception = null;

            try
            {
                fb.Get("/4");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsAssignableFrom<WebExceptionWrapper>(exception);
        }

        [Fact]
        public void WhenThereIsNotInternetConnectionAndFiddlerIsNotOpen_ThrowsWebExceptionWrapper()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.NoInternetConnection();

            Exception exception = null;

            try
            {
                var fb = mockFb.Object;
                fb.Get("/4");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsAssignableFrom<WebExceptionWrapper>(exception);
        }
    }
}