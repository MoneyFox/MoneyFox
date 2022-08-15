namespace MoneyFox.Tests.Core.Domain.Aggregates.BudgetAggregate
{

    using System.Collections.Immutable;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using TestFramework;
    using Xunit;
    using static TestFramework.BudgetAssertion;

    public sealed class BudgetShould
    {
        [Fact]
        public void BeCorrectlyInitialized()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();

            // Act
            var spendingLimit = new SpendingLimit(testBudget.SpendingLimit);
            var budget = new Budget(
                name: testBudget.Name,
                spendingLimit: spendingLimit,
                timeRange: BudgetTimeRange.YearToDate,
                includedCategories: testBudget.Categories);

            // Assert
            AssertBudget(actual: budget, expected: testBudget);
        }

        [Fact]
        public void UpdateValuesCorrectly()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();
            var budget = new Budget(
                name: "Empty",
                spendingLimit: new SpendingLimit(10),
                timeRange: BudgetTimeRange.YearToDate,
                includedCategories: ImmutableList.Create(1));

            // Act
            var spendingLimit = new SpendingLimit(testBudget.SpendingLimit);
            budget.Change(
                budgetName: testBudget.Name,
                spendingLimit: spendingLimit,
                includedCategories: testBudget.Categories,
                timeRange: BudgetTimeRange.YearToDate);

            // Assert
            AssertBudget(actual: budget, expected: testBudget);
        }
    }

}
