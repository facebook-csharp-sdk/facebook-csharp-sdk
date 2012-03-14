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

namespace Facebook.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;

    public class FacebookIntegrationTests
    {
        private readonly string _accessToken;

        public FacebookIntegrationTests(string accessToken)
        {
            _accessToken = accessToken;
        }

        public void StartIntegrationTests()
        {
            // permissions required: publish_stream,user_about_me

            // Graph Api Tests
            GetTestWithoutAccessToken();
            GetTestWithAccessToken(_accessToken);
            DeleteTest(_accessToken, PostDictionaryTest(_accessToken).id);
            DeleteTest(_accessToken, PostExpandoObjectTest(_accessToken).id);
            BatchRequestNullReferenceExceptionWhenOmitReponseOnSuccessIsTrue(_accessToken);

            SingleMediaObjectUploadTests(_accessToken, File.ReadAllBytes(@"C:\Users\Public\Pictures\Sample Pictures\Koala.jpg"), "image/jpeg", "koala.jpg");
            GraphVideoUploadTests(_accessToken, File.ReadAllBytes(@"D:\Prabir\Downloads\do-beer-not-drugs.3gp"), "video/3gpp", "a.3gp");

            // Legacy Rest Api Tests
            LegacyRestApiTests();
            LegacyRestApiVideoUploadTests(_accessToken, File.ReadAllBytes(@"D:\Prabir\Downloads\do-beer-not-drugs.3gp"), "video/3gpp", "do-bee-not-drugs.3gp");

            // FQL tests (single query)
            FqlSingleQueryTests(_accessToken);

            // FQL tests (multi-query)
            FqlMultiQueryTests(_accessToken);
        }

        public dynamic GetTestWithAccessToken(string accessToken)
        {
            return Test("get test with access token", () =>
                                                          {
                                                              var fb = new FacebookClient(accessToken);
                                                              return fb.Get("/me");
                                                          });
        }

        public dynamic PostDictionaryTest(string accessToken)
        {
            return Test("post message with dictionary<string,object>",
                        () =>
                        {
                            var fb = new FacebookClient(accessToken);
                            return fb.Post("/me/feed",
                                           new Dictionary
                                               <string, object>
                                                   {
                                                       {
                                                           "message",
                                                           "dictionary<string,object> test from fb c# sdk"
                                                           }
                                                   });
                        });
        }

        public dynamic DeleteTest(string accessToken, string path)
        {
            return Test("delete test", () =>
                                           {
                                               var fb = new FacebookClient(accessToken);
                                               return fb.Delete(path);
                                           });
        }

        public dynamic PostExpandoObjectTest(string accessToken)
        {
            return Test("post message with dictionary<string,object>",
                        () =>
                        {
                            var fb = new FacebookClient(accessToken);
                            dynamic parameter = new ExpandoObject();
                            parameter.message = "dynamic expando object test from fb c# sdk";

                            return fb.Post("/me/feed", parameter);
                        });
        }

        private void BatchRequestNullReferenceExceptionWhenOmitReponseOnSuccessIsTrue(string accessToken)
        {
            Test("(#5883) Batch Request NullReferenceException when omit_response_on_success = true",
                       () =>
                       {
                           var fb = new FacebookClient(accessToken);

                           return fb.Batch(
                               new FacebookBatchParameter("/me") { Data = new { omit_respone_on_success = true, name = "get-uid" } },
                               new FacebookBatchParameter("/", new { ids = "result=get-uid:$..id" }));
                       });
        }

        public dynamic SingleMediaObjectUploadTests(string accessToken, byte[] data, string contentType, string fileName)
        {
            return Test("single media object upload tests",
                        () =>
                        {
                            var fb = new FacebookClient(accessToken);

                            var parameters = new Dictionary<string, object>();
                            parameters["source"] = new FacebookMediaObject
                                                    {
                                                        ContentType = contentType,
                                                        FileName = fileName
                                                    }.SetValue(data);
                            parameters["message"] = "single media object upload test";

                            return fb.Post("/me/photos", parameters);
                        });
        }

        public dynamic GraphVideoUploadTests(string accessToken, byte[] data, string contentType, string fileName)
        {
            return Test("graph video upload tests", () =>
                                                        {
                                                            var fb = new FacebookClient(accessToken);
                                                            var parameters = new Dictionary<string, object>();
                                                            parameters["source"] = new FacebookMediaObject
                                                                                       {
                                                                                           ContentType = contentType,
                                                                                           FileName = fileName
                                                                                       }.SetValue(data);
                                                            parameters["message"] = "graph video upload test";

                                                            return fb.Post("/me/videos", parameters);
                                                        });
        }

        public void LegacyRestApiTests()
        {
            Test("legacy rest api tests", () =>
            {
                var fb = new FacebookClient();

                var parameters = new Dictionary<string, object>();
                parameters["fields"] = new[] { "name" };
                parameters["uids"] = new[] { 4 };
                parameters["method"] = "users.getInfo";

                return fb.Get(parameters);
            });
        }

        public void LegacyRestApiVideoUploadTests(string accessToken, byte[] data, string contentType, string fileName)
        {
            Test("legacy rest api video upload tests",
                 () =>
                 {
                     var fb = new FacebookClient(accessToken);

                     var mediaObject = new FacebookMediaObject
                     {
                         ContentType = contentType,
                         FileName = fileName
                     }.SetValue(data);

                     var parameters = new Dictionary<string, object>();
                     parameters["source"] = mediaObject;
                     parameters["caption"] = "video upload using fb c# sdk";
                     parameters["method"] = "video.upload";

                     return fb.Post(parameters);
                 });
        }

        public dynamic GetTestWithoutAccessToken()
        {
            return Test("get test without access token", () =>
            {
                var fb = new FacebookClient();

                return fb.Get("4");
            });
        }

        public void FqlSingleQueryTests(string accessToken)
        {
            Test("fql single query test", () =>
                                              {
                                                  var fb = new FacebookClient(accessToken);

                                                  return fb.Query("SELECT uid,name FROM  user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1=me())");
                                              });
        }

        public dynamic FqlMultiQueryTests(string accessToken)
        {
            return Test("fql multi query test", () =>
                                             {
                                                 var fb = new FacebookClient(accessToken);

                                                 return fb.Query(
                                                     "SELECT uid,name FROM  user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1=me())",
                                                     "SELECT name FROM user WHERE uid=me()");
                                             });
        }

        private object Test(string testName, Func<object> testBlock)
        {
            return Test(testName, testBlock, false);
        }

        private object Test(string testName, Func<object> testBlock, bool continueOnError)
        {
            Console.WriteLine("Started - '{0}'", testName);

            object returnObject = null;
            try
            {
                returnObject = testBlock();
            }
            catch (Exception ex)
            {
                if (!continueOnError)
                    throw;
                Console.WriteLine("Error - {0}", ex.Message);
            }

            Console.WriteLine("Finished - '{0}'", testName);
            Console.WriteLine();
            Console.WriteLine();

            return returnObject;
        }

    }
}