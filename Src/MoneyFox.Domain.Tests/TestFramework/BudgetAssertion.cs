namespace MoneyFox.Domain.Tests.TestFramework;

using FluentAssertions;
using FluentAssertions.Execution;
using MoneyFox.Domain.Aggregates.BudgetAggregate;

internal static class BudgetAssertion
{
    public static void AssertBudget(Budget actual, TestData.DefaultBudget expected)
    {
        using (new AssertionScope())
        {
            actual.Name.Should().Be(expected.Name);
            actual.SpendingLimit.Value.Should().Be(expected.SpendingLimit);
            actual.BudgetTimeRange.Should().Be(expected.BudgetTimeRange);
            actual.IncludedCategories.Should().BeEquivalentTo(expected.Categories);
        }
    }
}
