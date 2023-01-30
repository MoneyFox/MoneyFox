namespace MoneyFox.Domain.Exceptions;

public sealed class InvalidArgumentException : Exception
{
    public InvalidArgumentException(string paramName, string message) : base($"{paramName}: {message}")
    {
        ParamName = paramName;
    }

    public string ParamName { get; }
}
