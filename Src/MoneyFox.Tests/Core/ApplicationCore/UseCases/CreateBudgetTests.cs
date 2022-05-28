namespace MoneyFox.Tests.Core.ApplicationCore.UseCases
{

    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using MoneyFox.Core.ApplicationCore.UseCases.BudgetCreation;
    using NSubstitute;
    using TestFramework.Budget;
    using Xunit;

    public class CreateBudgetTests
    {
        private readonly IBudgetRepository budgetRepository;
        private readonly CreateBudget.Handler handler;

        public CreateBudgetTests()
        {
            budgetRepository = Substitute.For<IBudgetRepository>();
            handler = new CreateBudget.Handler(budgetRepository);
        }

        [Fact]
        public async Task AddsBudgetToRepository()
        {
            // Capture
            Budget? capturedBudget = null;
            await budgetRepository.AddAsync(Arg.Do<Budget>(b => capturedBudget = b));

            // Arrange
            var testData = new TestData.DefaultBudget();

            // Act
            var query = new CreateBudget.Query(testData.Name, testData.SpendingLimit);
            await handler.Handle(query, CancellationToken.None);

            // Assert
            await budgetRepository.AddAsync(Arg.Any<Budget>());

            capturedBudget.Should().NotBeNull();
            capturedBudget!.Name.Should().Be(testData.Name);
            capturedBudget.SpendingLimit.Should().Be(testData.SpendingLimit);
        }
    }

}
