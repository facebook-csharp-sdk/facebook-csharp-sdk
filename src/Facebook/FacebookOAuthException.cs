using System;
using System.Runtime.Serialization;

namespace Facebook
{
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class FacebookOAuthException : FacebookApiException
    {
        public FacebookOAuthException()
            : base()
        {
        }

        public FacebookOAuthException(string message)
            : base(message)
        {
        }

        public FacebookOAuthException(string message, string errorType)
            : base(message)
        {
            this.ErrorType = errorType;
        }
#if (!SILVERLIGHT)
        protected FacebookOAuthException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
        public FacebookOAuthException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public FacebookOAuthException(string message, string errorType, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorType = errorType;
        }
    }
}
