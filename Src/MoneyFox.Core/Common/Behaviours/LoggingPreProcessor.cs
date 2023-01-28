namespace MoneyFox.Core.Common.Behaviours;

using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.AppCenter.Analytics;
using Serilog;

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
public class LoggingPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        Log.Information(messageTemplate: "MoneyFox Request: {requestName} \tRequestData{@request} ", propertyValue0: requestName, propertyValue1: request);
        Analytics.TrackEvent(requestName);

        return Task.CompletedTask;
    }
}
