using System;
using System.Runtime.Serialization;

namespace MoneyFox.Core._Pending_.Exceptions
{
    [Serializable]
    public class StartAfterEnddateException : Exception
    {
        public StartAfterEnddateException()
        {
        }

        protected StartAfterEnddateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public StartAfterEnddateException(string message) : base(message)
        {
        }

        public StartAfterEnddateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}