﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Statistics.Queries
{
    public class GetCategorySpreadingQuery : IRequest<IEnumerable<StatisticEntry>>
    {
        private const int NUMBER_OF_STATISTIC_ITEMS = 6;

        public GetCategorySpreadingQuery(DateTime startDate,
            DateTime endDate,
            PaymentType paymentType = PaymentType.Expense,
            int numberOfCategoriesToShow = NUMBER_OF_STATISTIC_ITEMS)
        {
            StartDate = startDate;
            EndDate = endDate;
            PaymentType = paymentType;
            NumberOfCategoriesToShow = numberOfCategoriesToShow;
        }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public PaymentType PaymentType { get; }

        public int NumberOfCategoriesToShow { get; }
    }

    public class
        GetCategorySpreadingQueryHandler : IRequestHandler<GetCategorySpreadingQuery, IEnumerable<StatisticEntry>>
    {
        public static readonly string[] Colors =
        {
            "#266489",
            "#68B9C0",
            "#90D585",
            "#F3C151",
            "#F37F64",
            "#424856",
            "#8F97A4",
            "#7EAFC4",
            "#69E1BD",
            "#A6F297",
            "#F9F871",
            "#0087A3",
            "#00AAA9",
            "#3DCA9A",
            "#9BE582"
        };

        private GetCategorySpreadingQuery currentRequest = null!;
        private readonly IContextAdapter contextAdapter;

        public GetCategorySpreadingQueryHandler(IContextAdapter contextAdapter)
        {
            this.contextAdapter = contextAdapter;
        }

        public async Task<IEnumerable<StatisticEntry>> Handle(GetCategorySpreadingQuery request,
            CancellationToken cancellationToken)
        {
            currentRequest = request;
            return AggregateData(
                SelectRelevantDataFromList(await GetPaymentsWithoutTransferAsync(cancellationToken)),
                request.NumberOfCategoriesToShow);
        }

        private async Task<IEnumerable<Payment>> GetPaymentsWithoutTransferAsync(CancellationToken cancellationToken) =>
            await contextAdapter.Context
                                .Payments
                                .Include(x => x.Category)
                                .WithoutTransfers()
                                .HasDateLargerEqualsThan(currentRequest.StartDate.Date)
                                .HasDateSmallerEqualsThan(currentRequest.EndDate.Date)
                                .ToListAsync(cancellationToken);

        private List<(decimal Value, string Label)> SelectRelevantDataFromList(IEnumerable<Payment> payments)
        {
            IEnumerable<(decimal, string category)>? query = from payment in payments
                                                             group payment by new
                                                             {
                                                                 category = payment.Category != null
                                                                     ? payment.Category.Name
                                                                     : string.Empty
                                                             }
                                                             into temp
                                                             select (temp.Sum(
                                                                         x => x.Type == PaymentType.Income
                                                                             ? -x.Amount
                                                                             : x.Amount),
                                                                     temp.Key.category);

            query = currentRequest.PaymentType == PaymentType.Expense
                ? query.Where(x => x.Item1 > 0)
                : query.Where(x => x.Item1 < 0);

            return query.Select(x => (Math.Abs(x.Item1), x.category))
                        .OrderByDescending(x => x.Item1)
                        .ToList();
        }

        private IEnumerable<StatisticEntry> AggregateData(List<(decimal Value, string Label)> statisticData,
            int amountOfCategoriesToShow)
        {
            List<StatisticEntry> statisticList = statisticData
                                                 .Take(amountOfCategoriesToShow)
                                                 .Select(
                                                     x => new StatisticEntry(x.Value)
                                                     {
                                                         ValueLabel = x.Value.ToString(
                                                             "C",
                                                             CultureHelper.CurrentCulture),
                                                         Label = x.Label
                                                     })
                                                 .ToList();

            AddOtherItem(statisticData, statisticList, amountOfCategoriesToShow);
            SetColors(statisticList);

            return statisticList;
        }

        private static void AddOtherItem(IEnumerable<(decimal Value, string Label)> statisticData,
            ICollection<StatisticEntry> statisticList,
            int amountOfCategoriesToShow)
        {
            if(statisticList.Count < amountOfCategoriesToShow)
            {
                return;
            }

            decimal otherValue = statisticData
                                 .Where(x => statisticList.All(y => x.Label != y.Label))
                                 .Sum(x => x.Value);

            var othersItem = new StatisticEntry(otherValue)
            {
                Label = Strings.OthersLabel, ValueLabel = otherValue.ToString("C", CultureHelper.CurrentCulture)
            };

            if(othersItem.Value > 0)
            {
                statisticList.Add(othersItem);
            }
        }

        private static void SetColors(List<StatisticEntry> statisticItems)
        {
            int counter = statisticItems.Count >= Colors.Length
                ? Colors.Length
                : statisticItems.Count;

            for(int i = 0; i < counter; i++)
            {
                statisticItems[i].Color = Colors[i];
            }
        }
    }
}