namespace MoneyFox.Common.Exceptions;

internal sealed class ResolveDependencyException<T> : Exception
{
    public ResolveDependencyException() : base($"Failed to resolve {typeof(T)}") { }
}
