using System;
using System.Collections.Generic;

namespace Facebook
{
    /// <summary>
    /// Represents the Facebook Client.
    /// </summary>
    public interface IFacebookClient
    {
        /// <summary>
        /// Event handler for delete completion.
        /// </summary>
        event EventHandler<FacebookApiEventArgs> DeleteCompleted;

        /// <summary>
        /// Event handler for post completion.
        /// </summary>
        event EventHandler<FacebookApiEventArgs> PostCompleted;

        /// <summary>
        /// Event handler for get completion.
        /// </summary>
        event EventHandler<FacebookApiEventArgs> GetCompleted;

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use Facebook beta.
        /// </summary>
        bool UseFacebookBeta { get; set; }

#if (!SILVERLIGHT) // Silverlight should only have async calls

        /// <summary>
        /// Makes a DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException" />
        object Delete(string path);

        /// <summary>
        /// Makes a DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException" />
        object Delete(string path, IDictionary<string, object> parameters);

        /// <summary>
        /// Makes a GET requst to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException"/>
        object Get(string path);

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        object Get(string path, IDictionary<string, object> parameters);

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        object Get(IDictionary<string, object> parameters);

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <typeparam name="T">
        /// The result of the API call.
        /// </typeparam>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        T Get<T>(string path);

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="T">
        /// The result of the API call.
        /// </typeparam>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        T Get<T>(string path, IDictionary<string, object> parameters);

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="T">
        /// The result of the API call.
        /// </typeparam>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        T Get<T>(IDictionary<string, object> parameters);

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The jon result.
        /// </returns>
        object Post(string path, IDictionary<string, object> parameters);

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        object Post(IDictionary<string, object> parameters);

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        object Post(object parameters);

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        object Post(string path, object parameters);

#endif

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        void DeleteAsync(string path);

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void DeleteAsync(string path, IDictionary<string, object> parameters);

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        void DeleteAsync(string path, IDictionary<string, object> parameters, object userToken);

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        void GetAsync(string path);

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void GetAsync(string path, IDictionary<string, object> parameters);

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        void GetAsync(string path, IDictionary<string, object> parameters, object userToken);

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void GetAsync(IDictionary<string, object> parameters);

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void PostAsync(string path, IDictionary<string, object> parameters);

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        void PostAsync(string path, IDictionary<string, object> parameters, object userToken);

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void PostAsync(IDictionary<string, object> parameters);

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void PostAsync(string path, object parameters);

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        void PostAsync(string path, object parameters, object userToken);

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void PostAsync(object parameters);

#if (!SILVERLIGHT)

        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The FQL query result.
        /// </returns>
        object Query(string fql);

        /// <summary>
        /// Executes a FQL multiquery.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// A collection of the FQL query results.
        /// </returns>
        object Query(params string[] fql);

#endif

        /// <summary>
        /// Executes a FQL query asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        void QueryAsync(string fql);

        /// <summary>
        /// Executes a FQL query asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        void QueryAsync(string fql, object userToken);

        /// <summary>
        /// Executes a FQL multiquery asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        void QueryAsync(string[] fql);

        /// <summary>
        /// Executes a FQL multiquery asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <param name="userToken">
        /// The user Token.
        /// </param>
        void QueryAsync(string[] fql, object userToken);

#if !SILVERLIGHT

        /// <summary>
        /// Executes a batch request.
        /// </summary>
        /// <param name="batchParameters">
        /// The batch parameters.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        object Batch(params FacebookBatchParameter[] batchParameters);

#endif

        /// <summary>
        /// Executes a batch request asynchronously.
        /// </summary>
        /// <param name="batchParameters">
        /// The batch parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        void BatchAsync(FacebookBatchParameter[] batchParameters, object userToken);

        /// <summary>
        /// Executes a batch request asynchronously.
        /// </summary>
        /// <param name="batchParameters">
        /// The batch parameters.
        /// </param>
        void BatchAsync(FacebookBatchParameter[] batchParameters);
    }
}