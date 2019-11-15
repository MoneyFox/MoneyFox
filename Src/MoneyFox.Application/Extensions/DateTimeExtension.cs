using System;

namespace MoneyFox.Application.Extensions
{
    /// <summary>
    ///     Extension method for DateTime.
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        ///     Returns the first day of the current month.
        /// </summary>
        public static DateTime GetFirstDayOfMonth(this DateTime self)
        {
            return new DateTime(self.Year, self.Month, 1);
        }

        /// <summary>
        ///     Returns the last day of the current month.
        /// </summary>
        public static DateTime GetLastDayOfMonth(this DateTime self)
        {
            return new DateTime(self.Year, self.Month, DateTime.DaysInMonth(self.Year, self.Month));
        }
    }
}
