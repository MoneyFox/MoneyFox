namespace MoneyFox.Core._Pending_.Exceptions
{
    using System;
    using System.Runtime.Serialization;

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