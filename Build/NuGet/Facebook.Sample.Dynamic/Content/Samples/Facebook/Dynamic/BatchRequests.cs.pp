using System;
using Facebook;

namespace $rootnamespace$.Samples.Facebook.Dynamic
{
    public static class BatchRequestsSamples
    {
        public static void RunSamples(string accessToken)
        {
            BatchRequest(accessToken);
            BatchRequestWithFql(accessToken);
            BatchRequestWithFqlMultiQuery(accessToken);
        }

        public static void BatchRequest(string accessToken)
        {
            try
            {
                var fb = new FacebookClient(accessToken);

                dynamic result = fb.Batch(
                    new FacebookBatchParameter("me"),
                    new FacebookBatchParameter(HttpMethod.Get, "me/friends", new { limit = 10 }));

                dynamic result0 = result[0];
                dynamic result1 = result[1];

                // Note: Always check first if each result set is an exeption.

                if (result0 is Exception)
                {
                    var ex = (Exception)result0;
                    // Note: make sure to handle this exception.
                    throw ex;
                }
                else
                {
                    dynamic me = result0;
                    dynamic name = me.name;

                    Console.WriteLine("Hi {0}", name);
                }

                Console.WriteLine();

                if (result1 is Exception)
                {
                    var ex = (Exception)result1;
                    // Note: make sure to handle this exception.
                    throw ex;
                }
                else
                {
                    dynamic friends = result1.data;

                    Console.WriteLine("Some of your friends: ");

                    foreach (dynamic friend in friends)
                    {
                        Console.WriteLine(friend.name);
                    }
                }
            }
            catch (FacebookApiException ex)
            {
                // Note: make sure to handle this exception.
                throw;
            }
        }

        public static void BatchRequestWithFql(string accessToken)
        {
            try
            {
                var fb = new FacebookClient(accessToken);

                dynamic result = fb.Batch(
                    new FacebookBatchParameter("/4"),
                    new FacebookBatchParameter().Query("SELECT name FROM user WHERE uid=me()"));

                dynamic result0 = result[0];
                dynamic result1 = result[1];

                // Note: Always check first if each result set is an exeption.

                if (result0 is Exception)
                {
                    var ex = (Exception)result0;
                    // Note: make sure to handle this exception.
                    throw ex;
                }
                else
                {
                    Console.WriteLine("Batch Result 0: {0}", result0);
                }

                Console.WriteLine();

                if (result1 is Exception)
                {
                    var ex = (Exception)result1;
                    // Note: make sure to handle this exception.
                    throw ex;
                }
                else
                {
                    dynamic fqlResult = result1;

                    dynamic fqlResult1 = fqlResult[0];
                    Console.WriteLine("Hi {0}", fqlResult1.name);
                }
            }
            catch (FacebookApiException ex)
            {
                // Note: make sure to handle this exception.
                throw;
            }
        }

        public static void BatchRequestWithFqlMultiQuery(string accessToken)
        {
            try
            {
                var fb = new FacebookClient(accessToken);

                dynamic result = fb.Batch(
                    new FacebookBatchParameter("/4"),
                    new FacebookBatchParameter().Query(
                        "SELECT first_name FROM user WHERE uid=me()",
                        "SELECT last_name FROM user WHERE uid=me()"));

                dynamic result0 = result[0];
                dynamic result1 = result[1];

                // Note: Always check first if each result set is an exeption.

                if (result0 is Exception)
                {
                    var ex = (Exception)result0;
                    // Note: make sure to handle this exception.
                    throw ex;
                }
                else
                {
                    Console.WriteLine("Batch Result 0: {0}", result0);
                }

                Console.WriteLine();

                if (result1 is Exception)
                {
                    var ex = (Exception)result1;
                    // Note: make sure to handle this exception.
                    throw ex;
                }
                else
                {
                    dynamic fqlResult = result1;

                    dynamic fqlResultSet0 = fqlResult[0].fql_result_set;
                    Console.Write(fqlResultSet0);
                    Console.WriteLine();
                    dynamic fqlResultSet1 = fqlResult[1].fql_result_set;
                    Console.WriteLine(fqlResultSet1);
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