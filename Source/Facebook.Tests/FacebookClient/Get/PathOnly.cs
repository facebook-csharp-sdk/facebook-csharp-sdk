
namespace Facebook.Tests.FacebookClient.Get
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Moq;
    using Xunit;

    public class PathOnly
    {
        [Fact]
        public void Sync_DoesNotCallGetCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.ReturnsJson("{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}");

            var fb = mockFb.Object;
            int called = 0;

            fb.GetCompleted += (o, e) => ++called;

            fb.Get("/4");

            Assert.Equal(0, called);
        }

        [Fact]
        public void Sync_DoesNotCallPostCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.ReturnsJson("{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}");

            var fb = mockFb.Object;
            int called = 0;

            fb.PostCompleted += (o, e) => ++called;

            fb.Get("/4");

            Assert.Equal(0, called);
        }

        [Fact]
        public void Sync_DoesNotCallDeleteCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.ReturnsJson("{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}");

            var fb = mockFb.Object;
            int called = 0;

            fb.DeleteCompleted += (o, e) => ++called;

            fb.Get("/4");

            Assert.Equal(0, called);
        }

        [Fact]
        public void SyncWhenThereIsNoInternetConnectionAndFiddlerIsOpen_ThrowsWebExceptionWrapper()
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
        public void SyncWhenThereIsNotInternetConnectionAndFiddlerIsNotOpen_ThrowsWebExceptionWrapper()
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

        [Fact]
        public void SyncReturnsJsonObjectIfObject()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.ReturnsJson("{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}");

            var fb = mockFb.Object;
            dynamic result = fb.Get("/4");

            Assert.IsAssignableFrom<IDictionary<string, object>>(result);
            Assert.IsType<JsonObject>(result);
        }

        [Fact]
        public void Async_CallsGetCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.ReturnsJson("{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}");

            var fb = mockFb.Object;
            int called = 0;

            fb.GetCompleted += (o, e) => ++called;

            fb.GetAsync("/4");

            Assert.Equal(1, called);
        }

        [Fact]
        public void Async_DoesNotCallPostCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.ReturnsJson("{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}");

            var fb = mockFb.Object;
            int called = 0;

            fb.PostCompleted += (o, e) => ++called;

            fb.GetAsync("/4");

            Assert.Equal(0, called);
        }

        [Fact]
        public void Async_DoesNotCallDeleteCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.ReturnsJson("{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}");

            var fb = mockFb.Object;
            int called = 0;

            fb.DeleteCompleted += (o, e) => ++called;

            fb.GetAsync("/4");

            Assert.Equal(0, called);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsNotOpen_GetCompletedIsCalled()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.NoInternetConnection();

            int called = 0;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.GetCompleted += (o, e) =>
                                                             {
                                                                 called++;
                                                                 evt.Set();
                                                             };
                                  },
                              () => fb.GetAsync("/4"), 5000);

            Assert.Equal(1, called);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsNotOpen_PostCompletedIsNotCalled()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            mockFb.NoInternetConnection();

            int called = 0;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.PostCompleted += (o, e) =>
                                                              {
                                                                  called++;
                                                                  evt.Set();
                                                              };
                                  },
                              () => fb.GetAsync("/4"), 5000);

            Assert.Equal(0, called);
        }
    }
}