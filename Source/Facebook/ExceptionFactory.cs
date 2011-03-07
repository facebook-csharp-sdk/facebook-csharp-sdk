// --------------------------------
// <copyright file="ExceptionFactory.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;

    /// <summary>
    /// A utility for generating facebook exceptions.
    /// </summary>
    internal static class ExceptionFactory
    {
        /// <summary>
        /// Gets the path or parameters required exception.
        /// </summary>
        /// <value>The path or parameters required exception.</value>
        internal static ArgumentException PathOrParametersRequired
        {
            get { return new ArgumentException("You must supply either the 'path' or 'parameters' argument."); }
        }

        /// <summary>
        /// Gets the method required for rest call exception.
        /// </summary>
        /// <value>The method required for rest call exception.</value>
        internal static ArgumentException MethodRequiredForRestCall
        {
            get { return new ArgumentException("A method must be specified in order to make a rest call."); }
        }

        /// <summary>
        /// Gets the cannot include multiple media objects exception.
        /// </summary>
        /// <value>The cannot include multiple media objects exception.</value>
        internal static InvalidOperationException CannotIncludeMultipleMediaObjects
        {
            get { return new InvalidOperationException("You cannot include more than one Facebook Media Object in a single request."); }
        }

        /// <summary>
        /// Gets the media object must have properties set exception.
        /// </summary>
        /// <value>The media object must have properties set exception.</value>
        internal static InvalidOperationException MediaObjectMustHavePropertiesSet
        {
            get { return new InvalidOperationException("The media object must have a content type, file name, and value set."); }
        }

        /// <summary>
        /// Gets the invalid cookie exception.
        /// </summary>
        /// <value>The invalid cookie exception.</value>
        internal static InvalidOperationException InvalidCookie
        {
            get { return new InvalidOperationException("The cookie value is invalid. The cookie contains multiple value sets."); }
        }

        /// <summary>
        /// Gets the rest exception if possible.
        /// </summary>
        /// <param name="result">The web request result object to check for exception information.</param>
        /// <returns>The Facebook API exception or null.</returns>
        internal static FacebookApiException GetRestException(object result)
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
        /// Checks for rest exception.
        /// </summary>
        /// <param name="domainMaps">
        /// The domain maps.
        /// </param>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="json">
        /// The json string.
        /// </param>
        /// <returns>
        /// Returns <see cref="FacebookApiException"/> if it is a rest exception otherwise null.
        /// </returns>
        internal static FacebookApiException CheckForRestException(IDictionary<string, Uri> domainMaps, Uri requestUri, string json)
        {
            Contract.Requires(requestUri != null);

            FacebookApiException error = null;

            // HACK: We have to do this because the REST Api doesn't return
            // the correct status codes when an error has occurred.
            if (FacebookUtils.IsUsingRestApi(domainMaps, requestUri))
            {
                // If we are using the REST API we need to check for an exception
                var resultObject = JsonSerializer.Current.DeserializeObject(json);
                error = GetRestException(resultObject);
            }

            return error;
        }


        /// <summary>
        /// Gets the graph exception if possible.
        /// </summary>
        /// <param name="result">The web request result object to check for exception information.</param>
        /// <returns>A Facebook API exception or null.</returns>
        internal static FacebookApiException GetGraphException(object result)
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
                    }
                }
            }

            return resultException;
        }

        /// <summary>
        /// Gets the graph exception if possible.
        /// </summary>
        /// <param name="exception">The web exception.</param>
        /// <returns>A Facebook API exception or null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We don't want to have any exceptions that are part of building the FacebookApiException throw.")]
        internal static FacebookApiException GetGraphException(WebExceptionWrapper exception)
        {
            Contract.Requires(exception != null);

            FacebookApiException resultException = null;
            try
            {
                if (exception.HasResponse)
                {
                    object response = null;
                    string json = null;
                    using (var stream = exception.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                json = reader.ReadToEnd();
                            }
                        }
                    }

                    if (json != null)
                    {
                        response = JsonSerializer.Current.DeserializeObject(json);
                    }

                    resultException = GetGraphException(response);
                }
            }
            catch
            {
                resultException = null;

                // We dont want to throw anything associated with 
                // trying to build the FacebookApiException
            }

            return resultException;
        }

        /// <summary>
        /// Gets the graph exception if possible.
        /// </summary>
        /// <param name="exception">The web exception.</param>
        /// <returns>A Facebook API exception or null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We don't want to have any exceptions that are part of building the FacebookApiException throw.")]
        internal static FacebookApiException GetGraphException(WebException exception)
        {
            Contract.Requires(exception != null);

            return GetGraphException(new WebExceptionWrapper(exception));
        }
    }
}