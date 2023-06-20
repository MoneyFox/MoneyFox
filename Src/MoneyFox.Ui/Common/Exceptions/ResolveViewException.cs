namespace MoneyFox.Ui.Common.Exceptions;

public sealed class ResolveViewException<T> : Exception
{
    public ResolveViewException() : base($"Failed to resolve View for {typeof(T)}") { }
}

public sealed class ResolveViewException : Exception
{
    public ResolveViewException(Type type) : base($"Failed to resolve View for {type}") { }
}
