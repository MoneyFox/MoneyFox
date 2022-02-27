namespace MoneyFox.Core._Pending_.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class FailedToInitConfiFileException : Exception
    {
        public FailedToInitConfiFileException()
        {
        }

        protected FailedToInitConfiFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public FailedToInitConfiFileException(string message) : base(message)
        {
        }

        public FailedToInitConfiFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}