using System;

namespace MoneyFox.BusinessLogic.Extensions
{
    /// <summary>
    ///     Extension method for DateTime.
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        ///     Returns teh first day of the current month.
        /// </summary>
        public static DateTime GetFirstDayOfMonth(this DateTime self)
            => new DateTime(self.Year, self.Month, 1);

        /// <summary>
        ///     Returns the last day of the current month.
        /// </summary>
        public static DateTime GetLastDayOfMonth(this DateTime self)
            => new DateTime(self.Year, self.Month, DateTime.DaysInMonth(self.Year, self.Month));
    }
}