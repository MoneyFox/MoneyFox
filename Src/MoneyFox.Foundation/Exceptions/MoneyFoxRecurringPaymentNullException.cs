using System;

namespace MoneyFox.Foundation.Exceptions
{
    public class MoneyFoxRecurringPaymentNullException : Exception
    {
        public MoneyFoxRecurringPaymentNullException()
        {
        }

        public MoneyFoxRecurringPaymentNullException(string message) : base(message)
        {
        }

        public MoneyFoxRecurringPaymentNullException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
