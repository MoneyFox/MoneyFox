namespace MoneyFox.Tests.Core.Domain.Aggregates.BudgetAggregate
{

    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using TestFramework;
    using Xunit;
    using static TestFramework.BudgetAssertion;

    public sealed class BudgetShould
    {
        [Fact]
        public void BeCorrectlyInitialized()
        {
            var testBudget = new TestData.DefaultBudget();

            var budget = new Budget(testBudget.Name, testBudget.SpendingLimit, testBudget.Categories);

            AssertBudget(budget, testBudget);
        }
    }

}
