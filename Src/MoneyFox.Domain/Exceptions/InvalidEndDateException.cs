namespace MoneyFox.Domain.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public class InvalidEndDateException : Exception
{
    public InvalidEndDateException() { }

    public InvalidEndDateException(string message) : base(message) { }

    public InvalidEndDateException(string message, Exception innerException) : base(message: message, innerException: innerException) { }

    protected InvalidEndDateException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }
}
