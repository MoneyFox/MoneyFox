namespace MoneyFox.SharedKernel
{

    using System;
    using MediatR;

    public abstract class BaseDomainEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }

}
