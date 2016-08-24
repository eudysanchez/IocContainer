using System;
using System.Runtime.Serialization;

namespace IoCContainer.Exceptions
{
    [Serializable]
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException() : base() { }

        public TypeNotRegisteredException(string message) : base(message)
        {
        }

        public TypeNotRegisteredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeNotRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}