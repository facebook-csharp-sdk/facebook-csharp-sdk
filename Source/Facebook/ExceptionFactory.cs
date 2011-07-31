// --------------------------------
// <copyright file="ExceptionFactory.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A utility for generating Facebook exceptions.
    /// </summary>
    internal class ExceptionFactory
    {
        /// <summary>
        /// Gets the graph/rest api exception.
        /// </summary>
        /// <param name="domainMaps">The domain maps.</param>
        /// <param name="requestUri">The request uri.</param>
        /// <param name="responseString">The response string.</param>
        /// <param name="innerException">The actual web exception.</param>
        /// <param name="json">The json object.</param>
        /// <returns>The exception if found else null.</returns>
        public static Exception GetException(IDictionary<string, Uri> domainMaps, Uri requestUri, string responseString, Exception innerException, out object json)
        {
            json = null;
            try
            {
                json = JsonSerializer.Current.DeserializeObject(responseString);
                // we need to check for graph exception here again coz fb return 200ok
                // for https://graph.facebook.com/i_dont_exist
                return TryGetRestException(domainMaps, requestUri, json) ?? GetGraphException(json);
            }
            catch (Exception ex)
            {
                return innerException ?? ex;
            }
        }

        public static FacebookApiException TryGetRestException(IDictionary<string, Uri> domainMaps, Uri requestUri, object json)
        {
            FacebookApiException error = null;

            // HACK: We have to do this because the REST Api doesn't return
            // the correct status codes when an error has occurred.
            if (FacebookUtils.IsUsingRestApi(domainMaps, requestUri))
            {
                // If we are using the REST API we need to check for an exception
                error = GetRestException(json);
            }

            return error;
        }

        /// <summary>
        /// Gets the rest exception if possible.
        /// </summary>
        /// <param name="result">The web request result object to check for exception information.</param>
        /// <returns>The Facebook API exception or null.</returns>
        public static FacebookApiException GetRestException(object result)
        {
            // The REST API does not return a status that causes a WebException
            // even when there is an error. For this reason we have to parse a
            // successful response to see if it contains error information.
            // If it does have an error message we throw a FacebookApiException.
            FacebookApiException resultException = null;
            if (result != null)
            {
                var resultDict = result as IDictionary<string, object>;
                if (resultDict != null)
                {
                    if (resultDict.ContainsKey("error_code"))
                    {
                        string error_code = resultDict["error_code"].ToString();
                        string error_msg = null;
                        if (resultDict.ContainsKey("error_msg"))
                        {
                            error_msg = resultDict["error_msg"] as string;
                        }

                        // Error Details: http://wiki.developers.facebook.com/index.php/Error_codes
                        if (error_code == "190")
                        {
                            resultException = new FacebookOAuthException(error_msg, error_code);
                        }
                        else if (error_code == "4" || error_code == "API_EC_TOO_MANY_CALLS" || (error_msg != null && error_msg.Contains("request limit reached")))
                        {
                            resultException = new FacebookApiLimitException(error_msg, error_code);
                        }
                        else
                        {
                            resultException = new FacebookApiException(error_msg, error_code);
                        }
                    }
                }
            }

            return resultException;
        }

        /// <summary>
        /// Gets the graph exception if possible.
        /// </summary>
        /// <param name="result">The web request result object to check for exception information.</param>
        /// <returns>A Facebook API exception or null.</returns>
        public static FacebookApiException GetGraphException(object result)
        {
            // Note: broke down GetGraphException into different method for unit testing.
            FacebookApiException resultException = null;
            if (result != null)
            {
                var responseDict = result as IDictionary<string, object>;
                if (responseDict != null)
                {
                    if (responseDict.ContainsKey("error"))
                    {
                        var error = responseDict["error"] as IDictionary<string, object>;
                        if (error != null)
                        {
                            var errorType = error["type"] as string;
                            var errorMessage = error["message"] as string;

                            // Check to make sure the correct data is in the response
                            if (!String.IsNullOrEmpty(errorType) && !String.IsNullOrEmpty(errorMessage))
                            {
                                // We don't include the inner exception because it is not needed and is always a WebException.
                                // It is easier to understand the error if we use Facebook's error message.
                                if (errorType == "OAuthException")
                                {
                                    resultException = new FacebookOAuthException(errorMessage, errorType);
                                }
                                else if (errorType == "API_EC_TOO_MANY_CALLS" || (errorMessage != null && errorMessage.Contains("request limit reached")))
                                {
                                    resultException = new FacebookApiLimitException(errorMessage, errorType);
                                }
                                else
                                {
                                    resultException = new FacebookApiException(errorMessage, errorType);
                                }
                            }
                        }
                        else
                        {
                            long? errorNumber = null;
                            if (responseDict["error"] is long)
                                errorNumber = (long)responseDict["error"];
                            if (errorNumber == null && responseDict["error"] is int)
                                errorNumber = (int)responseDict["error"];
                            string errorDescription = null;
                            if (responseDict.ContainsKey("error_description"))
                                errorDescription = responseDict["error_description"] as string;
                            if (errorNumber != null && !string.IsNullOrEmpty(errorDescription))
                            {
                                if (errorNumber == 190)
                                {
                                    resultException = new FacebookOAuthException(errorDescription, "API_EC_PARAM_ACCESS_TOKEN");
                                }
                                else
                                {
                                    resultException = new FacebookApiException(errorDescription, errorNumber.Value.ToString());
                                }
                            }
                        }
                    }
                }
            }

            return resultException;
        }

        /// <summary>
        /// Gets the graph exception if possible.
        /// </summary>
        /// <param name="jsonString">The web request result string to check for exception information.</param>
        /// <param name="json">The json object.</param>
        /// <returns>A Facebook API exception or null.</returns>
        internal static FacebookApiException GetGraphException(string jsonString, out object json)
        {
            FacebookApiException resultException;

            try
            {
                json = JsonSerializer.Current.DeserializeObject(jsonString);
                resultException = GetGraphException(json);
            }
            catch
            {
                resultException = null;
                json = null;
                // We don't want to throw anything associated with 
                // trying to build the FacebookApiException
            }

            return resultException;
        }
    }
}