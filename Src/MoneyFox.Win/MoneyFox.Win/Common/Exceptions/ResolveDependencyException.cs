namespace MoneyFox.Win.Common.Exceptions;

using System;

internal sealed class ResolveDependencyException<T> : Exception
{
    public ResolveDependencyException() : base($"Failed to resolve {typeof(T)}") { }
}
