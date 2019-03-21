using System;
using System.Runtime.Serialization;

namespace MoneyFox.Foundation.Exceptions
{
    [Serializable]
    public class MoneyFoxInvalidPaymentTypeException : Exception
    {
        public MoneyFoxInvalidPaymentTypeException()
        {
        }

        public MoneyFoxInvalidPaymentTypeException(string message) : base(message)
        {
        }

        public MoneyFoxInvalidPaymentTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MoneyFoxInvalidPaymentTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
