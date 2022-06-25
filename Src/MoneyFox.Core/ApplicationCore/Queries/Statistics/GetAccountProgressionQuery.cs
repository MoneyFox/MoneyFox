namespace MoneyFox.Core.ApplicationCore.Queries.Statistics
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Helpers;
    using Domain.Aggregates.AccountAggregate;
    using Domain.Exceptions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using MoneyFox.Core._Pending_.Common.QueryObjects;
    using MoneyFox.Core.Common;
    using MoneyFox.Core.Common.Interfaces;

    public class GetAccountProgressionQuery : IRequest<List<StatisticEntry>>
    {
        public GetAccountProgressionQuery(int accountId, DateTime startDate, DateTime endDate)
        {
            AccountId = accountId;
            StartDate = startDate;
            EndDate = endDate;
            if (startDate > EndDate)
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

        private readonly IAppDbContext appDbContext;

        public GetAccountProgressionHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<StatisticEntry>> Handle(GetAccountProgressionQuery request, CancellationToken cancellationToken)
        {
            var payments = await appDbContext.Payments.Include(x => x.Category)
                .Include(x => x.ChargedAccount)
                .HasAccountId(request.AccountId)
                .HasDateLargerEqualsThan(request.StartDate.Date)
                .HasDateSmallerEqualsThan(request.EndDate.Date)
                .ToListAsync(cancellationToken);

            var returnList = new List<StatisticEntry>();
            foreach (var group in payments.GroupBy(x => new { x.Date.Month, x.Date.Year }))
            {
                var statisticEntry = new StatisticEntry(
                    value: group.Sum(x => GetPaymentAmountForSum(payment: x, request: request)),
                    label: $"{group.Key.Month:d2} {group.Key.Year:d4}");

                statisticEntry.ValueLabel = statisticEntry.Value.ToString(format: "c", provider: CultureHelper.CurrentCulture);
                statisticEntry.Color = statisticEntry.Value >= 0 ? BLUE_HEX_CODE : RED_HEX_CODE;
                returnList.Add(statisticEntry);
            }

            return returnList;
        }

        private static decimal GetPaymentAmountForSum(Payment payment, GetAccountProgressionQuery request)
        {
            return payment.Type switch
            {
                PaymentType.Expense => -payment.Amount,
                PaymentType.Income => payment.Amount,
                PaymentType.Transfer => payment.ChargedAccount.Id == request.AccountId ? -payment.Amount : payment.Amount,
                _ => 0
            };
        }
    }

}
