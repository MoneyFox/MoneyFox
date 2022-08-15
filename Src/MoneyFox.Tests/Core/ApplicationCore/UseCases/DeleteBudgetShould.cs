namespace MoneyFox.Tests.Core.ApplicationCore.UseCases
{

    using System.Threading;
    using System.Threading.Tasks;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using MoneyFox.Core.ApplicationCore.UseCases.BudgetDeletion;
    using NSubstitute;
    using TestFramework;
    using Xunit;

    public sealed class DeleteBudgetShould
    {
        private readonly IBudgetRepository budgetRepository;
        private readonly DeleteBudget.Handler handler;

        public DeleteBudgetShould()
        {
            budgetRepository = Substitute.For<IBudgetRepository>();
            handler = new DeleteBudget.Handler(budgetRepository);
        }

        [Fact]
        public async Task PassCorrectIdToDeleteToRepository()
        {
            // Arrange
            var testBudget = new TestData.DefaultBudget();

            // Act
            var command = new DeleteBudget.Command(budgetId: testBudget.Id);
            await handler.Handle(request: command, cancellationToken: CancellationToken.None);

            // Assert
            await budgetRepository.Received().DeleteAsync(testBudget.Id);
        }
    }

}
