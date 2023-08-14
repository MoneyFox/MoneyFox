namespace MoneyFox.Core.Features.TransactionRecurrence;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;
using Domain.Aggregates.RecurringTransactionAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RecurringTransactionCreation;

public static class CheckTransactionRecurrence
{
    public record Command : IRequest;

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext dbContext;
        private readonly ISender sender;
        private readonly ISystemDateHelper systemDateHelper;

        public Handler(IAppDbContext dbContext, ISender sender, ISystemDateHelper systemDateHelper)
        {
            this.dbContext = dbContext;
            this.sender = sender;
            this.systemDateHelper = systemDateHelper;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var recurringTransactions = await dbContext.RecurringTransactions.Where(rt => !rt.EndDate.HasValue || rt.EndDate > systemDateHelper.TodayDateOnly)
                .ToListAsync(cancellationToken);

            foreach (var recurringTransaction in recurringTransactions)
            {
                var createRecurringTransactionCommand = new CreateRecurringTransaction.Command(
                    recurringTransactionId: recurringTransaction.RecurringTransactionId,
                    chargedAccount: recurringTransaction.ChargedAccountId,
                    targetAccount: recurringTransaction.TargetAccountId,
                    amount: recurringTransaction.Amount,
                    categoryId: recurringTransaction.CategoryId,
                    startDate: recurringTransaction.StartDate,
                    endDate: recurringTransaction.EndDate,
                    recurrence: recurringTransaction.Recurrence,
                    note: recurringTransaction.Note,
                    isLastDayOfMonth: recurringTransaction.IsLastDayOfMonth,
                    isTransfer: recurringTransaction.IsTransfer);

                await CreateDueRecurrences(
                    cancellationToken: cancellationToken,
                    recurringTransaction: recurringTransaction,
                    createRecurringTransactionCommand: createRecurringTransactionCommand);
            }
        }

        private async Task CreateDueRecurrences(
            CancellationToken cancellationToken,
            RecurringTransaction recurringTransaction,
            CreateRecurringTransaction.Command createRecurringTransactionCommand)
        {
            var dateAfterRecurrence = recurringTransaction.LastRecurrence;
            while (true)
            {
                dateAfterRecurrence = DateAfterRecurrence(dateAfterRecurrence: dateAfterRecurrence, recurrence: recurringTransaction.Recurrence);
                if (dateAfterRecurrence <= systemDateHelper.TodayDateOnly)
                {
                    await sender.Send(request: createRecurringTransactionCommand, cancellationToken: cancellationToken);

                    continue;
                }

                break;
            }
        }

        private DateOnly DateAfterRecurrence(DateOnly dateAfterRecurrence, Recurrence recurrence)
        {
            return recurrence switch
            {
                Recurrence.Daily => dateAfterRecurrence.AddDays(1),
                Recurrence.Weekly => dateAfterRecurrence.AddDays(7),
                Recurrence.Biweekly => dateAfterRecurrence.AddDays(14),
                Recurrence.Monthly => dateAfterRecurrence.AddMonths(1),
                Recurrence.Bimonthly => dateAfterRecurrence.AddMonths(2),
                Recurrence.Quarterly => dateAfterRecurrence.AddMonths(3),
                Recurrence.Biannually => dateAfterRecurrence.AddMonths(6),
                Recurrence.Yearly => dateAfterRecurrence.AddYears(1),
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(recurrence), actualValue: recurrence, message: null)
            };
        }
    }
}
