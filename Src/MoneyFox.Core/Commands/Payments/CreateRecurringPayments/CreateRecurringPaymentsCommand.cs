namespace MoneyFox.Core.Commands.Payments.CreateRecurringPayments
{
    using _Pending_.Common.Helpers;
    using _Pending_.Common.QueryObjects;
    using Aggregates.Payments;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using NLog;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
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
                List<RecurringPayment> recurringPayments = await contextAdapter.Context
                    .RecurringPayments
                    .Include(x => x.ChargedAccount)
                    .Include(x => x.TargetAccount)
                    .Include(x => x.Category)
                    .Include(x => x.RelatedPayments)
                    .AsQueryable()
                    .IsNotExpired()
                    .ToListAsync(cancellationToken);

                List<Payment> recPaymentsToCreate = recurringPayments
                    .Where(x => x.RelatedPayments.Any())
                    .Where(
                        x => RecurringPaymentHelper.CheckIfRepeatable(
                            x.RelatedPayments
                                .OrderByDescending(d => d.Date)
                                .First()))
                    .Select(
                        x => new Payment(
                            RecurringPaymentHelper.GetPaymentDateFromRecurring(x),
                            x.Amount,
                            x.Type,
                            x.ChargedAccount,
                            x.TargetAccount,
                            x.Category,
                            x.Note ?? "",
                            x))
                    .ToList();

                recPaymentsToCreate.ForEach(x => x.RecurringPayment?.SetLastRecurrenceCreatedDate());

                Log.Information("Create {Count} recurring payments", recPaymentsToCreate.Count);

                contextAdapter.Context.Payments.AddRange(recPaymentsToCreate);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
