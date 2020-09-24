using System;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    [Serializable]
    public class ConfigFailedToInitException : Exception
    {
        public ConfigFailedToInitException()
        {
        }

        protected ConfigFailedToInitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConfigFailedToInitException(string message) : base(message)
        {
        }

        public ConfigFailedToInitException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
