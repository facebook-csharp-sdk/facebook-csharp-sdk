using System;
using System.Collections.Generic;
using Facebook;

namespace $rootnamespace$.Samples.Facebook
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

                var result = (IList<object>)fb.Batch(
                    new FacebookBatchParameter("me"),
                    new FacebookBatchParameter(HttpMethod.Get, "me/friends", new { limit = 10 }));

                var result0 = result[0];
                var result1 = result[1];

                // Note: Always check first if each result set is an exeption.

                if (result0 is Exception)
                {
                    var ex = (Exception)result0;
                    // Note: make sure to handle this exception.
                    throw ex;
                }
                else
                {
                    var me = (IDictionary<string, object>)result0;
                    var name = (string)me["name"];

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
                    var friends = (IList<object>)((IDictionary<string, object>)result1)["data"];

                    Console.WriteLine("Some of your friends: ");

                    foreach (IDictionary<string, object> friend in friends)
                    {
                        Console.WriteLine(friend["name"]);
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

                var result = (IList<object>)fb.Batch(
                    new FacebookBatchParameter("/4"),
                    new FacebookBatchParameter().Query("SELECT name FROM user WHERE uid=me()"));

                var result0 = result[0];
                var result1 = result[1];

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
                    var fqlResult = (IList<object>)result1;

                    var fqlResult1 = (IDictionary<string, object>) fqlResult[0];
                    Console.WriteLine("Hi {0}", fqlResult1["name"]);
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

                var result = (IList<object>)fb.Batch(
                    new FacebookBatchParameter("/4"),
                    new FacebookBatchParameter().Query(
                        "SELECT first_name FROM user WHERE uid=me()",
                        "SELECT last_name FROM user WHERE uid=me()"));

                var result0 = result[0];
                var result1 = result[1];

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
                    var fqlResult = (IList<object>)result1;

                    var fqlResultSet0 = ((IDictionary<string, object>)fqlResult[0])["fql_result_set"];
                    Console.Write(fqlResultSet0);
                    Console.WriteLine();
                    var fqlResultSet1 = ((IDictionary<string, object>)fqlResult[1])["fql_result_set"];
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