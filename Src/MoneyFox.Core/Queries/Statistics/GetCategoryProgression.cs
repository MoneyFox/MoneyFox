namespace MoneyFox.Core.Queries.Statistics;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

public static class GetCategoryProgression
{
    public class Query : IRequest<IImmutableList<StatisticEntry>>
    {
        public Query(int categoryId, DateOnly startDate, DateOnly endDate)
        {
            if (startDate > endDate)
            {
                throw new InvalidDateRangeException();
            }

            CategoryId = categoryId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int CategoryId { get; }

        public DateOnly StartDate { get; }

        public DateOnly EndDate { get; }
    }

    public class Handler : IRequestHandler<Query, IImmutableList<StatisticEntry>>
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

        public async Task<IImmutableList<StatisticEntry>> Handle(Query request, CancellationToken cancellationToken)
        {
            var payments = await appDbContext.Payments.Include(x => x.Category)
                .Include(x => x.ChargedAccount)
                .HasCategoryId(categoryId: request.CategoryId)
                .Where(payment => payment.Date.Date >= request.StartDate.ToDateTime(TimeOnly.MinValue))
                .Where(payment => payment.Date.Date <= request.EndDate.ToDateTime(TimeOnly.MinValue))
                .ToListAsync(cancellationToken);

            List<StatisticEntry> statisticList = new();
            foreach (var group in payments.GroupBy(x => new { x.Date.Month, x.Date.Year }))
            {
                StatisticEntry statisticEntry = new(
                    value: group.Sum(x => GetPaymentAmountForSum(payment: x, request: request)),
                    label: $"{group.Key.Month:d2} {group.Key.Year:d4}");

                statisticEntry.ValueLabel = statisticEntry.Value.FormatCurrency(settingsFacade.DefaultCurrency);
                statisticEntry.Color = statisticEntry.Value >= 0 ? BLUE_HEX_CODE : RED_HEX_CODE;
                statisticList.Add(statisticEntry);
            }

            return FillReturnList(request: request, statisticEntries: statisticList);
        }

        private IImmutableList<StatisticEntry> FillReturnList(Query request, ICollection<StatisticEntry> statisticEntries)
        {
            List<StatisticEntry> returnList = new();
            var startDate = request.StartDate;
            var endDate = request.EndDate.AddMonths(1);
            do
            {
                var items = statisticEntries.Where(x => x.Label == $"{startDate.Month:d2} {startDate.Year:d4}").ToList();
                returnList.AddRange(items);
                if (!items.Any())
                {
                    StatisticEntry placeholderItem = new(value: 0, label: $"{startDate.Month:d2} {startDate.Year:d4}");
                    placeholderItem.ValueLabel = placeholderItem.Value.FormatCurrency(settingsFacade.DefaultCurrency);
                    placeholderItem.Color = placeholderItem.Value >= 0 ? BLUE_HEX_CODE : RED_HEX_CODE;
                    returnList.Add(placeholderItem);
                }

                startDate = startDate.AddMonths(1);
            }
            while (startDate.Month != endDate.Month || startDate.Year != endDate.Year);

            return returnList.ToImmutableList();
        }

        private static decimal GetPaymentAmountForSum(Payment payment, Query request)
        {
            return payment.Type switch
            {
                PaymentType.Expense => -payment.Amount,
                PaymentType.Income => payment.Amount,
                PaymentType.Transfer => payment.ChargedAccount.Id == request.CategoryId ? -payment.Amount : payment.Amount,
                _ => 0
            };
        }
    }
}

