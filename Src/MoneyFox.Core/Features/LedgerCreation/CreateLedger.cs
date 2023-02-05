namespace MoneyFox.Core.Features.LedgerCreation;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain;
using Domain.Aggregates.LedgerAggregate;
using MediatR;

internal static class CreateLedger
{
    public sealed record Command(string Name, Money CurrentBalance, string? Note, bool IsExcludeFromEndOfMonthSummary) : IRequest<Unit>;

    public sealed class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext dbContext;

        public Handler(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var ledger = Ledger.Create(command.Name, command.CurrentBalance, command.Note, command.IsExcludeFromEndOfMonthSummary);
            dbContext.Add(ledger);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
