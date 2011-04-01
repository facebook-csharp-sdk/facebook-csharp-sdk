using System;
using System.Collections.Generic;
using Facebook;

namespace $rootnamespace$.Samples.Facebook
{
    public static class FQLSamples
    {
        public static void RunSamples(string accessToken)
        {
            SingleQuery(accessToken);
        }

        public static void SingleQuery(string accessToken)
        {
            var query = string.Format("SELECT uid,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})", "me()");

            try
            {
                var fb = new FacebookClient(accessToken);

                var result = (IList<object>)fb.Query(query);

                foreach (var row in result)
                {
                    var r = (IDictionary<string, object>)row;
                    var uid = (string)r["uid"];
                    var picSquare = (string)r["pic_square"];

                    Console.WriteLine("User Id: {0}", uid);
                    Console.WriteLine("Picture Square: {0}", picSquare);
                    Console.WriteLine();
                }

                // Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString());
            }
            catch (FacebookApiException ex)
            {
                // Note: make sure to handle this exception.
                throw;
            }
        }     
	}
}