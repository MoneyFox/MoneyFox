namespace MoneyFox.Domain.Aggregates.BudgetAggregate;

using JetBrains.Annotations;

public record struct BudgetId(int Value);

public record struct BudgetInterval(int NumberOfMonths);

public class Budget : EntityBase
{
    [UsedImplicitly]
    private Budget()
    {
        SpendingLimit = default!;
    }

    public Budget(string name, SpendingLimit spendingLimit, BudgetInterval budgetInterval, IReadOnlyList<int> includedCategories)
    {
        Name = name;
        SpendingLimit = spendingLimit;
        BudgetInterval = budgetInterval;
        IncludedCategories = includedCategories;
    }

    public BudgetId Id
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public string Name
    {
        get;

        [UsedImplicitly]
        private set;
    } = string.Empty;

    public SpendingLimit SpendingLimit
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public BudgetInterval BudgetInterval
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public BudgetTimeRange BudgetTimeRange
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public IReadOnlyList<int> IncludedCategories
    {
        get;

        [UsedImplicitly]
        private set;
    } = new List<int>();

    public void Change(string budgetName, SpendingLimit spendingLimit, IReadOnlyList<int> includedCategories, BudgetInterval budgetInterval)
    {
        Name = budgetName;
        SpendingLimit = spendingLimit;
        BudgetInterval = budgetInterval;
        IncludedCategories = includedCategories;
    }
}
