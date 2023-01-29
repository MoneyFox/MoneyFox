namespace MoneyFox.Domain.Tests.Aggregates.BudgetAggregate;

using System.Collections.Immutable;
using MoneyFox.Domain.Aggregates.BudgetAggregate;
using MoneyFox.Domain.Tests.TestFramework;
using static TestFramework.BudgetAssertion;

public sealed class BudgetTests
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
            timeRange: testBudget.BudgetTimeRange,
            includedCategories: testBudget.Categories);

        // Assert
        AssertBudget(actual: budget, expected: testBudget);
    }

    [Fact]
    public void UpdateValuesCorrectly()
    {
        // Arrange
        var testBudget = new TestData.DefaultBudget();
        var budget = new Budget(name: "Empty", spendingLimit: new(10), timeRange: BudgetTimeRange.YearToDate, includedCategories: ImmutableList.Create(1));

        // Act
        var spendingLimit = new SpendingLimit(testBudget.SpendingLimit);
        budget.Change(
            budgetName: testBudget.Name,
            spendingLimit: spendingLimit,
            includedCategories: testBudget.Categories,
            timeRange: testBudget.BudgetTimeRange);

        // Assert
        AssertBudget(actual: budget, expected: testBudget);
    }
}
