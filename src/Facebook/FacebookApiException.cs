// --------------------------------
// <copyright file="FacebookApiException.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security;

namespace Facebook
{
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class FacebookApiException : Exception
    {

        public string ErrorType { get; set; }

        public FacebookApiException()
            : base()
        {
        }

        public FacebookApiException(string message)
            : base(message)
        {
        }

        public FacebookApiException(string message, string errorType)
            : base(message)
        {
            this.ErrorType = errorType;
        }

#if (!SILVERLIGHT)
        protected FacebookApiException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
#endif
        public FacebookApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public FacebookApiException(string message, string errorType, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorType = errorType;
        }

        public override string ToString()
        {
            return string.Format("({0}) {1}", this.ErrorType ?? "Unknown", this.Message);
        }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("ErrorType", this.ErrorType);
            base.GetObjectData(info, context);
        }
    }
}
