using System;

namespace MoneyFox.Shared.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime GetFirstDayOfMonth(this DateTime self) 
            => new DateTime(self.Year, self.Month, 1);

        public static DateTime GetLastDayOfMonth(this DateTime self) 
            => new DateTime(self.Year, self.Month, DateTime.DaysInMonth(self.Year, self.Month));
    }
}