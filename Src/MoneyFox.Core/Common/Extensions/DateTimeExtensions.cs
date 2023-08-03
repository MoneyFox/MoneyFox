namespace MoneyFox.Core.Common.Extensions;

using System;

public static class DateTimeExtensions
{

    public static DateTime GetFirstDayOfMonth(this DateTime self)
    {
        return new(year: self.Year, month: self.Month, day: 1);
    }

    public static DateTime GetLastDayOfMonth(this DateTime self)
    {
        return new(year: self.Year, month: self.Month, day: DateTime.DaysInMonth(year: self.Year, month: self.Month));
    }

    public static DateOnly ToDateOnly(this DateTime self)
    {
        return DateOnly.FromDateTime(self);
    }
}
