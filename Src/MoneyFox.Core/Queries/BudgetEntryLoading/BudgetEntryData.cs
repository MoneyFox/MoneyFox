namespace MoneyFox.Core.Queries.BudgetEntryLoading;

using System.Collections.Generic;
using Domain.Aggregates.BudgetAggregate;

public sealed class BudgetEntryData
{
    public BudgetEntryData(
        BudgetId id,
        string name,
        decimal spendingLimit,
        int numberOfMonths,
        IReadOnlyList<BudgetCategory> categories)
    {
        Id = id;
        Name = name;
        SpendingLimit = spendingLimit;
        Categories = categories;
        NumberOfMonths = numberOfMonths;
    }

    public BudgetId Id { get; }

    public string Name { get; }

    public decimal SpendingLimit { get; }
    public int NumberOfMonths { get; }

    public IReadOnlyList<BudgetCategory> Categories { get; }

    public class BudgetCategory
    {
        public BudgetCategory(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }
    }
}
