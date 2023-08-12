namespace MoneyFox.Core.Features._Legacy_.Payments.UpdatePayment;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Interfaces;
using Common.Settings;
using Domain;
using Domain.Aggregates.AccountAggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RecurringTransactionUpdate;

public static class UpdatePayment
{
    public record Command(
        int Id,
        DateTime Date,
        decimal Amount,
        PaymentType Type,
        string? Note,
        bool IsRecurring,
        int CategoryId,
        int ChargedAccountId,
        int TargetAccountId,
        bool UpdateRecurringPayment,
        PaymentRecurrence? Recurrence,
        DateTime? EndDate,
        bool IsLastDayOfMonth) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;
        private readonly ISender sender;
        private readonly ISettingsFacade settings;

        public Handler(IAppDbContext appDbContext, ISender sender, ISettingsFacade settings)
        {
            this.appDbContext = appDbContext;
            this.sender = sender;
            this.settings = settings;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var existingPayment = await appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .Include(x => x.Category)
                .Include(x => x.RecurringPayment)
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

            if (command is { IsRecurring: true, UpdateRecurringPayment: true })
            {
                await sender.Send(
                    request: new UpdateRecurringTransaction.Command(
                        recurringTransactionId: existingPayment.RecurringTransactionId!.Value,
                        updatedAmount: new(amount: command.Amount, currencyAlphaIsoCode: settings.DefaultCurrency),
                        updatedCategoryId: command.CategoryId,
                        updatedRecurrence: command.Recurrence!.Value.ToRecurrence(),
                        updatedEndDate: command.EndDate.HasValue ? DateOnly.FromDateTime(command.EndDate.Value) : null,
                        isLastDayOfMonth: command.IsLastDayOfMonth),
                    cancellationToken: cancellationToken);
            }
            else if (!command.IsRecurring && existingPayment.RecurringPayment != null)
            {
                var linkedPayments = appDbContext.Payments.Where(x => x.IsRecurring)
                    .Where(x => x.RecurringPayment!.Id == existingPayment.RecurringPayment!.Id)
                    .ToList();

                appDbContext.RecurringPayments.Remove(existingPayment.RecurringPayment!);
                linkedPayments.ForEach(x => x.RemoveRecurringTransaction());
            }

            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
