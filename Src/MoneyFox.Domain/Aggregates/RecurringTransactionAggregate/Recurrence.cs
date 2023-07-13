namespace MoneyFox.Domain.Aggregates;

public enum Recurrence
{
    Daily = 0,
    DailyWithoutWeekend = 1,
    Weekly = 2,
    Monthly = 3,
    Yearly = 4,
    Biweekly = 5,
    Bimonthly = 6,
    Quarterly = 7,
    Biannually = 8
}
