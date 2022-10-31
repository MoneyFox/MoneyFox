namespace MoneyFox.Core._Pending_.Common;

using System;
using Core.Common.Helpers;

/// <summary>
///     Utility methods
/// </summary>
public static class HelperFunctions
{
    /// <summary>
    ///     Returns the first day of the current month.
    /// </summary>
    /// <returns></returns>
    public static DateTime GetFirstDayMonth(ISystemDateHelper systemDateHelper)
    {
        return new(year: systemDateHelper.Today.Year, month: systemDateHelper.Today.Month, day: 1);
    }

    /// <summary>
    ///     Returns the last day of the month
    /// </summary>
    /// <returns>Last day of the month</returns>
    public static DateTime GetEndOfMonth(ISystemDateHelper systemDateHelper)
    {
        var today = systemDateHelper.Today;

        return new(year: today.Year, month: today.Month, day: DateTime.DaysInMonth(year: today.Year, month: today.Month));
    }
}
