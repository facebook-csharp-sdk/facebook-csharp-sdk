// --------------------------------
// <copyright file="FacebookOAuthException.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents errors that occur as a result of problems with the OAuth access token.
    /// </summary>
#if !(SILVERLIGHT || WINRT)
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
        /// <param name="innerException">The inner exception.</param>
        public FacebookOAuthException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !(SILVERLIGHT || WINRT)
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
