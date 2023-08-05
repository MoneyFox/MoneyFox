namespace MoneyFox.Core.Features.RecurringTransactionUpdate;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;

public static class UpdateRecurringTransaction
{
    public record Command(Guid RecurringTransactionId) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Task Handle(Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
