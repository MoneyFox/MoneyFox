namespace MoneyFox.Core.Commands.Payments.UpdatePayment;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Domain.Aggregates.AccountAggregate;
using ApplicationCore.Domain.Exceptions;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdatePaymentCommand : IRequest
{
    public UpdatePaymentCommand(
        int id,
        DateTime date,
        decimal amount,
        bool isCleared,
        PaymentType type,
        string note,
        bool isRecurring,
        int categoryId,
        int chargedAccountId,
        int targetAccountId,
        bool updateRecurringPayment,
        PaymentRecurrence? recurrence,
        bool? isEndless,
        DateTime? endDate,
        bool isLastDayOfMonth)
    {
        Id = id;
        Date = date;
        Amount = amount;
        IsCleared = isCleared;
        Type = type;
        Note = note;
        IsRecurring = isRecurring;
        CategoryId = categoryId;
        ChargedAccountId = chargedAccountId;
        TargetAccountId = targetAccountId;
        UpdateRecurringPayment = updateRecurringPayment;
        PaymentRecurrence = recurrence;
        IsEndless = isEndless;
        EndDate = endDate;
        IsLastDayOfMonth = isLastDayOfMonth;
    }

    public int Id { get; }

    public DateTime Date { get; }

    public decimal Amount { get; }

    public bool IsCleared { get; }

    public PaymentType Type { get; }

    public string Note { get; }

    public bool IsRecurring { get; }

    public int CategoryId { get; }

    public int ChargedAccountId { get; }

    public int TargetAccountId { get; }

    public PaymentRecurrence? PaymentRecurrence { get; private set; }

    public bool? IsEndless { get; }

    public DateTime? EndDate { get; }

    public bool IsLastDayOfMonth { get; }

    public bool UpdateRecurringPayment { get; }

    public class Handler : IRequestHandler<UpdatePaymentCommand>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var existingPayment = await appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .Include(x => x.Category)
                .Include(x => x.RecurringPayment)
                .FirstAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);

            var chargedAccount = await appDbContext.Accounts.SingleAsync(
                predicate: a => a.Id == request.ChargedAccountId,
                cancellationToken: cancellationToken);

            var targetAccount = await appDbContext.Accounts.SingleAsync(predicate: a => a.Id == request.TargetAccountId, cancellationToken: cancellationToken);
            existingPayment.UpdatePayment(
                date: request.Date,
                amount: request.Amount,
                type: request.Type,
                chargedAccount: chargedAccount,
                targetAccount: targetAccount,
                category: await appDbContext.Categories.FindAsync(request.CategoryId),
                note: request.Note);

            if (request is { IsRecurring: true, UpdateRecurringPayment: true, PaymentRecurrence: { } })
            {
                HandleRecurringPayment(request: request, existingPayment: existingPayment);
            }
            else if (!request.IsRecurring && existingPayment.RecurringPayment != null)
            {
                var linkedPayments = appDbContext.Payments.Where(x => x.IsRecurring)
                    .Where(x => x.RecurringPayment!.Id == existingPayment.RecurringPayment!.Id)
                    .ToList();

                _ = appDbContext.RecurringPayments.Remove(existingPayment.RecurringPayment!);
                linkedPayments.ForEach(x => x.RemoveRecurringPayment());
            }

            _ = await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private static void HandleRecurringPayment(UpdatePaymentCommand request, Payment existingPayment)
        {
            if (existingPayment.IsRecurring)
            {
                existingPayment.RecurringPayment!.UpdateRecurringPayment(
                    amount: request.Amount,
                    recurrence: request.PaymentRecurrence ?? existingPayment.RecurringPayment.Recurrence,
                    chargedAccount: existingPayment.ChargedAccount,
                    isLastDayOfMonth: request.IsLastDayOfMonth,
                    note: request.Note,
                    endDate: request.IsEndless.HasValue && request.IsEndless.Value ? null : request.EndDate,
                    targetAccount: existingPayment.TargetAccount,
                    category: existingPayment.Category);
            }
            else
            {
                if (!request.PaymentRecurrence.HasValue)
                {
                    throw new RecurrenceNullException(nameof(request.PaymentRecurrence));
                }

                existingPayment.AddRecurringPayment(
                    recurrence: request.PaymentRecurrence.Value,
                    isLastDayOfMonth: request.IsLastDayOfMonth,
                    endDate: request.IsEndless.HasValue && request.IsEndless.Value ? null : request.EndDate);
            }
        }
    }
}
