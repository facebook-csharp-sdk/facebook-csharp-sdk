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

    public class ProcessBatchResultTests
    {
        private readonly object _json;

        public ProcessBatchResultTests()
        {
            _json = JsonSerializer.Current.DeserializeObject("[{\"code\":200,\"headers\":[{\"name\":\"Content-Type\",\"value\":\"text\\/javascript; charset=UTF-8\"},{\"name\":\"ETag\",\"value\":\"\\\"e4fca032be6bf50177f2455f2bd6f1be1ed66d5e\\\"\"}],\"body\":\"{\\\"id\\\":\\\"4\\\",\\\"name\\\":\\\"Mark Zuckerberg\\\",\\\"first_name\\\":\\\"Mark\\\",\\\"last_name\\\":\\\"Zuckerberg\\\",\\\"link\\\":\\\"http:\\\\\\/\\\\\\/www.facebook.com\\\\\\/zuck\\\",\\\"username\\\":\\\"zuck\\\",\\\"gender\\\":\\\"male\\\",\\\"locale\\\":\\\"en_US\\\",\\\"updated_time\\\":\\\"2011-03-20T22:48:50+0000\\\"}\"},{\"code\":400,\"headers\":[{\"name\":\"WWW-Authenticate\",\"value\":\"OAuth \\\"Facebook Platform\\\" \\\"invalid_request\\\" \\\"Unknown path components: \\/friend\\\"\"},{\"name\":\"HTTP\\/1.1\",\"value\":\"400 Bad Request\"},{\"name\":\"Cache-Control\",\"value\":\"no-store\"},{\"name\":\"Content-Type\",\"value\":\"text\\/javascript; charset=UTF-8\"}],\"body\":\"{\\\"error\\\":{\\\"type\\\":\\\"OAuthException\\\",\\\"message\\\":\\\"Unknown path components: \\\\\\/friend\\\"}}\"},{\"code\":200,\"headers\":[{\"name\":\"Content-Type\",\"value\":\"text\\/javascript; charset=UTF-8\"},{\"name\":\"ETag\",\"value\":\"\\\"be154857f6cafd69942e2793637c04123f939be0\\\"\"}],\"body\":\"{\\\"data\\\":[{\\\"name\\\":\\\"Jimmi Hendrix\\\",\\\"id\\\":\\\"100001241534829\\\"}],\\\"paging\\\":{\\\"next\\\":\\\"https:\\\\\\/\\\\\\/graph.facebook.com\\\\\\/me\\\\\\/friends?access_token=131403313556860\\\\u00257C6c88dcd4391c0d453e70b214-100001327642026\\\\u00257CseENNAUAe-P64skHn3K66vruN84&limit=1&offset=1\\\"}}\"},{\"code\":200,\"headers\":[{\"name\":\"Content-Type\",\"value\":\"text\\/javascript; charset=UTF-8\"},{\"name\":\"ETag\",\"value\":\"\\\"fb1b336b4162e64befd0deaabe26660290492b4e\\\"\"}],\"body\":\"{\\\"100001241534829\\\":{\\\"id\\\":\\\"100001241534829\\\",\\\"name\\\":\\\"Jimmi Hendrix\\\",\\\"first_name\\\":\\\"Jimmi\\\",\\\"last_name\\\":\\\"Hendrix\\\",\\\"link\\\":\\\"http:\\\\\\/\\\\\\/www.facebook.com\\\\\\/profile.php?id=100001241534829\\\",\\\"birthday\\\":\\\"01\\\\\\/01\\\\\\/1985\\\",\\\"location\\\":{\\\"id\\\":\\\"110585945628334\\\",\\\"name\\\":\\\"Bangkok, Thailand\\\"},\\\"locale\\\":\\\"en_US\\\",\\\"updated_time\\\":\\\"2011-03-19T00:17:53+0000\\\"}}\"},{\"code\":200,\"headers\":[{\"name\":\"P3P\",\"value\":\"CP=\\\"Facebook does not have a P3P policy. Learn why here: http:\\/\\/fb.me\\/p3p\\\"\"},{\"name\":\"Content-Type\",\"value\":\"text\\/javascript; charset=UTF-8\"},{\"name\":\"ETag\",\"value\":\"\\\"b11576956dcb07c3057f3e68a5cbd6dad1de523d\\\"\"}],\"body\":\"{\\\"data\\\":[{\\\"id\\\":\\\"100001241534829_122954607755923\\\",\\\"from\\\":{\\\"name\\\":\\\"Jimmi Hendrix\\\",\\\"id\\\":\\\"100001241534829\\\"},\\\"to\\\":{\\\"data\\\":[{\\\"name\\\":\\\"C# Sample 2\\\",\\\"id\\\":\\\"131403313556860\\\"}]},\\\"picture\\\":\\\"http:\\\\\\/\\\\\\/b.static.ak.fbcdn.net\\\\\\/rsrc.php\\\\\\/v1\\\\\\/yE\\\\\\/r\\\\\\/tKlGLd_GmXe.png\\\",\\\"link\\\":\\\"http:\\\\\\/\\\\\\/www.facebook.com\\\\\\/event.php?eid=151436411540845\\\",\\\"name\\\":\\\"xy15\\\",\\\"properties\\\":[{\\\"text\\\":\\\"Wednesday, September 1, 2010 at 3:14pm\\\"}],\\\"icon\\\":\\\"http:\\\\\\/\\\\\\/b.static.ak.fbcdn.net\\\\\\/rsrc.php\\\\\\/v1\\\\\\/yW\\\\\\/r\\\\\\/r28KD-9uEMh.gif\\\",\\\"type\\\":\\\"link\\\",\\\"object_id\\\":\\\"151436411540845\\\",\\\"created_time\\\":\\\"2010-09-02T12:15:14+0000\\\",\\\"updated_time\\\":\\\"2010-09-02T12:15:14+0000\\\"},{\\\"id\\\":\\\"100001241534829_122952631089454\\\",\\\"from\\\":{\\\"name\\\":\\\"Jimmi Hendrix\\\",\\\"id\\\":\\\"100001241534829\\\"},\\\"to\\\":{\\\"data\\\":[{\\\"name\\\":\\\"C# Sample 2\\\",\\\"id\\\":\\\"131403313556860\\\"}]},\\\"picture\\\":\\\"http:\\\\\\/\\\\\\/b.static.ak.fbcdn.net\\\\\\/rsrc.php\\\\\\/v1\\\\\\/yE\\\\\\/r\\\\\\/tKlGLd_GmXe.png\\\",\\\"link\\\":\\\"http:\\\\\\/\\\\\\/www.facebook.com\\\\\\/event.php?eid=122691847782346\\\",\\\"name\\\":\\\"xy15\\\",\\\"properties\\\":[{\\\"text\\\":\\\"Friday, September 3, 2010 at 2:04am\\\"}],\\\"icon\\\":\\\"http:\\\\\\/\\\\\\/b.static.ak.fbcdn.net\\\\\\/rsrc.php\\\\\\/v1\\\\\\/yW\\\\\\/r\\\\\\/r28KD-9uEMh.gif\\\",\\\"type\\\":\\\"link\\\",\\\"object_id\\\":\\\"122691847782346\\\",\\\"created_time\\\":\\\"2010-09-02T12:05:01+0000\\\",\\\"updated_time\\\":\\\"2010-09-02T12:05:01+0000\\\"},{\\\"id\\\":\\\"100001241534829_122951701089547\\\",\\\"from\\\":{\\\"name\\\":\\\"Jimmi Hendrix\\\",\\\"id\\\":\\\"100001241534829\\\"},\\\"picture\\\":\\\"http:\\\\\\/\\\\\\/b.static.ak.fbcdn.net\\\\\\/rsrc.php\\\\\\/v1\\\\\\/yE\\\\\\/r\\\\\\/tKlGLd_GmXe.png\\\",\\\"link\\\":\\\"http:\\\\\\/\\\\\\/www.facebook.com\\\\\\/event.php?eid=104890806239072\\\",\\\"name\\\":\\\"xy14\\\",\\\"properties\\\":[{\\\"text\\\":\\\"Thursday, September 2, 2010 at 1:59pm\\\"}],\\\"icon\\\":\\\"http:\\\\\\/\\\\\\/b.static.ak.fbcdn.net\\\\\\/rsrc.php\\\\\\/v1\\\\\\/yW\\\\\\/r\\\\\\/r28KD-9uEMh.gif\\\",\\\"type\\\":\\\"link\\\",\\\"object_id\\\":\\\"104890806239072\\\",\\\"created_time\\\":\\\"2010-09-02T12:00:18+0000\\\",\\\"updated_time\\\":\\\"2010-09-02T12:00:18+0000\\\"},{\\\"id\\\":\\\"100001241534829_122951261089591\\\",\\\"from\\\":{\\\"name\\\":\\\"Jimmi Hendrix\\\",\\\"id\\\":\\\"100001241534829\\\"},\\\"to\\\":{\\\"data\\\":[{\\\"name\\\":\\\"C# Sample 2\\\",\\\"id\\\":\\\"131403313556860\\\"}]},\\\"picture\\\":\\\"http:\\\\\\/\\\\\\/b.static.ak.fbcdn.net\\\\\\/rsrc.php\\\\\\/v1\\\\\\/yE\\\\\\/r\\\\\\/tKlGLd_GmXe.png\\\",\\\"link\\\":\\\"http:\\\\\\/\\\\\\/www.facebook.com\\\\\\/event.php?eid=150623688295557\\\",\\\"name\\\":\\\"xy13\\\",\\\"properties\\\":[{\\\"text\\\":\\\"Thursday, September 2, 2010 at 12:57pm\\\"}],\\\"icon\\\":\\\"http:\\\\\\/\\\\\\/b.static.ak.fbcdn.net\\\\\\/rsrc.php\\\\\\/v1\\\\\\/yW\\\\\\/r\\\\\\/r28KD-9uEMh.gif\\\",\\\"type\\\":\\\"link\\\",\\\"object_id\\\":\\\"150623688295557\\\",\\\"created_time\\\":\\\"2010-09-02T11:57:41+0000\\\",\\\"updated_time\\\":\\\"2010-09-02T11:57:41+0000\\\"},{\\\"id\\\":\\\"100001241534829_122951134422937\\\",\\\"from\\\":{\\\"name\\\":\\\"Jimmi Hendrix\\\",\\\"id\\\":\\\"100001241534829\\\"},\\\"to\\\":{\\\"data\\\":[{\\\"name\\\":\\\"C# Sample 2\\\",\\\"id\\\":\\\"131403313556860\\\"}]},\\\"picture\\\":\\\"http:\\\\\\/\\\\\\/b.static.ak.fbcdn.net\\\\\\/rsrc.php\\\\\\/v1\\\\\\/yE\\\\\\/r\\\\\\/tKlGLd_GmXe.png\\\",\\\"link\\\":\\\"http:\\\\\\/\\\\\\/www.facebook.com\\\\\\/event.php?eid=111345762257219\\\",\\\"name\\\":\\\"xy12\\\",\\\"properties\\\":[{\\\"text\\\":\\\"Wednesday, September 1, 2010 at 10:56pm\\\"}],\\\"icon\\\":\\\"http:\\\\\\/\\\\\\/b.static.ak.fbcdn.net\\\\\\/rsrc.php\\\\\\/v1\\\\\\/yW\\\\\\/r\\\\\\/r28KD-9uEMh.gif\\\",\\\"type\\\":\\\"link\\\",\\\"object_id\\\":\\\"111345762257219\\\",\\\"created_time\\\":\\\"2010-09-02T11:56:56+0000\\\",\\\"updated_time\\\":\\\"2010-09-02T11:56:56+0000\\\"}],\\\"paging\\\":{\\\"previous\\\":\\\"https:\\\\\\/\\\\\\/graph.facebook.com\\\\\\/100001241534829\\\\\\/feed?access_token=131403313556860\\\\u00257C6c88dcd4391c0d453e70b214-100001327642026\\\\u00257CseENNAUAe-P64skHn3K66vruN84&limit=5&since=1283429714\\\",\\\"next\\\":\\\"https:\\\\\\/\\\\\\/graph.facebook.com\\\\\\/100001241534829\\\\\\/feed?access_token=131403313556860\\\\u00257C6c88dcd4391c0d453e70b214-100001327642026\\\\u00257CseENNAUAe-P64skHn3K66vruN84&limit=5&until=1283428616\\\"}}\"},{\"code\":200,\"headers\":[{\"name\":\"Content-Type\",\"value\":\"application\\/json\"},{\"name\":\"Cache-Control\",\"value\":\"public, max-age=60\"},{\"name\":\"Expires\",\"value\":\"Mon, 21 Mar 2011 14:30:15 -0700\"},{\"name\":\"Pragma\",\"value\":\"\"},{\"name\":\"P3P\",\"value\":\"CP=\\\"Facebook does not have a P3P policy. Learn why here: http:\\/\\/fb.me\\/p3p\\\"\"}],\"body\":\"{\\\"error_code\\\":601,\\\"error_msg\\\":\\\"Parser error: unexpected end of query.\\\",\\\"request_args\\\":[{\\\"key\\\":\\\"query\\\",\\\"value\\\":\\\"SELECT name FROM user WHERE uid=\\\"},{\\\"key\\\":\\\"_fb_url\\\",\\\"value\\\":\\\"method\\\\\\/fql.query\\\"},{\\\"key\\\":\\\"access_token\\\",\\\"value\\\":\\\"131403313556860|6c88dcd4391c0d453e70b214-100001327642026|seENNAUAe-P64skHn3K66vruN84\\\"},{\\\"key\\\":\\\"format\\\",\\\"value\\\":\\\"json\\\"},{\\\"key\\\":\\\"method\\\",\\\"value\\\":\\\"fql_query\\\"}]}\"},{\"code\":200,\"headers\":[{\"name\":\"Content-Type\",\"value\":\"application\\/json\"},{\"name\":\"P3P\",\"value\":\"CP=\\\"Facebook does not have a P3P policy. Learn why here: http:\\/\\/fb.me\\/p3p\\\"\"}],\"body\":\"[{\\\"name\\\":\\\"query0\\\",\\\"fql_result_set\\\":[{\\\"first_name\\\":\\\"Jack\\\"}]},{\\\"name\\\":\\\"query1\\\",\\\"fql_result_set\\\":[{\\\"last_name\\\":\\\"Jonhson\\\"}]}]\"}]");
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
        public void CountIs7()
        {
            var list = (IList<object>)_json;

            Assert.Equal(7, list.Count);
        }

        [Fact]
        public void ProcessBatchRequest_IsTypeOfJsonArray()
        {
            var result = FacebookClient.ProcessBatchResult(_json);

            Assert.IsType<JsonArray>(result);
        }

        [Fact]
        public void ProcessBatchRequests_CountIs7()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);
            Assert.Equal(7, result.Count);
        }

        [Fact]
        public void Result0IsNotAnException()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);
            Assert.False(result[0] is Exception);
        }

        [Fact]
        public void Result1IsFacebookOAuthExcpetion()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);
            Assert.IsType<FacebookOAuthException>(result[1]);
        }

        [Fact]
        public void Result2IsNotAnException()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);
            Assert.False(result[2] is Exception);
        }

        [Fact]
        public void Result3IsNotAnException()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);
            Assert.False(result[3] is Exception);
        }

        [Fact]
        public void Result4IsNotAnException()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);
            Assert.False(result[4] is Exception);
        }

        [Fact]
        public void Result5IsNotAnException()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);
            Assert.IsType<FacebookApiException>(result[5]);
        }

        [Fact]
        public void Result6IsNotAnException()
        {
            var result = (IList<object>)FacebookClient.ProcessBatchResult(_json);
            Assert.False(result[6] is Exception);
        }
    }
}