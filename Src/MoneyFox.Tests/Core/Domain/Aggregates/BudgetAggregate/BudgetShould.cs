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
            var budget = new Budget(name: testBudget.Name, spendingLimit: testBudget.SpendingLimit, includedCategories: testBudget.Categories);

            // Assert
            AssertBudget(actual: budget, expected: testBudget);
        }

        [Fact]
        public void UpdateValuesCorrectly()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();
            var budget = new Budget("Empty", 10, ImmutableList.Create(1));

            // Act
            budget.Change(testBudget.Name, testBudget.SpendingLimit, testBudget.Categories);

            // Assert
            AssertBudget(actual: budget, expected: testBudget);
        }
    }

}
