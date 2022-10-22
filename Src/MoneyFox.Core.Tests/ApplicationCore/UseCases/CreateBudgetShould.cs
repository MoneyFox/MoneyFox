namespace MoneyFox.Core.Tests.ApplicationCore.UseCases
{

    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using MoneyFox.Core.ApplicationCore.UseCases.BudgetCreation;
    using NSubstitute;
    using TestFramework;
    using Xunit;
    using static TestFramework.BudgetAssertion;

    public sealed class CreateBudgetShould
    {
        private readonly IBudgetRepository budgetRepository;
        private readonly CreateBudget.Handler handler;

        public CreateBudgetShould()
        {
            budgetRepository = Substitute.For<IBudgetRepository>();
            handler = new CreateBudget.Handler(budgetRepository);
        }

        [Fact]
        public async Task AddBudgetToRepository()
        {
            // Capture
            Budget? capturedBudget = null;
            await budgetRepository.AddAsync(Arg.Do<Budget>(b => capturedBudget = b));

            // Arrange
            var testData = new TestData.DefaultBudget();

            // Act
            var query = new CreateBudget.Command(
                name: testData.Name,
                spendingLimit: testData.SpendingLimit,
                budgetTimeRange: BudgetTimeRange.YearToDate,
                categories: testData.Categories);

            await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            capturedBudget.Should().NotBeNull();
            AssertBudget(actual: capturedBudget!, expected: testData);
        }
    }

}
