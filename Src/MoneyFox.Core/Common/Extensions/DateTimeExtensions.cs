namespace MoneyFox.Core.Common.Extensions;

using System;

/// <summary>
///     Extension method for DateTime.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    ///     Returns the first day of the current month.
    /// </summary>
    public static DateTime GetFirstDayOfMonth(this DateTime self)
    {
        return new(year: self.Year, month: self.Month, day: 1);
    }

    /// <summary>
    ///     Returns the last day of the current month.
    /// </summary>
    public static DateTime GetLastDayOfMonth(this DateTime self)
    {
        return new(year: self.Year, month: self.Month, day: DateTime.DaysInMonth(year: self.Year, month: self.Month));
    }
}
