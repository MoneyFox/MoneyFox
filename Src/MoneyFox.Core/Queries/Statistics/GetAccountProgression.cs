namespace MoneyFox.Core.Queries.Statistics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Common.Settings;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetAccountProgression
{
    public record Query : IRequest<List<StatisticEntry>>
    {
        public Query(int accountId, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new InvalidDateRangeException();
            }

            AccountId = accountId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int AccountId { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }
    }

    public class Handler : IRequestHandler<Query, List<StatisticEntry>>
    {
        private const string RED_HEX_CODE = "#cd3700";
        private const string BLUE_HEX_CODE = "#87cefa";

        private readonly IAppDbContext appDbContext;
        private readonly ISettingsFacade settingsFacade;

        public Handler(IAppDbContext appDbContext, ISettingsFacade settingsFacade)
        {
            this.appDbContext = appDbContext;
            this.settingsFacade = settingsFacade;
        }

        public async Task<List<StatisticEntry>> Handle(Query request, CancellationToken cancellationToken)
        {
            var baseQuery = appDbContext.Payments
                .Include(x => x.Category)
                .Include(x => x.ChargedAccount)
                .HasDateLargerEqualsThan(request.StartDate.Date)
                .HasDateSmallerEqualsThan(request.EndDate.Date);

            if (request.AccountId > 0)
            {
                baseQuery = baseQuery.HasAccountId(accountId: request.AccountId);
            }

            var payments = await baseQuery.ToListAsync(cancellationToken);

            List<StatisticEntry> returnList = new();
            foreach (var group in payments.GroupBy(x => new { x.Date.Month, x.Date.Year }))
            {
                StatisticEntry statisticEntry = new(
                    value: group.Sum(x => GetPaymentAmountForSum(payment: x, request: request)),
                    label: $"{group.Key.Month:d2} {group.Key.Year:d4}");

                statisticEntry.ValueLabel = statisticEntry.Value.FormatCurrency(settingsFacade.DefaultCurrency);
                statisticEntry.Color = statisticEntry.Value >= 0 ? BLUE_HEX_CODE : RED_HEX_CODE;
                returnList.Add(statisticEntry);
            }

            return returnList;
        }

        private static decimal GetPaymentAmountForSum(Payment payment, Query request)
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
