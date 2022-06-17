namespace MoneyFox.Common.Exceptions
{

    using System;

    internal sealed class ResolveViewModeException<T> : Exception
    {
        public ResolveViewModeException() : base($"Failed to resolve ViewModel for {typeof(T)}") { }
    }

}
