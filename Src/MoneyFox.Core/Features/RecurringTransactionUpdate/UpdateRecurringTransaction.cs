namespace MoneyFox.Core.Features.RecurringTransactionUpdate;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Interfaces;
using Domain;
using Domain.Aggregates;
using Domain.Aggregates.RecurringTransactionAggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class UpdateRecurringTransaction
{
    public record Command : IRequest
    {
        public Command(
            Guid recurringTransactionId,
            Money updatedAmount,
            int? updatedCategoryId,
            Recurrence updatedRecurrence,
            DateOnly? updatedEndDate,
            bool isLastDayOfMonth)
        {
            if (updatedEndDate != null && updatedEndDate < DateTime.Today.ToDateOnly())
            {
                throw new InvalidEndDateException();
            }

            RecurringTransactionId = recurringTransactionId;
            UpdatedAmount = updatedAmount;
            UpdatedCategoryId = updatedCategoryId;
            UpdatedRecurrence = updatedRecurrence;
            UpdatedEndDate = updatedEndDate;
            IsLastDayOfMonth = isLastDayOfMonth;
        }

        public Guid RecurringTransactionId { get; init; }
        public Money UpdatedAmount { get; init; }
        public int? UpdatedCategoryId { get; init; }
        public Recurrence UpdatedRecurrence { get; init; }
        public DateOnly? UpdatedEndDate { get; init; }
        public bool IsLastDayOfMonth { get; init; }
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
            var recurringTransaction = await appDbContext.RecurringTransactions.SingleAsync(
                predicate: rt => rt.RecurringTransactionId == command.RecurringTransactionId,
                cancellationToken: cancellationToken);

            recurringTransaction.UpdateRecurrence(
                updatedAmount: command.UpdatedAmount,
                updatedCategoryId: command.UpdatedCategoryId,
                updatedRecurrence: command.UpdatedRecurrence,
                updatedEndDate: command.UpdatedEndDate,
                updatedIsLastDayOfMonth: command.IsLastDayOfMonth);

            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
