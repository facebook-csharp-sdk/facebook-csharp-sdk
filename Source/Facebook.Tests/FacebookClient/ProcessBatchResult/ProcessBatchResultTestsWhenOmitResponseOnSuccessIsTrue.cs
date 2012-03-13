//-----------------------------------------------------------------------
// <copyright file="<file>.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

namespace Facebook.Tests.FacebookClient.ProcessBatchResult
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class ProcessBatchResultTestsWhenOmitResponseOnSuccessIsTrue
    {
        private readonly object _json;

        public ProcessBatchResultTestsWhenOmitResponseOnSuccessIsTrue()
        {
            _json = JsonSerializer.Current.DeserializeObject("[null,{\"code\":200,\"headers\":[{\"name\":\"Cache-Control\",\"value\":\"private, no-cache, no-store, must-revalidate\"},{\"name\":\"Connection\",\"value\":\"close\"},{\"name\":\"Content-Type\",\"value\":\"text\\/javascript; charset=UTF-8\"},{\"name\":\"ETag\",\"value\":\"\\\"cdb63860b87133f4caf887558d51e0bdbbd1ba4c\\\"\"},{\"name\":\"Expires\",\"value\":\"Sat, 01 Jan 2000 00:00:00 GMT\"},{\"name\":\"P3P\",\"value\":\"CP=\\\"Facebook does not have a P3P policy. Learn why here: http:\\/\\/fb.me\\/p3p\\\"\"},{\"name\":\"Pragma\",\"value\":\"no-cache\"},{\"name\":\"Set-Cookie\",\"value\":\"datr=WRsgTjXc5Y9bQZF1GFJ9GejD; expires=Sun, 14-Jul-2013 10:50:01 GMT; path=\\/; domain=.facebook.com; httponly\"}],\"body\":\"{\\\"100002554230450\\\":{\\\"id\\\":\\\"100002554230450\\\",\\\"name\\\":\\\"Jack Johanathon\\\",\\\"first_name\\\":\\\"Jack\\\",\\\"last_name\\\":\\\"Johanathon\\\",\\\"link\\\":\\\"http:\\\\\\/\\\\\\/www.facebook.com\\\\\\/profile.php?id=100002554230450\\\",\\\"gender\\\":\\\"male\\\",\\\"timezone\\\":5.5,\\\"locale\\\":\\\"en_US\\\",\\\"updated_time\\\":\\\"2011-06-23T15:55:31+0000\\\"}}\"}]");
        }

        [Fact]
        public void ResultIsJsonArray()
        {
            Assert.IsType<JsonArray>(_json);
        }

        [Fact]
        public void ResultIsAssignableFromIListOfObject()
        {
            Assert.IsAssignableFrom<IList<object>>(_json);
        }

        [Fact]
        public void CountIs2()
        {
            var list = (IList<object>)_json;

            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void ProcessBatchRequest_IsTypeOfJsonArray()
        {
            var result = FacebookClient.ProcessBatchResult(_json);

            Assert.IsType<JsonArray>(result);
        }

        [Fact]
        public void ProcessBatchRequests_CountIs2()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Result0IsNotAnException()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);
            Assert.False(result[0] is Exception);
        }

        [Fact]
        public void Result0IsNull()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);

            Assert.Null(result[0]);
        }

        [Fact]
        public void ProcessBatchResultDoesNotThrowException()
        {
            Exception exception = null;

            try
            {
                FacebookClient.ProcessBatchResult(_json);
            }
            catch (NullReferenceException ex)
            {
                exception = ex;
            }

            Assert.Null(exception);
        }
    }
}