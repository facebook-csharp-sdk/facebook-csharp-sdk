using System;
using System.Collections.Generic;
using Facebook;

namespace $rootnamespace$.Samples.Facebook.Dynamic
{
    public static class FQLSamples
    {
        public static void RunSamples(string accessToken)
        {
            SingleQuery(accessToken);
			
			Console.WriteLine();

            MultiQuery(accessToken);
        }

        public static void SingleQuery(string accessToken)
        {
            var query = string.Format("SELECT uid,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})", "me()");

            try
            {
                var fb = new FacebookClient(accessToken);

                dynamic result = fb.Query(query);

                foreach (var row in result)
                {
                    dynamic r = row;
                    dynamic uid = r.uid;
                    dynamic picSquare = r.pic_square;

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

        public static void MultiQuery(string accessToken)
        {
            var query1 = "SELECT uid FROM user WHERE uid=me()";
            var query2 = "SELECT profile_url FROM user WHERE uid=me()";

            try
            {
                var fb = new FacebookClient(accessToken);

                dynamic result = fb.Query(query1, query2);

                dynamic result0 = result[0].fql_result_set;
                dynamic result1 = result[1].fql_result_set;

                Console.WriteLine("Query 0 result: {0}", result0);
                Console.WriteLine();
                Console.WriteLine("Query 1 result: {0}", result1);
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
	}
}