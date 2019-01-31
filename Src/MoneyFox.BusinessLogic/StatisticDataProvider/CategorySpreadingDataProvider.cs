using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.StatisticDataProvider;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.BusinessLogic.StatisticDataProvider
{
    public interface ICategorySpreadingDataProvider
    {
        Task<IEnumerable<StatisticEntry>> GetValues(DateTime startDate, DateTime endDate);
    }

    public class CategorySpreadingDataProvider : ICategorySpreadingDataProvider
    {
        private readonly IStatisticDbAccess statisticDbAccess;

        public CategorySpreadingDataProvider(IStatisticDbAccess statisticDbAccess)
        {
            this.statisticDbAccess = statisticDbAccess;
        }

        /// <summary>
        ///     Selects payments from the given timeframe and calculates the spreading for the six categories
        ///     with the highest spendings. All others are summarized in a "other" item.
        /// </summary>
        /// <param name="startDate">Startpoint form which to select data.</param>
        /// <param name="endDate">Endpoint form which to select data.</param>
        /// <returns>Statistic value for the given time. </returns>
        public async Task<IEnumerable<StatisticEntry>> GetValues(DateTime startDate, DateTime endDate)
        {
            return AggregateData(SelectRelevantDataFromList(await statisticDbAccess.GetPaymentsWithoutTransfer(startDate, endDate)));
        }

        private List<(float Value, string Label)> SelectRelevantDataFromList(IEnumerable<Payment> payments)
        {
            return (from payment in payments
                    group payment by new
                    {
                        category = payment.Category != null ? payment.Category.Name : string.Empty
                    }
                    into temp
                    select (
                        (float) temp.Sum(x => x.Type == PaymentType.Income ? -x.Amount : x.Amount),
                        temp.Key.category))
                   .Where(x => x.Item1 > 0)
                   .OrderByDescending(x => x.Item1)
                   .ToList();
        }

        private IEnumerable<StatisticEntry> AggregateData(List<(float Value, string Label)> statisticData)
        {
            var statisticList = statisticData
                                .Take(6)
                                .Select(x => new StatisticEntry( x.Value) {ValueLabel = x.Value.ToString("C"), Label = x.Label})
                                .ToList();

            AddOtherItem(statisticData, statisticList);
            SetLabel(statisticList);

            return statisticList;
        }

        private static void SetLabel(List<StatisticEntry> statisticList)
        {
            foreach (var statisticItem in statisticList)
            {
                statisticItem.Label = statisticItem.Label;
            }
        }

        private void AddOtherItem(List<(float Value, string Label)> statisticData, ICollection<StatisticEntry> statisticList)
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
                ValueLabel = otherValue.ToString("C")
            };

            if (othersItem.Value > 0)
            {
                statisticList.Add(othersItem);
            }
        }
    }
}