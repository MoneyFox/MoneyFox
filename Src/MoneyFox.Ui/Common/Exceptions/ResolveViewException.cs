namespace MoneyFox.Ui.Common.Exceptions;

public sealed class ResolveViewException<T> : Exception
{
    public ResolveViewException() : base($"Failed to resolve View for {typeof(T)}") { }
}

public sealed class ResolveViewException : Exception
{
    public ResolveViewException(Type type) : base($"Failed to resolve View for {type}") { }
}

public sealed class FindViewForViewModelException : Exception
{
    public FindViewForViewModelException(Type type) : base($"Failed to resolve ViewModel for view {type}") { }
}

public sealed class FindViewModelForViewException : Exception
{
    public FindViewModelForViewException(Type type) : base($"Failed to resolve View for viewModel {type}") { }
}
