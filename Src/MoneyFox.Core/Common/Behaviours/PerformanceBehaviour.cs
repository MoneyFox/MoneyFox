namespace MoneyFox.Core.Common.Behaviours;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Serilog;

[UsedImplicitly]
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private const int THRESHOLD_LONG_RUNNING_REQUEST_MS = 500;
    private readonly Stopwatch timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        timer.Start();
        var response = await next();
        timer.Stop();
        var elapsedMilliseconds = timer.ElapsedMilliseconds;
        if (elapsedMilliseconds > THRESHOLD_LONG_RUNNING_REQUEST_MS)
        {
            var requestName = typeof(TRequest).Name;
            Log.Warning(
                messageTemplate: "MoneyFox Long Running Request: {name} \tElapsedTime: ({elapsedMilliseconds} milliseconds) \tRequestData: {@request}",
                propertyValue0: requestName,
                propertyValue1: elapsedMilliseconds,
                propertyValue2: request);
        }

        return response;
    }
}
