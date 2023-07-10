namespace MoneyFox.Domain.Aggregates.BudgetAggregate;

using JetBrains.Annotations;

public record struct BudgetId(int Value);

public class Budget : EntityBase
{
    [UsedImplicitly]
    private Budget()
    {
        SpendingLimit = default!;
        Interval = default!;
    }

    public Budget(string name, SpendingLimit spendingLimit, BudgetInterval interval, IReadOnlyList<int> includedCategories)
    {
        Name = name;
        SpendingLimit = spendingLimit;
        Interval = interval;
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

    public BudgetInterval Interval
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
        Interval = budgetInterval;
        IncludedCategories = includedCategories;
    }

    public void SetInterval(int numberOfMonths)
    {
        Interval = new(numberOfMonths);
    }
}
