namespace MoneyFox.Ui.Common.Exceptions;

internal sealed class ResolveDependencyException<T> : Exception
{
    public ResolveDependencyException() : base($"Failed to resolve {typeof(T)}") { }
}
