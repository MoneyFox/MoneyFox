using System;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    [Serializable]
    public class RecurrenceNullException : Exception
    {
        public RecurrenceNullException()
        {
        }

        protected RecurrenceNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RecurrenceNullException(string message) : base(message)
        {
        }

        public RecurrenceNullException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
