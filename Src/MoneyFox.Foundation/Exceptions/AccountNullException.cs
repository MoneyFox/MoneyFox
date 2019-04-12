using System;
using System.Runtime.Serialization;

namespace MoneyFox.Foundation.Exceptions
{
    [Serializable]
    public class AccountNullException : Exception
    {
        public AccountNullException()
        {
        }

        protected AccountNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
