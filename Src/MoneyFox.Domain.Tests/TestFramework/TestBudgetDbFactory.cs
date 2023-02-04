namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.BudgetAggregate;

internal static class TestBudgetDbFactory
{
    internal static Budget CreateDbBudget(this TestData.IBudget budget)
    {
        return new(name: budget.Name, spendingLimit: budget.SpendingLimit, interval: budget.Interval, includedCategories: budget.Categories);
    }
}
