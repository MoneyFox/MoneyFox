using System;
using System.Runtime.Serialization;

namespace MoneyFox.Core._Pending_.Exceptions
{
    [Serializable]
    public class InvalidEndDateException : Exception
    {
        public InvalidEndDateException()
        {
        }

        public InvalidEndDateException(string message) : base(message)
        {
        }

        public InvalidEndDateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidEndDateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}