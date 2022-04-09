namespace MoneyFox.Core._Pending_.Exceptions
{

    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class PageNotFoundException : Exception
    {
        public PageNotFoundException() { }

        protected PageNotFoundException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }

        public PageNotFoundException(string message) : base(message) { }

        public PageNotFoundException(string message, Exception innerException) : base(message: message, innerException: innerException) { }
    }

}
