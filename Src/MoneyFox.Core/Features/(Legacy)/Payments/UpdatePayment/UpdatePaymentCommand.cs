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
        bool? IsEndless,
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
                        RecurringTransactionId: existingPayment.RecurringTransactionId!.Value,
                        UpdatedAmount: new(amount: command.Amount, currencyAlphaIsoCode: settings.DefaultCurrency),
                        UpdatedCategoryId: command.CategoryId,
                        UpdatedRecurrence: command.Recurrence!.Value.ToRecurrence(),
                        UpdatedEndDate: command.EndDate.HasValue ? DateOnly.FromDateTime(command.EndDate.Value) : null,
                        IsLastDayOfMonth: command.IsLastDayOfMonth),
                    cancellationToken: cancellationToken);

                UpdateRecurringPayment(request: command, existingPayment: existingPayment);
            }
            else if (!command.IsRecurring && existingPayment.RecurringPayment != null)
            {
                var linkedPayments = appDbContext.Payments.Where(x => x.IsRecurring)
                    .Where(x => x.RecurringPayment!.Id == existingPayment.RecurringPayment!.Id)
                    .ToList();

                _ = appDbContext.RecurringPayments.Remove(existingPayment.RecurringPayment!);
                linkedPayments.ForEach(x => x.RemoveRecurringPayment());
            }

            _ = await appDbContext.SaveChangesAsync(cancellationToken);
        }

        private static void UpdateRecurringPayment(Command request, Payment existingPayment)
        {
            if (existingPayment.IsRecurring)
            {
                existingPayment.RecurringPayment!.UpdateRecurringPayment(
                    amount: request.Amount,
                    recurrence: request.Recurrence ?? existingPayment.RecurringPayment.Recurrence,
                    chargedAccount: existingPayment.ChargedAccount,
                    isLastDayOfMonth: request.IsLastDayOfMonth,
                    note: request.Note,
                    endDate: request.IsEndless.HasValue && request.IsEndless.Value ? null : request.EndDate,
                    targetAccount: existingPayment.TargetAccount,
                    category: existingPayment.Category);
            }
            else
            {
                if (!request.Recurrence.HasValue)
                {
                    throw new RecurrenceNullException(nameof(request.Recurrence));
                }

                existingPayment.AddRecurringPayment(
                    recurrence: request.Recurrence.Value,
                    isLastDayOfMonth: request.IsLastDayOfMonth,
                    endDate: request.IsEndless.HasValue && request.IsEndless.Value ? null : request.EndDate);
            }
        }
    }
}
