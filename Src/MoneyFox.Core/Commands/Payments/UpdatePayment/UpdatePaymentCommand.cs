namespace MoneyFox.Core.Commands.Payments.UpdatePayment
{

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using _Pending_.Exceptions;
    using Aggregates.Payments;
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
            DateTime? endDate)
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

        public bool UpdateRecurringPayment { get; }

        public class Handler : IRequestHandler<UpdatePaymentCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
            {
                var existingPayment = await contextAdapter.Context.Payments.Include(x => x.ChargedAccount)
                    .Include(x => x.TargetAccount)
                    .Include(x => x.Category)
                    .Include(x => x.RecurringPayment)
                    .FirstAsync(x => x.Id == request.Id);

                if (existingPayment == null)
                {
                    return Unit.Value;
                }

                var chargedAccount = await contextAdapter.Context.Accounts.FindAsync(request.ChargedAccountId);
                var targetAccount = await contextAdapter.Context.Accounts.FindAsync(request.TargetAccountId);
                existingPayment.UpdatePayment(
                    date: request.Date,
                    amount: request.Amount,
                    type: request.Type,
                    chargedAccount: chargedAccount,
                    targetAccount: targetAccount,
                    category: await contextAdapter.Context.Categories.FindAsync(request.CategoryId),
                    note: request.Note);

                if (request.IsRecurring && request.UpdateRecurringPayment && request.PaymentRecurrence.HasValue)
                {
                    HandleRecurringPayment(request: request, existingPayment: existingPayment);
                }
                else if (!request.IsRecurring && existingPayment.RecurringPayment != null)
                {
                    contextAdapter.Context.RecurringPayments.Remove(existingPayment.RecurringPayment!);
                    var linkedPayments = contextAdapter.Context.Payments.Where(x => x.IsRecurring)
                        .Where(x => x.RecurringPayment!.Id == existingPayment.RecurringPayment!.Id)
                        .ToList();

                    linkedPayments.ForEach(x => x.RemoveRecurringPayment());
                }

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

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
                        endDate: request.IsEndless.HasValue && request.IsEndless.Value ? null : request.EndDate);
                }
            }
        }
    }

}
