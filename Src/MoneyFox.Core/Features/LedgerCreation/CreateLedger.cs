namespace MoneyFox.Core.Features.LedgerCreation;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain;
using Domain.Aggregates.LedgerAggregate;
using MediatR;

public static class CreateLedger
{
    public sealed record Command : IRequest
    {
        public Command(string name, Money currentBalance, string? note, bool isExcludeFromEndOfMonthSummary)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required", nameof(name));
            }

            Name = name;
            CurrentBalance = currentBalance;
            Note = note;
            IsExcludeFromEndOfMonthSummary = isExcludeFromEndOfMonthSummary;
        }

        public string Name { get; }
        public Money CurrentBalance { get; }
        public string? Note { get; }
        public bool IsExcludeFromEndOfMonthSummary { get; }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext dbContext;

        public Handler(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var ledger = Ledger.Create(
                name: command.Name,
                openingBalance: command.CurrentBalance,
                note: command.Note,
                isExcluded: command.IsExcludeFromEndOfMonthSummary);

            dbContext.Add(ledger);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
