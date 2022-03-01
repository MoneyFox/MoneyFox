namespace MoneyFox.SharedKernel
{
    using MediatR;
    using System;

    public abstract class BaseDomainEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}