namespace MoneyFox.Domain.Exceptions;

public class NavigationException : Exception
{
    public NavigationException(string message, Exception innerException) : base(message: message, innerException: innerException) { }

    public NavigationException(Type type) : base(message: $"Could not navigate to {type}") { }
}
