namespace MoneyFox.Core.Features.RecurringTransactionUpdate;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain;
using Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class UpdateRecurringTransaction
{
    public record Command(
        Guid RecurringTransactionId,
        Money UpdatedAmount,
        int UpdatedCategoryId,
        Recurrence UpdatedRecurrence,
        DateOnly? UpdatedEndDate,
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
