namespace MoneyFox.Ui.Common.Exceptions;

public sealed class ResolveViewModelException<T> : Exception
{
    public ResolveViewModelException() : base($"Failed to resolve ViewModel for {typeof(T)}") { }
}
