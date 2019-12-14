using System;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    [Serializable]
    public class CanceledOperationException : Exception
    {
        public CanceledOperationException()
        {
        }

        public CanceledOperationException(string message) : base(message)
        {
        }

        public CanceledOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CanceledOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
