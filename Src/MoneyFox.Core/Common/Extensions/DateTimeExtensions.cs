namespace MoneyFox.Core.Common.Extensions;

using System;

public static class DateTimeExtensions
{

    public static DateTime GetFirstDayOfMonth(this DateTime self)
    {
        return new(
            year: self.Year,
            month: self.Month,
            day: 1,
            hour: 0,
            minute: 0,
            second: 0,
            kind: DateTimeKind.Local);
    }

    public static DateTime GetLastDayOfMonth(this DateTime self)
    {
        return new(
            year: self.Year,
            month: self.Month,
            day: DateTime.DaysInMonth(year: self.Year, month: self.Month),
            hour: 0,
            minute: 0,
            second: 0,
            kind: DateTimeKind.Local);
    }

    public static DateOnly ToDateOnly(this DateTime self)
    {
        return DateOnly.FromDateTime(self);
    }
}
