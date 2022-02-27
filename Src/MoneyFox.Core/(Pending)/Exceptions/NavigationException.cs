namespace MoneyFox.Core._Pending_.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NavigationException : Exception
    {
        public NavigationException()
        {
        }

        public NavigationException(string message) : base(message)
        {
        }

        public NavigationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NavigationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}