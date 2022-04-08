namespace MoneyFox.Core._Pending_.Exceptions
{

    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class GraphClientNullException : Exception
    {
        public GraphClientNullException() { }

        public GraphClientNullException(string message) : base(message) { }

        public GraphClientNullException(string message, Exception exception) : base(message: message, innerException: exception) { }

        protected GraphClientNullException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }
    }

}
