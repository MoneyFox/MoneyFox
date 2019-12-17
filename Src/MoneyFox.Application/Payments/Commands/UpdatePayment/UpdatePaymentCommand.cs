using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommand : IRequest
    {
        [SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "Intended")]
        public UpdatePaymentCommand(int id,
                                    DateTime date,
                                    decimal amount,
                                    bool isCleared,
                                    PaymentType type,
                                    string note,
                                    bool isRecurring,
                                    int categoryId,
                                    int chargedAccountId,
                                    int targetAccountId,
                                    bool updateRecurringPayment)
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
        }

        public int Id { get; private set; }

        public DateTime Date { get; private set; }

        public decimal Amount { get; private set; }

        public bool IsCleared { get; private set; }

        public PaymentType Type { get; private set; }

        public string Note { get; private set; }

        public bool IsRecurring { get; private set; }

        public int CategoryId { get; private set; }

        public int ChargedAccountId { get; private set; }

        public int TargetAccountId { get; private set; }

        public bool UpdateRecurringPayment { get; private set; }

        public class Handler : IRequestHandler<UpdatePaymentCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
            {
                Payment existingPayment = await contextAdapter.Context.Payments.FindAsync(request.Id);

                if(existingPayment == null) return Unit.Value;

                existingPayment.UpdatePayment(request.Date,
                                              request.Amount,
                                              request.Type,
                                              await contextAdapter.Context.Accounts.FindAsync(request.ChargedAccountId),
                                              await contextAdapter.Context.Accounts.FindAsync(request.TargetAccountId),
                                              await contextAdapter.Context.Categories.FindAsync(request.CategoryId),
                                              request.Note);

                if(request.UpdateRecurringPayment)
                {
                    existingPayment.RecurringPayment
                                   .UpdateRecurringPayment(existingPayment.Amount,
                                                           existingPayment.RecurringPayment.Recurrence,
                                                           existingPayment.ChargedAccount,
                                                           existingPayment.Note,
                                                           existingPayment.RecurringPayment.IsEndless
                                                           ? null
                                                           : existingPayment.RecurringPayment.EndDate,
                                                           existingPayment.TargetAccount);
                }

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
