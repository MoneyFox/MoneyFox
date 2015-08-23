using System;
using System.Collections.Generic;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Helper
{
    public class Utilities
    {
        /// <summary>
        ///     Returns the last day of the month
        /// </summary>
        /// <returns>Last day of the month</returns>
        public static DateTime GetEndOfMonth()
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        }

        /// <summary>
        ///     Will round all values of the passed statistic item list
        /// </summary>
        /// <param name="items">List of statistic items.</param>
        public static void RoundStatisticItems(List<StatisticItem> items)
        {
            foreach (var item in items)
            {
                item.Value = Math.Round(item.Value, 2, MidpointRounding.AwayFromZero);
            }
        }
    }
}