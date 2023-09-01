namespace MoneyFox.Core.Features.TransactionRecurrence;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Extensions;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.RecurringTransactionAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentCreation;

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
                await CreateDueRecurrences(cancellationToken: cancellationToken, recurringTransaction: recurringTransaction);
            }
        }

        private async Task CreateDueRecurrences(CancellationToken cancellationToken, RecurringTransaction recurringTransaction)
        {
            while (true)
            {
                var dateAfterRecurrence = DateAfterRecurrence(recurringTransaction.LastRecurrence, recurringTransaction.Recurrence);
                if (dateAfterRecurrence <= systemDateHelper.TodayDateOnly.GetLastDayOfMonth())
                {
                    PaymentType paymentType;
                    if (recurringTransaction.IsTransfer)
                    {
                        paymentType = PaymentType.Transfer;
                    }
                    else
                    {
                        paymentType = recurringTransaction.Amount.Amount < 0 ? PaymentType.Expense : PaymentType.Income;
                    }

                    var createRecurringTransactionCommand = new CreatePayment.Command(
                        ChargedAccountId: recurringTransaction.ChargedAccountId,
                        TargetAccountId: recurringTransaction.TargetAccountId,
                        Amount: recurringTransaction.Amount,
                        Type: paymentType,
                        Date: dateAfterRecurrence,
                        CategoryId: recurringTransaction.CategoryId,
                        RecurringTransactionId: recurringTransaction.RecurringTransactionId,
                        Note: recurringTransaction.Note ?? string.Empty);

                    await sender.Send(request: createRecurringTransactionCommand, cancellationToken: cancellationToken);
                    recurringTransaction.SetLastRecurrence(dateAfterRecurrence);
                    continue;
                }

                break;
            }

            await dbContext.SaveChangesAsync(cancellationToken);
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
