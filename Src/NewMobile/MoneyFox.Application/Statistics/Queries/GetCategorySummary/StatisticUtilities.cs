using System;
using System.Collections.Generic;

namespace MoneyFox.Application.Statistics.Queries.GetCategorySummary
{
    public static class StatisticUtilities
    {
        /// <summary>
        /// Will round all values of the passed statistic item list
        /// </summary>
        /// <param name="items">List of statistic items.</param>
        public static void RoundStatisticItems(List<CategoryOverviewItem> items)
        {
            items.ForEach(x =>
                          {
                              x.Value = Math.Round(x.Value, 2, MidpointRounding.AwayFromZero);
                              x.Percentage = Math.Round(x.Percentage, 2, MidpointRounding.AwayFromZero);
                          });
        }
    }
}
