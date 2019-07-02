using System;
using System.Runtime.Serialization;

namespace MoneyFox.Foundation.Exceptions
{
    [Serializable]
    public class RecurringPaymentNullException : Exception
    {
        public RecurringPaymentNullException()
        {
        }

        public RecurringPaymentNullException(string message) : base(message)
        {
        }

        public RecurringPaymentNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RecurringPaymentNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
