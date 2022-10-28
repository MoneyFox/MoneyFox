namespace MoneyFox.Core.ApplicationCore.Domain.Exceptions;

using System;
using System.Runtime.Serialization;

[Serializable]
public class StartAfterEnddateException : Exception
{
    public StartAfterEnddateException() { }

    protected StartAfterEnddateException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }

    public StartAfterEnddateException(string message) : base(message) { }

    public StartAfterEnddateException(string message, Exception innerException) : base(message: message, innerException: innerException) { }
}
