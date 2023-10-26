namespace MoneyFox.Core.Features.PaymentCreation;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.CategoryAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class CreatePayment
{
    public record Command(
        int ChargedAccountId,
        int? TargetAccountId,
        Money Amount,
        PaymentType Type,
        DateOnly Date,
        int? CategoryId,
        Guid? RecurringTransactionId,
        string Note) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext context;

        public Handler(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var chargedAccount = await context.Accounts.SingleAsync(predicate: a => a.Id == command.ChargedAccountId, cancellationToken: cancellationToken);
            Account? targetAccount = null;
            if (command.TargetAccountId.HasValue)
            {
                targetAccount = await context.Accounts.SingleAsync(predicate: a => a.Id == command.TargetAccountId, cancellationToken: cancellationToken);
            }

            Category? category = null;
            if (command.CategoryId.HasValue)
            {
                category = await context.Categories.SingleAsync(predicate: a => a.Id == command.CategoryId, cancellationToken: cancellationToken);
            }

            var payment = new Payment(
                date: command.Date.ToDateTime(TimeOnly.MinValue),
                amount: Math.Abs(command.Amount.Amount),
                type: command.Type,
                chargedAccount: chargedAccount,
                targetAccount: targetAccount,
                category: category,
                note: command.Note,
                recurringTransactionId: command.RecurringTransactionId);

            context.Add(payment);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
