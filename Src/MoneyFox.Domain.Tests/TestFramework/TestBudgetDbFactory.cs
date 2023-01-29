namespace MoneyFox.Domain.Tests.TestFramework;

internal static class TestBudgetDbFactory
{
    internal static Budget CreateDbBudget(this TestData.IBudget budget)
    {
        var spendingLimit = new SpendingLimit(budget.SpendingLimit);

        return new(name: budget.Name, spendingLimit: spendingLimit, timeRange: budget.BudgetTimeRange, includedCategories: budget.Categories);
    }
}
