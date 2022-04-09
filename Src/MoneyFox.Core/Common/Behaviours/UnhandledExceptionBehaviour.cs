namespace MoneyFox.Core.Common.Behaviours
{

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using NLog;

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                logger.Error(exception: ex, message: "MoneyFox Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }

}
