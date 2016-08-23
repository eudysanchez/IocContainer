using System;
using System.Runtime.Serialization;

namespace IoCContainer.Exceptions
{
    [Serializable]
    internal class FailedToCreateInstanceException : Exception
    {
        public FailedToCreateInstanceException() : base() { }

        public FailedToCreateInstanceException(string message) : base(message)
        {
        }

        public FailedToCreateInstanceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FailedToCreateInstanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}