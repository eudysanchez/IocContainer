using System;
using System.Runtime.Serialization;

namespace IoCContainer.ContainerFolder
{
    [Serializable]
    public class MultipleImplementationsException : Exception
    {
        public MultipleImplementationsException() : base() { }

        public MultipleImplementationsException(string message) : base(message)
        {
        }

        public MultipleImplementationsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MultipleImplementationsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}