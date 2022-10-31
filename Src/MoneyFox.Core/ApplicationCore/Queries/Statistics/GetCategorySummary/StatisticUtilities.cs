namespace MoneyFox.Core.ApplicationCore.Queries.Statistics.GetCategorySummary;

using System;
using System.Collections.Generic;

public static class StatisticUtilities
{
    private const int POSITIONS_TO_ROUND = 2;

    /// <summary>
    ///     Will round all values of the passed statistic item list
    /// </summary>
    /// <param name="items">List of statistic items.</param>
    public static void RoundStatisticItems(List<CategoryOverviewItem> items)
    {
        items.ForEach(
            x =>
            {
                x.Value = Math.Round(d: x.Value, decimals: POSITIONS_TO_ROUND, mode: MidpointRounding.AwayFromZero);
                x.Percentage = Math.Round(d: x.Percentage, decimals: POSITIONS_TO_ROUND, mode: MidpointRounding.AwayFromZero);
            });
    }
}
