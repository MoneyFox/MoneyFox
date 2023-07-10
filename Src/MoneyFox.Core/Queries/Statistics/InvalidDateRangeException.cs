namespace MoneyFox.Core.Queries.Statistics;

using System;

public class InvalidDateRangeException : Exception
{
    public InvalidDateRangeException() : base(message: "StartDate can't be after EndDate") { }
}
