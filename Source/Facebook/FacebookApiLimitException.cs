//-----------------------------------------------------------------------
// <copyright file="FacebookApiLimitException.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

namespace Facebook
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents errors that occur as a result of problems with the OAuth access token.
    /// </summary>
    public class FacebookApiLimitException : FacebookApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiLimitException"/> class. 
        /// </summary>
        public FacebookApiLimitException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiLimitException"/> class. 
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public FacebookApiLimitException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiLimitException"/> class. 
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorType">The error type.</param>
        public FacebookApiLimitException(string message, string errorType)
            : base(message, errorType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiLimitException"/> class. 
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public FacebookApiLimitException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
