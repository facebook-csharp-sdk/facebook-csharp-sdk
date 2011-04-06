using System;
using System.Dynamic;
using Facebook;

namespace $rootnamespace$.Samples.Facebook.Dynamic
{
    public static class LegacyRestApiSamples
    {
        public static void RunSamples(string accessToken)
        {
            GetSample(accessToken);

            dynamic postId = PostToMyWall(accessToken, "message posted from Facebook C# SDK sample using rest api");

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

                dynamic parameters = new ExpandoObject();
                parameters.method = "pages.isFan";
                parameters.page_id = "162171137156411"; // id of http://www.facebook.com/csharpsdk official page
                
                dynamic isFan =fb.Get(parameters);

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

                dynamic parameters = new ExpandoObject();
                parameters.method = "stream.publish";
                parameters.message = message;

                dynamic result = fb.Post(parameters);
                dynamic postId = result;

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

                dynamic parameters = new ExpandoObject();
                parameters.method = "stream.remove";
                parameters.post_id = postId;

                dynamic result = fb.Post(parameters);

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

        public static string UploadPhoto(string accessToken, string filePath)
        {
            // sample usage: UploadPhoto(accessToken, @"C:\Users\Public\Pictures\Sample Pictures\Penguins.jpg");

            var mediaObject = new FacebookMediaObject
            {
                FileName = System.IO.Path.GetFileName(filePath),
                ContentType = "image/jpeg"
            };

            mediaObject.SetValue(System.IO.File.ReadAllBytes(filePath));

            try
            {
                var fb = new FacebookClient(accessToken);

                dynamic parameters = new ExpandoObject();
                parameters.method = "facebook.photos.upload";
                parameters.caption = "photo upload using rest api";
                parameters.source = mediaObject;

                dynamic result = fb.Post(parameters);

                dynamic pictureId = result.pid;

                Console.WriteLine("Picture Id: {0}", pictureId);

                // Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString());

                return pictureId;
            }
            catch (FacebookApiException ex)
            {
                // Note: make sure to handle this exception.
                throw;
            }
        }
    }
}