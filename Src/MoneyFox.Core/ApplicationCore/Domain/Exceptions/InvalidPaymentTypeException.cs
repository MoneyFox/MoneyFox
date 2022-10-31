namespace MoneyFox.Core.ApplicationCore.Domain.Exceptions;

using System;
using System.Runtime.Serialization;

[Serializable]
public class InvalidPaymentTypeException : Exception
{
    public InvalidPaymentTypeException() { }

    public InvalidPaymentTypeException(string message) : base(message) { }

    public InvalidPaymentTypeException(string message, Exception innerException) : base(message: message, innerException: innerException) { }

    protected InvalidPaymentTypeException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }
}
