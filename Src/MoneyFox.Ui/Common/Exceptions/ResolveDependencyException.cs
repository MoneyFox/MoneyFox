namespace MoneyFox.Ui.Common.Exceptions;

public sealed class ResolveDependencyException(Type type) : Exception($"Failed to resolve {type}");
