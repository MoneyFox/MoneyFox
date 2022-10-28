namespace MoneyFox.Core.ApplicationCore.Domain.Exceptions;

using System;
using System.Runtime.Serialization;

[Serializable]
public class NavigationException : Exception
{
    public NavigationException() { }

    public NavigationException(string message) : base(message) { }

    public NavigationException(string message, Exception innerException) : base(message: message, innerException: innerException) { }

    protected NavigationException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }
}
