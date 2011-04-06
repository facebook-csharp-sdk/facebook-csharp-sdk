using System;
using System.Dynamic;
using Facebook;

namespace $rootnamespace$.Samples.Facebook.Dynamic
{
    public static class GraphApiSamples
    {
        public static void RunSamples(string accessToken)
        {
            GetSampleWithoutAccessToken();
            GetSampleWithAccessToken(accessToken);

            var postId = PostToMyWall(accessToken, "message posted from Facebook C# SDK sample using graph api");

            Console.WriteLine();
            Console.WriteLine("Goto www.facebook.com and check if the message was posted in the wall. Then press any key to continue");
            Console.ReadKey();

            Delete(accessToken, postId);

            Console.WriteLine();
            Console.WriteLine("Goto www.facebook.com and check if the message was deleted in the wall. Then press any key to continue");
            Console.ReadKey();
        }

        public static void GetSampleWithoutAccessToken()
        {
            try
            {
                var fb = new FacebookClient();

                dynamic result = fb.Get("/4");

                dynamic id = result.id;
                dynamic name = result.name;
                dynamic firstName = result.first_name;
                dynamic lastName = result.last_name;

                Console.WriteLine("Id: {0}", id);
                Console.WriteLine("Name: {0}", name);
                Console.WriteLine("First Name: {0}", firstName);
                Console.WriteLine("Last Name: {0}", lastName);
                Console.WriteLine();

                // Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString());
            }
            catch (FacebookApiException ex)
            {
                // Note: make sure to handle this exception.
                throw;
            }
        }

        public static void GetSampleWithAccessToken(string accessToken)
        {
            try
            {
                var fb = new FacebookClient(accessToken);

                dynamic result = fb.Get("/me");

                dynamic id = result.id;
                dynamic name = result.name;
                dynamic firstName = result.first_name;
                dynamic lastName = result.last_name;

                Console.WriteLine("Id: {0}", id);
                Console.WriteLine("Name: {0}", name);
                Console.WriteLine("First Name: {0}", firstName);
                Console.WriteLine("Last Name: {0}", lastName);
                Console.WriteLine();

                // Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString());
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
                parameters.message = message;

                dynamic result = fb.Post("/me/feed", parameters);

                dynamic postId = result.id;

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

            return null;
        }

        public static void Delete(string accessToken, string id)
        {
            try
            {
                var fb = new FacebookClient(accessToken);

                dynamic result = fb.Delete(id);

                // Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString());
            }
            catch (FacebookApiException ex)
            {
                // Note: make sure to handle this exception.
                throw;
            }
        }

        public static string UploadPictureToWall(string accessToken, string filePath)
        {
            // sample usage: UploadPictureToWall(accessToken, @"C:\Users\Public\Pictures\Sample Pictures\Penguins.jpg");

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
                parameters.source = mediaObject;
                parameters.message = "photo";
                
                dynamic result = fb.Post("me/photos", parameters);

                dynamic postId = result.id;

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
    }
}