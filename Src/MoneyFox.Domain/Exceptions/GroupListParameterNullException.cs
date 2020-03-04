using System;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    [Serializable]
    public class GroupListParameterNullException : Exception
    {
        public GroupListParameterNullException()
        {
        }

        public GroupListParameterNullException(string message) : base(message)
        {
        }

        public GroupListParameterNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GroupListParameterNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
