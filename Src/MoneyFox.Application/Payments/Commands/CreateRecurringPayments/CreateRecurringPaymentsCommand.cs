using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Helpers;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Commands.CreateRecurringPayments
{
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
                                                                               .ToListAsync();

                List<Payment> recPaymentsToCreate = recurringPayments
                                                   .Where(x => x.RelatedPayments.Any())
                                                   .Where(x => RecurringPaymentHelper.CheckIfRepeatable(x.RelatedPayments
                                                                                                         .OrderByDescending(d => d.Date)
                                                                                                         .First()))
                                                   .Select(x => new Payment(RecurringPaymentHelper.GetPaymentDateFromRecurring(x),
                                                                            x.Amount,
                                                                            x.Type,
                                                                            x.ChargedAccount,
                                                                            x.TargetAccount,
                                                                            x.Category,
                                                                            x.Note ?? "",
                                                                            x))
                                                   .ToList();

                recPaymentsToCreate.ForEach(x => x.RecurringPayment?.SetLastRecurrenceCreatedDate());

                contextAdapter.Context.Payments.AddRange(recPaymentsToCreate);
                await contextAdapter.Context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
