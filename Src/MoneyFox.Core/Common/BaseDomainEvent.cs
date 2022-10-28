namespace MoneyFox.Core.Common;

using System;
using MediatR;

public abstract class BaseDomainEvent : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
