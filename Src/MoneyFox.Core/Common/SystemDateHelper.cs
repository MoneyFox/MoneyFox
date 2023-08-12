namespace MoneyFox.Core.Common;

using System;

public interface ISystemDateHelper
{
    DateOnly TodayDateOnly { get; }
    DateTime Today { get; }
    DateTime Now { get; }
}

public class SystemDateHelper : ISystemDateHelper
{
    public DateOnly TodayDateOnly => DateOnly.FromDateTime(Today);
    public DateTime Today => DateTime.Today;
    public DateTime Now => DateTime.Now;
}
