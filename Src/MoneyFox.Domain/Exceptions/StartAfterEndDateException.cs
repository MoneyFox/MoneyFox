namespace MoneyFox.Domain.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public class StartAfterEndDateException : Exception
{
    public StartAfterEndDateException() { }

    protected StartAfterEndDateException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }

    public StartAfterEndDateException(string message) : base(message) { }

    public StartAfterEndDateException(string message, Exception innerException) : base(message: message, innerException: innerException) { }
}
