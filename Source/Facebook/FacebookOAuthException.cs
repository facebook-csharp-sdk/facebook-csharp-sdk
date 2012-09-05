//-----------------------------------------------------------------------
// <copyright file="FacebookOAuthException.cs" company="The Outercurve Foundation">
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
#if !(SILVERLIGHT || NETFX_CORE)
    [Serializable]
#endif
    public class FacebookOAuthException : FacebookApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        public FacebookOAuthException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public FacebookOAuthException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorType">The error type.</param>
        public FacebookOAuthException(string message, string errorType)
            : base(message, errorType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorType">Type of the error.</param>
        /// <param name="errorCode">Code of the error.</param>
        public FacebookOAuthException(string message, string errorType, int errorCode)
            : base(message, errorType, errorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorType">Type of the error.</param>
        /// <param name="errorCode">Code of the error.</param>
        /// <param name="errorSubcode">Subcode of the error.</param>
        public FacebookOAuthException(string message, string errorType, int errorCode, int errorSubcode)
            : base(message, errorType, errorCode, errorSubcode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public FacebookOAuthException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !(SILVERLIGHT || NETFX_CORE)
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookOAuthException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected FacebookOAuthException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
