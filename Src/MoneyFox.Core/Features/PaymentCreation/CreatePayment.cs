namespace MoneyFox.Core.Features.PaymentCreation;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Domain;
using MoneyFox.Domain.Aggregates.AccountAggregate;
using MoneyFox.Domain.Aggregates.CategoryAggregate;

public static class CreatePayment
{
    public record Command(int ChargedAccountId, int? TargetAccountId, Money Amount, PaymentType Type, DateOnly Date, int? CategoryId, Guid? RecurringTransactionId, string Note) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext context;

        public Handler(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var chargedAccount = await context.Accounts.SingleAsync(a => a.Id == command.ChargedAccountId, cancellationToken: cancellationToken);

            Account? targetAccount = null;
            if (command.TargetAccountId.HasValue)
            {
                targetAccount = await context.Accounts.SingleAsync(a => a.Id == command.TargetAccountId, cancellationToken: cancellationToken);
            }

            Category? category = null;
            if (command.CategoryId.HasValue)
            {
                category = await context.Categories.SingleAsync(a => a.Id == command.CategoryId, cancellationToken: cancellationToken);
            }

            var payment = new Payment(command.Date.ToDateTime(TimeOnly.MinValue), command.Amount.Amount, command.Type, chargedAccount, targetAccount, category, command.Note, command.RecurringTransactionId);

            context.Add(payment);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
