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
    }
}