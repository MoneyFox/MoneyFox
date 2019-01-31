using System;

namespace MoneyFox.Foundation.Exceptions
{
    public class MoneyFoxInvalidEndDateException : Exception
    {
        public MoneyFoxInvalidEndDateException()
        {
        }

        public MoneyFoxInvalidEndDateException(string message) : base(message)
        {
        }

        public MoneyFoxInvalidEndDateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
