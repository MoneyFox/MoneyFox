using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Domain;
using MoneyFox.Application.Common.QueryObjects;

namespace MoneyFox.Application.Statistics.Queries
{
    public class GetCategorySpreadingQuery : IRequest<IEnumerable<StatisticEntry>>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class GetCategorySpreadingQueryHandler : IRequestHandler<GetCategorySpreadingQuery, IEnumerable<StatisticEntry>>
    {
        private const int NUMBER_OF_STATISTIC_ITEMS = 6;

        public static readonly string[] Colors =
        { "#266489", "#68B9C0", "#90D585", "#F3C151", "#F37F64", "#424856", "#8F97A4" };

        private readonly IContextAdapter contextAdapter;

        public GetCategorySpreadingQueryHandler(IContextAdapter contextAdapter)
        {
            this.contextAdapter = contextAdapter;
        }

        public async Task<IEnumerable<StatisticEntry>> Handle(GetCategorySpreadingQuery request, CancellationToken cancellationToken)
        {
            return AggregateData(SelectRelevantDataFromList(await GetPaymentsWithoutTransferAsync(request,
                                                                                                  cancellationToken)));
        }

        private async Task<IEnumerable<Payment>> GetPaymentsWithoutTransferAsync(GetCategorySpreadingQuery request,
                                                                                 CancellationToken cancellationToken)
        {
            return await contextAdapter.Context
                                       .Payments
                                       .Include(x => x.Category)
                                       .WithoutTransfers()
                                       .HasDateLargerEqualsThan(request.StartDate.Date)
                                       .HasDateSmallerEqualsThan(request.EndDate.Date)
                                       .ToListAsync(cancellationToken);
        }

        private List<(decimal Value, string Label)> SelectRelevantDataFromList(IEnumerable<Payment> payments)
        {
            return (from payment in payments
                    group payment by new
                    {
                        category = payment.Category != null
                                                    ? payment.Category.Name
                                                    : string.Empty
                    }
                    into temp
                    select (
                               temp.Sum(x => x.Type == PaymentType.Income
                                                ? -x.Amount
                                                : x.Amount),
                               temp.Key.category))
                  .Where(x => x.Item1 > 0)
                  .OrderByDescending(x => x.Item1)
                  .ToList();
        }

        private IEnumerable<StatisticEntry> AggregateData(List<(decimal Value, string Label)> statisticData)
        {
            var statisticList = statisticData
                                                .Take(6)
                                                .Select(x => new StatisticEntry(x.Value)
                                                {
                                                    ValueLabel = x.Value.ToString("C", CultureHelper.CurrentCulture),
                                                    Label = x.Label
                                                })
                                                .ToList();

            AddOtherItem(statisticData, statisticList);
            SetColors(statisticList);

            return statisticList;
        }

        private static void AddOtherItem(IEnumerable<(decimal Value, string Label)> statisticData, ICollection<StatisticEntry> statisticList)
        {
            if(statisticList.Count < NUMBER_OF_STATISTIC_ITEMS)
            {
                return;
            }

            decimal otherValue = statisticData
                                  .Where(x => statisticList.All(y => x.Label != y.Label))
                                  .Sum(x => x.Value);

            var othersItem = new StatisticEntry(otherValue)
            {
                Label = Strings.OthersLabel,
                ValueLabel = otherValue.ToString("C", CultureHelper.CurrentCulture)
            };

            if(othersItem.Value > 0)
            {
                statisticList.Add(othersItem);
            }
        }

        private static void SetColors(List<StatisticEntry> statisticItems)
        {
            for(int i = 0; i < statisticItems.Count; i++)
            {
                statisticItems[i].Color = Colors[i];
            }
        }
    }
}
