using System;

namespace MoneyFox.Application
{
    /// <summary>
    ///     Utility methods
    /// </summary>
    public static class HelperFunctions
    {
        /// <summary>
        ///     Returns the last day of the month
        /// </summary>
        /// <returns>Last day of the month</returns>
        public static DateTime GetEndOfMonth()
        {
            DateTime today = DateTime.Today;

            return new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        }
    }
}
