using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Queries.GetPaymentsForAccountId
{
    public class GetPaymentsForAccountIdQuery : IRequest<List<Payment>>
    {
        public GetPaymentsForAccountIdQuery(int accountId,
                                            DateTime timeRangeStart,
                                            DateTime timeRangeEnd)
        {
            AccountId = accountId;
            TimeRangeStart = timeRangeStart;
            TimeRangeEnd = timeRangeEnd;
        }

        public int AccountId { get; }

        public DateTime TimeRangeStart { get; }

        public DateTime TimeRangeEnd { get; }

        public bool IsClearedFilterActive { get; set; }

        public bool IsRecurringFilterActive { get; set; }

        public class Handler : IRequestHandler<GetPaymentsForAccountIdQuery, List<Payment>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Payment>> Handle(GetPaymentsForAccountIdQuery request, CancellationToken cancellationToken)
            {
                IQueryable<Payment> paymentQuery = contextAdapter.Context
                                                                 .Payments
                                                                 .Include(x => x.Category)
                                                                 .Include(x => x.RecurringPayment)
                                                                 .HasAccountId(request.AccountId);

                if(request.IsClearedFilterActive)
                    paymentQuery = paymentQuery.AreCleared();
                if(request.IsRecurringFilterActive)
                    paymentQuery = paymentQuery.AreRecurring();

                paymentQuery = paymentQuery.Where(x => x.Date >= request.TimeRangeStart);
                paymentQuery = paymentQuery.Where(x => x.Date <= request.TimeRangeEnd);

                return await paymentQuery.OrderDescendingByDate().ToListAsync(cancellationToken);
            }
        }
    }
}
