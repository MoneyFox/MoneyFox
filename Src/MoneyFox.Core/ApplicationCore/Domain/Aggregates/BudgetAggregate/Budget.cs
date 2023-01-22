namespace MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;

using System.Collections.Generic;
using Common.Interfaces;
using JetBrains.Annotations;

public class Budget : EntityBase, IAggregateRoot
{
    [UsedImplicitly]
    private Budget()
    {
        SpendingLimit = default!;
    }

    public Budget(string name, SpendingLimit spendingLimit, BudgetTimeRange timeRange, IReadOnlyList<int> includedCategories)
    {
        Name = name;
        SpendingLimit = spendingLimit;
        BudgetTimeRange = timeRange;
        IncludedCategories = includedCategories;
    }

    public int Id
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

    public void Change(string budgetName, SpendingLimit spendingLimit, IReadOnlyList<int> includedCategories, BudgetTimeRange timeRange)
    {
        Name = budgetName;
        SpendingLimit = spendingLimit;
        BudgetTimeRange = timeRange;
        IncludedCategories = includedCategories;
    }
}
