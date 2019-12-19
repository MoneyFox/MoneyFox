using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Statistics.Queries.GetCategorySpreading
{
    public class GetCategorySpreadingQueryHandler : IRequestHandler<GetCategorySpreadingQuery, IEnumerable<StatisticEntry>>
    {
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

        private async Task<IEnumerable<Payment>> GetPaymentsWithoutTransferAsync(GetCategorySpreadingQuery request, CancellationToken cancellationToken)
        {
            return await contextAdapter.Context
                                       .Payments
                                       .Include(x => x.Category)
                                       .WithoutTransfers()
                                       .HasDateLargerEqualsThan(request.StartDate.Date)
                                       .HasDateSmallerEqualsThan(request.EndDate.Date)
                                       .ToListAsync(cancellationToken);
        }

        private List<(float Value, string Label)> SelectRelevantDataFromList(IEnumerable<Payment> payments)
        {
            return (from payment in payments
                    group payment by new
                    {
                        category = payment.Category != null
                                   ? payment.Category.Name : string.Empty
                    }
                    into temp
                    select (
                        (float) temp.Sum(x => x.Type == PaymentType.Income
                                              ? -x.Amount : x.Amount),
                        temp.Key.category))
                   .Where(x => x.Item1 > 0)
                   .OrderByDescending(x => x.Item1)
                   .ToList();
        }

        private IEnumerable<StatisticEntry> AggregateData(List<(float Value, string Label)> statisticData)
        {
            List<StatisticEntry> statisticList = statisticData
                                                 .Take(6)
                                                 .Select(x => new StatisticEntry(x.Value)
                                                 {
                                                     ValueLabel = x.Value.ToString("C",CultureInfo.CurrentCulture), Label = x.Label
                                                 })
                                                 .ToList();

            AddOtherItem(statisticData, statisticList);
            SetColors(statisticList);

            return statisticList;
        }

        private static void AddOtherItem(IEnumerable<(float Value, string Label)> statisticData, ICollection<StatisticEntry> statisticList)
        {
            if(statisticList.Count < 6)
                return;

            float otherValue = statisticData
                               .Where(x => statisticList.All(y => x.Label != y.Label))
                               .Sum(x => x.Value);

            var othersItem = new StatisticEntry(otherValue)
            {
                Label = Strings.OthersLabel,
                ValueLabel = otherValue.ToString("C", CultureInfo.InvariantCulture)
            };

            if(othersItem.Value > 0)
                statisticList.Add(othersItem);
        }

        private static void SetColors(List<StatisticEntry> statisticItems)
        {
            for(var i = 0; i < statisticItems.Count; i++)
            {
                statisticItems[i].Color = Colors[i];
            }
        }
    }
}
