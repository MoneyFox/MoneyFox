namespace MoneyFox.Core.Common.Extensions;

using System;

public static class DateOnlyExtensions
{
    public static DateOnly GetLastDayOfMonth(this DateOnly self)
    {
        return new(
            year: self.Year,
            month: self.Month,
            day: DateTime.DaysInMonth(year: self.Year, month: self.Month));
    }
}
