namespace MoneyFox.Core.Common.Mediatr
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class CustomPublisher : ICustomPublisher
    {
        private readonly ServiceFactory serviceFactory;

        private IDictionary<PublishStrategy, IMediator> PublishStrategies = new Dictionary<PublishStrategy, IMediator>();

        public CustomPublisher(ServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            PublishStrategies[PublishStrategy.Async] = new CustomMediator(serviceFactory: this.serviceFactory, publish: AsyncContinueOnException);
            PublishStrategies[PublishStrategy.ParallelNoWait] = new CustomMediator(serviceFactory: this.serviceFactory, publish: ParallelNoWait);
            PublishStrategies[PublishStrategy.ParallelWhenAll] = new CustomMediator(serviceFactory: this.serviceFactory, publish: ParallelWhenAll);
            PublishStrategies[PublishStrategy.ParallelWhenAny] = new CustomMediator(serviceFactory: this.serviceFactory, publish: ParallelWhenAny);
            PublishStrategies[PublishStrategy.SyncContinueOnException]
                = new CustomMediator(serviceFactory: this.serviceFactory, publish: SyncContinueOnException);

            PublishStrategies[PublishStrategy.SyncStopOnException] = new CustomMediator(serviceFactory: this.serviceFactory, publish: SyncStopOnException);
        }

        public PublishStrategy DefaultStrategy { get; set; } = PublishStrategy.SyncContinueOnException;

        public Task Publish<TNotification>(TNotification notification)
        {
            return Publish(notification: notification, strategy: DefaultStrategy, cancellationToken: default);
        }

        public Task Publish<TNotification>(TNotification notification, PublishStrategy strategy)
        {
            return Publish(notification: notification, strategy: strategy, cancellationToken: default);
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken)
        {
            return Publish(notification: notification, strategy: DefaultStrategy, cancellationToken: cancellationToken);
        }

        public Task Publish<TNotification>(TNotification notification, PublishStrategy strategy, CancellationToken cancellationToken)
        {
            if (!PublishStrategies.TryGetValue(key: strategy, value: out var mediator))
            {
                throw new ArgumentException($"Unknown strategy: {strategy}");
            }

            return mediator.Publish(notification: notification, cancellationToken: cancellationToken);
        }

        private Task ParallelWhenAll(
            IEnumerable<Func<INotification, CancellationToken, Task>> handlers,
            INotification notification,
            CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            foreach (var handler in handlers)
            {
                tasks.Add(Task.Run(() => handler(arg1: notification, arg2: cancellationToken)));
            }

            return Task.WhenAll(tasks);
        }

        private Task ParallelWhenAny(
            IEnumerable<Func<INotification, CancellationToken, Task>> handlers,
            INotification notification,
            CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            foreach (var handler in handlers)
            {
                tasks.Add(Task.Run(() => handler(arg1: notification, arg2: cancellationToken)));
            }

            return Task.WhenAny(tasks);
        }

        private Task ParallelNoWait(
            IEnumerable<Func<INotification, CancellationToken, Task>> handlers,
            INotification notification,
            CancellationToken cancellationToken)
        {
            foreach (var handler in handlers)
            {
                Task.Run(() => handler(arg1: notification, arg2: cancellationToken));
            }

            return Task.CompletedTask;
        }

        private async Task AsyncContinueOnException(
            IEnumerable<Func<INotification, CancellationToken, Task>> handlers,
            INotification notification,
            CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            var exceptions = new List<Exception>();
            foreach (var handler in handlers)
            {
                try
                {
                    tasks.Add(handler(arg1: notification, arg2: cancellationToken));
                }
                catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
                {
                    exceptions.Add(ex);
                }
            }

            try
            {
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (AggregateException ex)
            {
                exceptions.AddRange(ex.Flatten().InnerExceptions);
            }
            catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
            {
                exceptions.Add(ex);
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        private async Task SyncStopOnException(
            IEnumerable<Func<INotification, CancellationToken, Task>> handlers,
            INotification notification,
            CancellationToken cancellationToken)
        {
            foreach (var handler in handlers)
            {
                await handler(arg1: notification, arg2: cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task SyncContinueOnException(
            IEnumerable<Func<INotification, CancellationToken, Task>> handlers,
            INotification notification,
            CancellationToken cancellationToken)
        {
            var exceptions = new List<Exception>();
            foreach (var handler in handlers)
            {
                try
                {
                    await handler(arg1: notification, arg2: cancellationToken).ConfigureAwait(false);
                }
                catch (AggregateException ex)
                {
                    exceptions.AddRange(ex.Flatten().InnerExceptions);
                }
                catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }
    }

}
