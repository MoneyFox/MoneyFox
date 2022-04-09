namespace MoneyFox.Core.Common.Behaviours
{

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR.Pipeline;
    using NLog;

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    public class LoggingPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            logger.Info(message: "MoneyFox Request: {request} {@userName} \tRequestData{@Request} ", argument1: requestName, argument2: request);

            return Task.CompletedTask;
        }
    }

}
