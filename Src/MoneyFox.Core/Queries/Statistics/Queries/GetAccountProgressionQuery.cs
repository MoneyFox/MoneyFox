using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.QueryObjects;
using MoneyFox.Core._Pending_.Exceptions;
using MoneyFox.Core.Aggregates.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Queries.Statistics.Queries
{
    public class GetAccountProgressionQuery : IRequest<List<StatisticEntry>>
    {
        public GetAccountProgressionQuery(int accountId, DateTime startDate, DateTime endDate)
        {
            AccountId = accountId;
            StartDate = startDate;
            EndDate = endDate;

            if(startDate > EndDate)
            {
                throw new StartAfterEnddateException();
            }
        }

        public int AccountId { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }
    }

    public class GetAccountProgressionHandler : IRequestHandler<GetAccountProgressionQuery, List<StatisticEntry>>
    {
        private const string RED_HEX_CODE = "#cd3700";
        private const string BLUE_HEX_CODE = "#87cefa";

        private readonly IContextAdapter contextAdapter;

        public GetAccountProgressionHandler(IContextAdapter contextAdapter)
        {
            this.contextAdapter = contextAdapter;
        }

        public async Task<List<StatisticEntry>> Handle(GetAccountProgressionQuery request,
            CancellationToken cancellationToken)
        {
            List<Payment>? payments = await contextAdapter.Context
                .Payments
                .Include(x => x.Category)
                .Include(x => x.ChargedAccount)
                .HasAccountId(request.AccountId)
                .HasDateLargerEqualsThan(request.StartDate.Date)
                .HasDateSmallerEqualsThan(request.EndDate.Date)
                .ToListAsync(cancellationToken);

            var returnList = new List<StatisticEntry>();
            foreach(var group in payments.GroupBy(x => new { x.Date.Month, x.Date.Year }))
            {
                var statisticEntry = new StatisticEntry(
                    group.Sum(x => GetPaymentAmountForSum(x, request)),
                    $"{group.Key.Month:d2} {group.Key.Year:d4}");
                statisticEntry.ValueLabel = statisticEntry.Value.ToString("c", CultureHelper.CurrentCulture);
                statisticEntry.Color = statisticEntry.Value >= 0 ? BLUE_HEX_CODE : RED_HEX_CODE;
                returnList.Add(statisticEntry);
            }

            return returnList;
        }

        private static decimal GetPaymentAmountForSum(Payment payment, GetAccountProgressionQuery request) =>
            payment.Type switch
            {
                PaymentType.Expense => -payment.Amount,
                PaymentType.Income => payment.Amount,
                PaymentType.Transfer => payment.ChargedAccount.Id == request.AccountId
                    ? -payment.Amount
                    : payment.Amount,
                _ => 0
            };
    }
}