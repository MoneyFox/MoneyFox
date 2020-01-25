using System;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    [Serializable]
    public class InvalidPaymentTypeException : Exception
    {
        public InvalidPaymentTypeException()
        { }

        public InvalidPaymentTypeException(string message) : base(message)
        { }

        public InvalidPaymentTypeException(string message, Exception innerException) : base(message, innerException)
        { }

        protected InvalidPaymentTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
