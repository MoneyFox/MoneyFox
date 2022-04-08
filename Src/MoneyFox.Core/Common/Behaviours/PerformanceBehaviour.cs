namespace MoneyFox.Core.Common.Behaviours
{

    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using NLog;

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    {
        private const int THRESHOLD_LONG_RUNNING_REQUEST_MS = 500;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Stopwatch timer;

        public PerformanceBehaviour()
        {
            timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            timer.Start();
            var response = await next();
            timer.Stop();
            var elapsedMilliseconds = timer.ElapsedMilliseconds;
            if (elapsedMilliseconds > THRESHOLD_LONG_RUNNING_REQUEST_MS)
            {
                var requestName = typeof(TRequest).Name;
                logger.Warn(
                    message: "MoneyFox Long Running Request: {Name} \tElapsedTime: ({ElapsedMilliseconds} milliseconds) \tRequestData: {@Request}",
                    argument1: requestName,
                    argument2: elapsedMilliseconds,
                    argument3: request);
            }

            return response;
        }
    }

}
