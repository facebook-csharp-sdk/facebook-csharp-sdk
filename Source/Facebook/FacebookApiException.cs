//-----------------------------------------------------------------------
// <copyright file="FacebookApiException.cs" company="The Outercurve Foundation">
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
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represent errors that occur while calling a Facebook API.
    /// </summary>
#if !(SILVERLIGHT || NETFX_CORE)
    [Serializable]
#endif
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly",
        Justification = "There are security issues associated with this method that make it difficult to support when running in partial trust.")]
    public class FacebookApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiException"/> class.
        /// </summary>
        public FacebookApiException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public FacebookApiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorType">Type of the error.</param>
        public FacebookApiException(string message, string errorType)
            : this(String.Format(CultureInfo.InvariantCulture, "({0}) {1}", errorType ?? "Unknown", message))
        {
            ErrorType = errorType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorType">Type of the error.</param>
        /// <param name="errorCode">Code of the error.</param>
        public FacebookApiException(string message, string errorType, int errorCode)
            : this(String.Format(CultureInfo.InvariantCulture, "({0} - #{1}) {2}", errorType ?? "Unknown", errorCode, message))
        {
            ErrorType = errorType;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorType">Type of the error.</param>
        /// <param name="errorCode">Code of the error.</param>
        /// <param name="errorSubcode">Subcode of the error.</param>
        public FacebookApiException(string message, string errorType, int errorCode, int errorSubcode)
            : this(message, errorType, errorCode)
        {
            ErrorSubcode = errorSubcode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public FacebookApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !(SILVERLIGHT || NETFX_CORE)
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected FacebookApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        /// <value>The type of the error.</value>
        public string ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the code of the error.
        /// </summary>
        /// <value>The code of the error.</value>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error subcode.
        /// </summary>
        /// <value>The code of the error subcode.</value>
        public int ErrorSubcode { get; set; }
    }
}
