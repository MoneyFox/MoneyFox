namespace MoneyFox.Core._Pending_.Exceptions
{

    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class FailedToInitConfiFileException : Exception
    {
        public FailedToInitConfiFileException() { }

        protected FailedToInitConfiFileException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }

        public FailedToInitConfiFileException(string message) : base(message) { }

        public FailedToInitConfiFileException(string message, Exception innerException) : base(message: message, innerException: innerException) { }
    }

}
