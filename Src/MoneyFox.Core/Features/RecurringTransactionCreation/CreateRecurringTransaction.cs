namespace MoneyFox.Core.Features.RecurringTransactionCreation;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Interfaces;
using Domain;
using Domain.Aggregates.RecurringTransactionAggregate;
using Domain.Exceptions;
using MediatR;

public static class CreateRecurringTransaction
{
    public record Command : IRequest
    {
        public Command(
            Guid recurringTransactionId,
            int chargedAccount,
            int? targetAccount,
            Money amount,
            int? categoryId,
            DateOnly startDate,
            DateOnly? endDate,
            Recurrence recurrence,
            string? note,
            bool isLastDayOfMonth,
            DateOnly lastRecurrence,
            bool isTransfer)
        {
            if (endDate != null && endDate < DateTime.Today.ToDateOnly())
            {
                throw new InvalidEndDateException();
            }

            RecurringTransactionId = recurringTransactionId;
            ChargedAccount = chargedAccount;
            TargetAccount = targetAccount;
            Amount = amount;
            CategoryId = categoryId;
            StartDate = startDate;
            EndDate = endDate;
            Recurrence = recurrence;
            Note = note;
            IsLastDayOfMonth = isLastDayOfMonth;
            LastRecurrence = lastRecurrence;
            IsTransfer = isTransfer;
        }

        public Guid RecurringTransactionId { get; init; }
        public int ChargedAccount { get; init; }
        public int? TargetAccount { get; init; }
        public Money Amount { get; init; }
        public int? CategoryId { get; init; }
        public DateOnly StartDate { get; init; }
        public DateOnly? EndDate { get; init; }
        public Recurrence Recurrence { get; init; }
        public string? Note { get; init; }
        public bool IsLastDayOfMonth { get; init; }
        public DateOnly LastRecurrence { get; init; }
        public bool IsTransfer { get; init; }
    }

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
                lastRecurrence: command.LastRecurrence,
                isTransfer: command.IsTransfer);

            appDbContext.Add(recurringTransaction);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
