namespace MoneyFox.Domain.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public class GroupListParameterNullException : Exception
{
    public GroupListParameterNullException() { }

    public GroupListParameterNullException(string message) : base(message) { }

    public GroupListParameterNullException(string message, Exception innerException) : base(message: message, innerException: innerException) { }

    protected GroupListParameterNullException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }
}
