namespace MoneyFox.Tests.TestFramework
{

    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;

    internal static class TestBudgetDbFactory
    {
        internal static Budget CreateDbBudget(this TestData.IBudget budget)
        {
            var spendingLimit = new SpendingLimit(budget.SpendingLimit);
            return new Budget(name: budget.Name, spendingLimit: spendingLimit, includedCategories: budget.Categories);
        }
    }

}
