namespace MoneyFox.Domain.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public class RecurringPaymentNullException : Exception
{
    public RecurringPaymentNullException() { }

    public RecurringPaymentNullException(string message) : base(message) { }

    public RecurringPaymentNullException(string message, Exception innerException) : base(message: message, innerException: innerException) { }

    protected RecurringPaymentNullException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }
}
