using System;
using System.Collections.Generic;

namespace MoneyFox.Core.Queries.Statistics.Queries.GetCategorySummary
{
    public static class StatisticUtilities
    {
        private const int POSITIONS_TO_ROUND = 2;

        /// <summary>
        ///     Will round all values of the passed statistic item list
        /// </summary>
        /// <param name="items">List of statistic items.</param>
        public static void RoundStatisticItems(List<CategoryOverviewItem> items) =>
            items.ForEach(
                x =>
                {
                    x.Value = Math.Round(x.Value, POSITIONS_TO_ROUND, MidpointRounding.AwayFromZero);
                    x.Percentage = Math.Round(x.Percentage, POSITIONS_TO_ROUND, MidpointRounding.AwayFromZero);
                });
    }
}