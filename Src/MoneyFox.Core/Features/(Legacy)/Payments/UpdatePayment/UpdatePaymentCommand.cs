namespace MoneyFox.Core.Features._Legacy_.Payments.UpdatePayment;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class UpdatePayment
{
    public record Command(
        int Id,
        DateTime Date,
        decimal Amount,
        PaymentType Type,
        string? Note,
        int CategoryId,
        int ChargedAccountId,
        int TargetAccountId) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var existingPayment = await appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .Include(x => x.Category)
                .FirstAsync(predicate: x => x.Id == command.Id, cancellationToken: cancellationToken);

            var chargedAccount = await appDbContext.Accounts.SingleAsync(
                predicate: a => a.Id == command.ChargedAccountId,
                cancellationToken: cancellationToken);

            var targetAccount = await appDbContext.Accounts.FindAsync(command.TargetAccountId);
            existingPayment.UpdatePayment(
                date: command.Date,
                amount: command.Amount,
                type: command.Type,
                chargedAccount: chargedAccount,
                targetAccount: targetAccount,
                category: await appDbContext.Categories.FindAsync(command.CategoryId),
                note: command.Note);

            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
