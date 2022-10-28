namespace MoneyFox.Core.Common.Mediatr;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class CustomMediator : Mediator
{
    private readonly Func<IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, CancellationToken, Task> publish;

    public CustomMediator(
        ServiceFactory serviceFactory,
        Func<IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, CancellationToken, Task> publish) : base(serviceFactory)
    {
        this.publish = publish;
    }

    protected override Task PublishCore(
        IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers,
        INotification notification,
        CancellationToken cancellationToken)
    {
        return publish(arg1: allHandlers, arg2: notification, arg3: cancellationToken);
    }
}
