namespace MoneyFox.Core.Queries.GetPaymentsForAccountIdQuery;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetPaymentsForAccountIdQuery : IRequest<List<Payment>>
{
    public GetPaymentsForAccountIdQuery(
        int accountId,
        DateTime timeRangeStart,
        DateTime timeRangeEnd,
        bool isClearedFilterActive = false,
        bool isRecurringFilterActive = false,
        PaymentTypeFilter filteredPaymentType = PaymentTypeFilter.All)
    {
        AccountId = accountId;
        TimeRangeStart = timeRangeStart;
        TimeRangeEnd = timeRangeEnd;
        IsClearedFilterActive = isClearedFilterActive;
        IsRecurringFilterActive = isRecurringFilterActive;
        FilteredPaymentType = filteredPaymentType;
    }

    public int AccountId { get; }

    public DateTime TimeRangeStart { get; }

    public DateTime TimeRangeEnd { get; }

    public PaymentTypeFilter FilteredPaymentType { get; }

    public bool IsClearedFilterActive { get; }

    public bool IsRecurringFilterActive { get; }

    public class Handler : IRequestHandler<GetPaymentsForAccountIdQuery, List<Payment>>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<Payment>> Handle(GetPaymentsForAccountIdQuery request, CancellationToken cancellationToken)
        {
            var paymentQuery = appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .Include(x => x.Category)
                .Include(x => x.RecurringPayment)
                .HasAccountId(accountId: request.AccountId);

            if (request.IsClearedFilterActive)
            {
                paymentQuery = paymentQuery.AreCleared();
            }

            if (request.IsRecurringFilterActive)
            {
                paymentQuery = paymentQuery.AreRecurring();
            }

            if (request.FilteredPaymentType != PaymentTypeFilter.All)
            {
                paymentQuery = paymentQuery.IsPaymentType(PaymentTypeFilterHelper.PaymentTypeFilterToPaymentType(request.FilteredPaymentType));
            }

            paymentQuery = paymentQuery.Where(x => x.Date >= request.TimeRangeStart);
            paymentQuery = paymentQuery.Where(x => x.Date <= request.TimeRangeEnd);

            return await paymentQuery.OrderDescendingByDate().ToListAsync(cancellationToken);
        }
    }
}
