namespace MoneyFox.Tests.TestFramework.Budget
{

    using FluentAssertions;
    using FluentAssertions.Execution;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;

    internal static class BudgetAssertion
    {
        public static void AssertBudget(Budget actual, TestData.DefaultBudget expected)
        {
            using (new AssertionScope())
            {
                actual.Name.Should().Be(expected.Name);
                actual.SpendingLimit.Should().Be(expected.SpendingLimit);
                actual.IncludedCategories.Should().BeEquivalentTo(expected.Categories);
            }
        }
    }

}
