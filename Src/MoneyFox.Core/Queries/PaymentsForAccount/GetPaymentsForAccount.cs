namespace MoneyFox.Core.Queries.PaymentsForAccount;

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

public static class GetPaymentsForAccount
{
    public record Query(
        int AccountId,
        DateTime TimeRangeStart,
        DateTime TimeRangeEnd,
        PaymentTypeFilter FilteredPaymentType,
        bool IsClearedFilterActive,
        bool IsRecurringFilterActive) : IRequest<IReadOnlyList<PaymentData>>;

    public class Handler : IRequestHandler<Query, IReadOnlyList<PaymentData>>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IReadOnlyList<PaymentData>> Handle(Query request, CancellationToken cancellationToken)
        {
            var paymentQuery = appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .Include(x => x.Category)
                .HasAccountId(accountId: request.AccountId);

            if (request.IsClearedFilterActive)
            {
                paymentQuery = paymentQuery.AreCleared();
            }

            if (request.IsRecurringFilterActive)
            {
                paymentQuery = paymentQuery.Where(payment => payment.IsRecurring);
            }

            if (request.FilteredPaymentType != PaymentTypeFilter.All)
            {
                paymentQuery = paymentQuery.IsPaymentType(PaymentTypeFilterToPaymentType(request.FilteredPaymentType));
            }

            paymentQuery = paymentQuery.Where(x => x.Date >= request.TimeRangeStart);
            paymentQuery = paymentQuery.Where(x => x.Date <= request.TimeRangeEnd);

            return await paymentQuery.Select(
                    p => new PaymentData(
                        p.Id,
                        p.ChargedAccount.Id,
                        p.Amount,
                        p.Category == null ? null : p.Category.Name,
                        DateOnly.FromDateTime(p.Date),
                        p.Note ?? string.Empty,
                        p.IsCleared,
                        p.IsRecurring,
                        p.Type))
                .OrderByDescending(p => p.Date)
                .ToListAsync(cancellationToken);
        }

        private static PaymentType PaymentTypeFilterToPaymentType(PaymentTypeFilter paymentTypeFilter)
        {
            return paymentTypeFilter switch
            {
                PaymentTypeFilter.Expense => PaymentType.Expense,
                PaymentTypeFilter.Income => PaymentType.Income,
                PaymentTypeFilter.Transfer => PaymentType.Transfer,
                _ => throw new InvalidCastException()
            };
        }
    }

    public record PaymentData(
        int Id,
        int ChargedAccountId,
        decimal Amount,
        string? CategoryName,
        DateOnly Date,
        string Note,
        bool IsCleared,
        bool IsRecurring,
        PaymentType Type);
}
