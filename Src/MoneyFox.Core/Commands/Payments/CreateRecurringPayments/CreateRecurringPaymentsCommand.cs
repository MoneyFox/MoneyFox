namespace MoneyFox.Core.Commands.Payments.CreateRecurringPayments
{

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using _Pending_.Common.Helpers;
    using _Pending_.Common.QueryObjects;
    using Aggregates.AccountAggregate;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Serilog;

    public class CreateRecurringPaymentsCommand : IRequest
    {
        public class Handler : IRequestHandler<CreateRecurringPaymentsCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(CreateRecurringPaymentsCommand request, CancellationToken cancellationToken)
            {
                var recurringPayments = await contextAdapter.Context.RecurringPayments.Include(x => x.ChargedAccount)
                    .Include(x => x.TargetAccount)
                    .Include(x => x.Category)
                    .Include(x => x.RelatedPayments)
                    .AsQueryable()
                    .IsNotExpired()
                    .ToListAsync(cancellationToken);

                var recPaymentsToCreate = recurringPayments.Where(x => x.RelatedPayments.Any())
                    .Where(x => RecurringPaymentHelper.CheckIfRepeatable(x.RelatedPayments.OrderByDescending(d => d.Date).First()))
                    .Select(
                        x => new Payment(
                            date: RecurringPaymentHelper.GetPaymentDateFromRecurring(x),
                            amount: x.Amount,
                            type: x.Type,
                            chargedAccount: x.ChargedAccount,
                            targetAccount: x.TargetAccount,
                            category: x.Category,
                            note: x.Note ?? "",
                            recurringPayment: x))
                    .ToList();

                recPaymentsToCreate.ForEach(x => x.RecurringPayment?.SetLastRecurrenceCreatedDate());
                Log.Information(messageTemplate: "Create {Count} recurring payments", propertyValue: recPaymentsToCreate.Count);
                contextAdapter.Context.Payments.AddRange(recPaymentsToCreate);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }

}
