namespace MoneyFox.Core.Common.Behaviours;

using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Serilog;

[UsedImplicitly]
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            Log.Error(
                exception: ex,
                messageTemplate: "MoneyFox Request: Unhandled Exception for Request {Name} {@Request}",
                propertyValue0: requestName,
                propertyValue1: request);

            throw;
        }
    }
}
