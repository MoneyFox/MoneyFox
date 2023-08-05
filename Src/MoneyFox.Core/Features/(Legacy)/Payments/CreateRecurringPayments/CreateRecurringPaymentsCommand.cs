namespace MoneyFox.Core.Features._Legacy_.Payments.CreateRecurringPayments;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

public class CreateRecurringPaymentsCommand : IRequest
{
    public class Handler : IRequestHandler<CreateRecurringPaymentsCommand>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(CreateRecurringPaymentsCommand request, CancellationToken cancellationToken)
        {
            var recurringPayments = await appDbContext.RecurringPayments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .Include(x => x.Category)
                .Include(x => x.RelatedPayments)
                .AsQueryable()
                .Where(x1 => x1.IsEndless || x1.EndDate >= DateTime.Today)
                .ToListAsync(cancellationToken);

            var recPaymentsToCreate = recurringPayments.Where(x => x.RelatedPayments.Any())
                .Where(x => RecurrenceHelper.CheckIfRepeatable(x.RelatedPayments.OrderByDescending(d => d.Date).First()))
                .Select(
                    x => new Payment(
                        date: RecurrenceHelper.GetPaymentDateFromRecurring(x),
                        amount: x.Amount,
                        type: x.Type,
                        chargedAccount: x.ChargedAccount,
                        targetAccount: x.TargetAccount,
                        category: x.Category,
                        note: x.Note ?? ""))
                .ToList();

            recPaymentsToCreate.ForEach(x => x.RecurringPayment?.SetLastRecurrenceCreatedDate());
            Log.Information(messageTemplate: "Create {Count} recurring payments", propertyValue: recPaymentsToCreate.Count);
            appDbContext.Payments.AddRange(recPaymentsToCreate);
            _ = await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
