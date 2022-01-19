using System;
using System.Runtime.Serialization;

namespace MoneyFox.Core._Pending_.Exceptions
{
    [Serializable]
    public class PageNotFoundException : Exception
    {
        public PageNotFoundException()
        {
        }

        protected PageNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public PageNotFoundException(string message) : base(message)
        {
        }

        public PageNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}