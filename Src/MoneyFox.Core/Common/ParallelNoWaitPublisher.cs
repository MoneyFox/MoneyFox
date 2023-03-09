namespace MoneyFox.Core.Common;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class ParallelNoWaitPublisher : INotificationPublisher
{
    public Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
    {
        foreach (var handler in handlerExecutors)
        {
            Task.Run(() => handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false), cancellationToken);
        }

        return Task.CompletedTask;
    }
}
