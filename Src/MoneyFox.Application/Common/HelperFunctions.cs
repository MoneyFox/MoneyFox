using System;

namespace MoneyFox.Application.Common
{
    /// <summary>
    /// Utility methods
    /// </summary>
    public static class HelperFunctions
    {
        /// <summary>
        ///     Returns the first day of the current month.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetFirstDayMonth(ISystemDateHelper systemDateHelper)
            => new DateTime(systemDateHelper.Today.Year, systemDateHelper.Today.Month, 1);

        /// <summary>
        ///     Returns the last day of the month
        /// </summary>
        /// <returns>Last day of the month</returns>
        public static DateTime GetEndOfMonth(ISystemDateHelper systemDateHelper)
        {
            DateTime today = systemDateHelper.Today;
            return new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        }
    }
}
