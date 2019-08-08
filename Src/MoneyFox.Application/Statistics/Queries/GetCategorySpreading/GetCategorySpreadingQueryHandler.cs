using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Application.Statistics.Models;
using MoneyFox.BusinessDbAccess.QueryObjects;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Statistics.Queries.GetCategorySpreading
{
    public class GetCategorySpreadingQueryHandler : IRequestHandler<GetCategorySpreadingQuery, IEnumerable<StatisticEntry>>
    {
        public static readonly string[] Colors =
        {
            "#266489",
            "#68B9C0",
            "#90D585",
            "#F3C151",
            "#F37F64",
            "#424856",
            "#8F97A4"
        };

        private readonly IEfCoreContext context;

        public GetCategorySpreadingQueryHandler(IEfCoreContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<StatisticEntry>> Handle(GetCategorySpreadingQuery request, CancellationToken cancellationToken)
        {
            return AggregateData(SelectRelevantDataFromList(await GetPaymentsWithoutTransfer(request, cancellationToken)));
        }

        private async Task<IEnumerable<Payment>> GetPaymentsWithoutTransfer(GetCategorySpreadingQuery request, CancellationToken cancellationToken) => await context.Payments
                                                                                                                                                                    .Include(x => x.Category)
                                                                                                                                                                    .WithoutTransfers()
                                                                                                                                                                    .HasDateLargerEqualsThan(request.StartDate.Date)
                                                                                                                                                                    .HasDateSmallerEqualsThan(request.EndDate.Date)
                                                                                                                                                                    .ToListAsync(cancellationToken);

        private List<(float Value, string Label)> SelectRelevantDataFromList(IEnumerable<Payment> payments)
        {
            return (from payment in payments
                    group payment by new
                    {
                        category = payment.Category != null ? payment.Category.Name : string.Empty
                    }
                    into temp
                    select (
                        (float)temp.Sum(x => x.Type == PaymentType.Income ? -x.Amount : x.Amount),
                        temp.Key.category))
                   .Where(x => x.Item1 > 0)
                   .OrderByDescending(x => x.Item1)
                   .ToList();
        }

        private IEnumerable<StatisticEntry> AggregateData(List<(float Value, string Label)> statisticData)
        {
            var statisticList = statisticData
                                .Take(6)
                                .Select(x => new StatisticEntry(x.Value) { ValueLabel = x.Value.ToString("C", CultureInfo.CurrentCulture), Label = x.Label })
                                .ToList();

            AddOtherItem(statisticData, statisticList);
            SetColors(statisticList);

            return statisticList;
        }

        private static void AddOtherItem(List<(float Value, string Label)> statisticData, ICollection<StatisticEntry> statisticList)
        {
            if (statisticList.Count < 6)
            {
                return;
            }

            var otherValue = statisticData
                             .Where(x => statisticList.All(y => x.Label != y.Label))
                             .Sum(x => x.Value);

            var othersItem = new StatisticEntry(otherValue)
            {
                Label = Strings.OthersLabel,
                ValueLabel = otherValue.ToString("C", CultureInfo.InvariantCulture)
            };

            if (othersItem.Value > 0)
            {
                statisticList.Add(othersItem);
            }
        }

        private void SetColors(List<StatisticEntry> statisticItems)
        {

            for (int i = 0; i < statisticItems.Count; i++)
            {
                statisticItems[i].Color = Colors[i];
            }
        }
    }
}
