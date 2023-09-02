namespace MoneyFox.Domain.Exceptions;

public class NavigationException : Exception
{
    public NavigationException(string message, Exception innerException) : base(message: message, innerException: innerException) { }
}
