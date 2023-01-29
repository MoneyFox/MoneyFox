namespace MoneyFox.Domain.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public class PaymentNotFoundException : Exception
{
    public PaymentNotFoundException() { }

    public PaymentNotFoundException(string message) : base(message) { }

    public PaymentNotFoundException(string message, Exception innerException) : base(message: message, innerException: innerException) { }

    protected PaymentNotFoundException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }
}
