using System;
using System.Collections.Generic;
using Facebook;

namespace $rootnamespace$.Samples.Facebook
{
    public static class LegacyRestApiSamples
    {
        public static void RunSamples(string accessToken)
        {
            GetSample(accessToken);

            var postId = PostToMyWall(accessToken, "message posted from Facebook C# SDK sample using rest api");

            Console.WriteLine();
            Console.WriteLine("Goto www.facebook.com and check if the message was posted in the wall. Then press any key to continue");
            Console.ReadKey();

            DeletePost(accessToken, postId);

            Console.WriteLine();
            Console.WriteLine("Goto www.facebook.com and check if the message was deleted in the wall. Then press any key to continue");
            Console.ReadKey();
        }

        public static void GetSample(string accessToken)
        {
            try
            {
                var fb = new FacebookClient(accessToken);

                var parameters = new Dictionary<string, object>
                                     {
                                         { "method", "pages.isFan" },
                                         { "page_id", "162171137156411" } // id of http://www.facebook.com/csharpsdk official page
                                     };

                var isFan = (bool)fb.Get(parameters);

                if (isFan)
                {
                    Console.WriteLine("You are a fan of http://www.facebook.com/csharpsdk");
                }
                else
                {
                    Console.WriteLine("You are not a fan of http://www.facebook.com/csharpsdk");
                }
            }
            catch (FacebookApiException ex)
            {
                // Note: make sure to handle this exception.
                throw;
            }
        }

        public static string PostToMyWall(string accessToken, string message)
        {
            try
            {
                var fb = new FacebookClient(accessToken);

                var parameters = new Dictionary<string, object>
                                     {
                                         { "method", "stream.publish" },
                                         { "message", message }
                                     };

                var result = fb.Post(parameters);
                var postId = (string)result;

                Console.WriteLine("Post Id: {0}", postId);

                // Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString());

                return postId;
            }
            catch (FacebookApiException ex)
            {
                // Note: make sure to handle this exception.
                throw;
            }
        }

        public static string DeletePost(string accessToken, string postId)
        {
            try
            {
                var fb = new FacebookClient(accessToken);

                var parameters = new Dictionary<string, object>
                                     {
                                         { "method", "stream.remove" },
                                         { "post_id", postId }
                                     };

                var result = fb.Post(parameters);

                // Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString());

                return postId;
            }
            catch (FacebookApiException ex)
            {
                // Note: make sure to handle this exception.
                throw;
            }
        }
    }
}