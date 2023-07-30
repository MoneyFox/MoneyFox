namespace MoneyFox.Core.Features.RecurringTransactionCreation;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Domain;
using MoneyFox.Domain.Aggregates;
using MoneyFox.Domain.Aggregates.AccountAggregate;
using MoneyFox.Domain.Aggregates.RecurringTransactionAggregate;

internal static class CreateRecurringTransaction
{
    public record Command(
        Guid RecurringTransactionId,
        int ChargedAccount,
        int? TargetAccount,
        Money Amount,
        int? CategoryId,
        PaymentType Type,
        DateOnly StartDate,
        DateOnly? EndDate,
        Recurrence Recurrence,
        string? Note,
        bool IsLastDayOfMonth) : IRequest;

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
                type: command.Type,
                startDate: command.StartDate,
                endDate: command.EndDate,
                recurrence: command.Recurrence,
                note: command.Note,
                isLastDayOfMonth: command.IsLastDayOfMonth);

            appDbContext.Add(recurringTransaction);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
