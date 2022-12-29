namespace MoneyFox.Core.Tests.ApplicationCore.UseCases;

using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Core.ApplicationCore.UseCases.BudgetDeletion;
using NSubstitute;
using TestFramework;

public sealed class DeleteBudgetShould
{
    private readonly IBudgetRepository budgetRepository;
    private readonly DeleteBudget.Handler handler;

    public DeleteBudgetShould()
    {
        budgetRepository = Substitute.For<IBudgetRepository>();
        handler = new(budgetRepository);
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
