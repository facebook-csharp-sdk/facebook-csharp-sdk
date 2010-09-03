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
    internal static class ExceptionHelper
    {
        internal static FacebookApiException GetRestException(object result)
        {
            // The REST API does not return a status that causes a WebException
            // even when there is an error. For this reason we have to parse a
            // successful response to see if it contains error infomration.
            // If it does have an error message we throw a FacebookApiException.

            FacebookApiException resultException = null;
            if (result != null && result is IDictionary<string, object>)
            {
                var resultDict = (IDictionary<string, object>)result;
                if (resultDict.ContainsKey("error_code"))
                {
                    string error_code = resultDict["error_code"].ToString();
                    string error_msg = null;
                    if (resultDict.ContainsKey("error_msg"))
                    {
                        error_msg = resultDict["error_msg"].ToString();
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
            return resultException;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We don't want to have any exceptions that are part of building the FacebookApiException throw.")]
        internal static FacebookApiException GetGraphException(WebException ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }
            Contract.Ensures(Contract.Result<FacebookApiException>() != null);
            Contract.EndContractBlock();

            FacebookApiException resultException = null;
            try
            {
                if (ex.Response != null)
                {
                    dynamic response = null;
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            response = JsonSerializer.DeserializeObject(stream);
                        }
                    }
                    if (response != null)
                    {
                        // Check to make sure the correct data is in the response
                        if (response.error.type != InvalidProperty.Instance && response.error.message != InvalidProperty.Instance)
                        {
                            // We dont include the inner exception because it is not needed and is always a WebException.
                            // It is easier to understand the error if we use Facebook's error message.
                            if (response.error.type == "OAuthException")
                            {
                                resultException = new FacebookOAuthException(response.error.message, response.error.type);
                            }
                            else
                            {
                                resultException = new FacebookApiException(response.error.message, response.error.type);
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

            if (resultException == null)
            {
                // If we have made it to here it means that either  
                // no detailed error message was recieved by facebook 
                // or the format of the message was not expected.
                resultException = new FacebookApiException("Unknown Facebook API Exception.", ex);
            }

            return resultException;
        }
    }
}
