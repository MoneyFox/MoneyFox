namespace MoneyFox.Domain.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public class AccountNullException : Exception
{
    public AccountNullException() { }

    protected AccountNullException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }

    public AccountNullException(string message) : base(message) { }

    public AccountNullException(string message, Exception innerException) : base(message: message, innerException: innerException) { }
}
