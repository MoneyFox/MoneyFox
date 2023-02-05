namespace MoneyFox.Domain.Tests.Aggregates.BudgetAggregate;

using System.Collections.Immutable;
using Domain.Aggregates.BudgetAggregate;
using TestFramework;
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
        var budget = new Budget(name: testBudget.Name, spendingLimit: spendingLimit, interval: testBudget.Interval, includedCategories: testBudget.Categories);

        // Assert
        AssertBudget(actual: budget, expected: testBudget);
    }

    [Fact]
    public void UpdateValuesCorrectly()
    {
        // Arrange
        var testBudget = new TestData.DefaultBudget();
        var budget = new Budget(name: "Empty", spendingLimit: new(10), interval: new(2), includedCategories: ImmutableList.Create(1));

        // Act
        var spendingLimit = new SpendingLimit(testBudget.SpendingLimit);
        budget.Change(
            budgetName: testBudget.Name,
            spendingLimit: spendingLimit,
            includedCategories: testBudget.Categories,
            budgetInterval: testBudget.Interval);

        // Assert
        AssertBudget(actual: budget, expected: testBudget);
    }
}
