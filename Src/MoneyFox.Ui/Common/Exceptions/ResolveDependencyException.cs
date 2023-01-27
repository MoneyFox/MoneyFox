namespace MoneyFox.Ui.Common.Exceptions;

public sealed class ResolveDependencyException<T> : Exception
{
    public ResolveDependencyException() : base($"Failed to resolve {typeof(T)}") { }
}
