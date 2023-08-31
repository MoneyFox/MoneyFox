namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.BudgetAggregate;
using FluentAssertions.Execution;

internal static class BudgetAssertion
{
    public static void AssertBudget(Budget actual, TestData.DefaultBudget expected)
    {
        using (new AssertionScope())
        {
            actual.Name.Should().Be(expected.Name);
            actual.SpendingLimit.Value.Should().Be(expected.SpendingLimit);
            actual.Interval.Should().Be(expected.Interval);
            actual.IncludedCategories.Should().BeEquivalentTo(expected.Categories);
        }
    }
}
