namespace MoneyFox.Tests.TestFramework
{

    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;

    internal static class TestBudgetDbFactory
    {
        internal static Budget CreateDbBudget(this TestData.IBudget category)
        {
            return new Budget(name: category.Name, spendingLimit: category.SpendingLimit, includedCategories: category.Categories);
        }
    }

}
