namespace MoneyFox.Core._Pending_.Exceptions
{
    using System;
    using System.Runtime.Serialization;

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