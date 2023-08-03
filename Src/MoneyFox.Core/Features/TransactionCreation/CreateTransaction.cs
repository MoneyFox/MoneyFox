namespace MoneyFox.Core.Features.TransactionCreation;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Domain;
using MoneyFox.Domain.Aggregates;
using MoneyFox.Domain.Aggregates.RecurringTransactionAggregate;

internal static class CreateTransaction
{
    public record Command(
        Guid RecurringTransactionId,
        int ChargedAccount,
        int? TargetAccount,
        Money Amount,
        int? CategoryId,
        DateOnly StartDate,
        DateOnly? EndDate,
        Recurrence Recurrence,
        string? Note,
        bool IsLastDayOfMonth,
        bool IsTransfer) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var recurringTransaction = RecurringTransaction.Create(
                recurringTransactionId: command.RecurringTransactionId,
                chargedAccount: command.ChargedAccount,
                targetAccount: command.TargetAccount,
                amount: command.Amount,
                categoryId: command.CategoryId,
                startDate: command.StartDate,
                endDate: command.EndDate,
                recurrence: command.Recurrence,
                note: command.Note,
                isLastDayOfMonth: command.IsLastDayOfMonth,
                isTransfer: command.IsTransfer);

            appDbContext.Add(recurringTransaction);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
