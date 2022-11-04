namespace MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading;

using Domain.Aggregates.BudgetAggregate;

public class BudgetListData
{
    public BudgetListData(int id, string name, decimal spendingLimit, decimal currentSpending,
        BudgetTimeRange timeRange)
    {
        Id = id;
        Name = name;
        SpendingLimit = spendingLimit;
        CurrentSpending = currentSpending;
        TimeRange = timeRange;
    }

    public int Id { get; }

    public string Name { get; }

    public decimal SpendingLimit { get; }
    public BudgetTimeRange TimeRange { get; }

    public decimal CurrentSpending { get; }
}
