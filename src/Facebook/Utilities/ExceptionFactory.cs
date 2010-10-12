// --------------------------------
// <copyright file="ExceptionHelper.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net;

namespace Facebook.Utilities
{
    internal static class ExceptionFactory
    {
        internal static FacebookApiException GetRestException(object result)
        {
            // The REST API does not return a status that causes a WebException
            // even when there is an error. For this reason we have to parse a
            // successful response to see if it contains error infomration.
            // If it does have an error message we throw a FacebookApiException.

            FacebookApiException resultException = null;
            if (result != null)
            {
                var resultDict = result as IDictionary<string, object>;
                if (resultDict != null)
                {
                    if (resultDict.ContainsKey("error_code"))
                    {
                        string error_code = resultDict["error_code"] as String;
                        string error_msg = null;
                        if (resultDict.ContainsKey("error_msg"))
                        {
                            error_msg = resultDict["error_msg"] as String;
                        }

                        // Error Details: http://wiki.developers.facebook.com/index.php/Error_codes
                        if (error_code == "190")
                        {
                            resultException = new FacebookOAuthException(error_msg, error_code);
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We don't want to have any exceptions that are part of building the FacebookApiException throw.")]
        internal static FacebookApiException GetGraphException(WebException ex)
        {
            Contract.Requires(ex != null);
            Contract.Ensures(Contract.Result<FacebookApiException>() != null);
            Contract.EndContractBlock();

            FacebookApiException resultException = null;
            try
            {
                if (ex.Response != null)
                {
                    object response = null;
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            response = JsonSerializer.DeserializeObject(stream);
                        }
                    }
                    if (response != null)
                    {
                        var responseDict = response as IDictionary<string, object>;
                        if (responseDict != null)
                        {
                            if (responseDict.ContainsKey("error"))
                            {
                                var error = responseDict["error"] as IDictionary<string, object>;
                                if (error != null)
                                {
                                    var errorType = error["type"] as String;
                                    var errorMessage = error["message"] as String;
                                    // Check to make sure the correct data is in the response
                                    if (!String.IsNullOrEmpty(errorType) && !String.IsNullOrEmpty(errorMessage))
                                    {
                                        // We dont include the inner exception because it is not needed and is always a WebException.
                                        // It is easier to understand the error if we use Facebook's error message.
                                        if (errorType == "OAuthException")
                                        {
                                            resultException = new FacebookOAuthException(errorMessage, errorType);
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
                }
            }
            catch
            {
                resultException = null;
                // We dont want to throw anything associated with 
                // trying to build the FacebookApiException
            }

            //if (resultException == null)
            //{
            //    // If we have made it to here it means that either  
            //    // no detailed error message was recieved by facebook 
            //    // or the format of the message was not expected.
            //    resultException = new FacebookApiException("Unknown Facebook API Exception.", ex);
            //}

            return resultException;
        }

        internal static ArgumentException PathOrParametersRequired
        {
            get { return new ArgumentException("You must supply either the 'path' or 'parameters' argument."); }
        }

        internal static ArgumentException MethodRequiredForRestCall
        {
            get { return new ArgumentException("A method must be specified in order to make a rest call."); }
        }

        internal static InvalidOperationException CannotIncludeMultipleMediaObjects
        {
            get { return new InvalidOperationException("You cannot include more than one Facebook Media Object in a single request."); }
        }

        internal static InvalidOperationException MediaObjectMustHavePropertiesSet
        {
            get { return new InvalidOperationException("The media object must have a content type, file name, and value set."); }
        }

        internal static InvalidOperationException InvalidCookie
        {
            get { return new InvalidOperationException("The cookie value is invalid. The cookie contains multiple value sets."); }
        }
    }
}
