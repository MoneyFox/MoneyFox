namespace MoneyFox.Core.Tests.Features.BudgetDeletion;

using Core.Features.BudgetDeletion;
using Domain.Tests.TestFramework;

public sealed class DeleteBudgetTests : InMemoryTestBase
{
    private readonly DeleteBudget.Handler handler;

    public DeleteBudgetTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task DoesNotThrowExceptionWhenBudgetWithIdNotFound()
    {
        // Act
        var command = new DeleteBudget.Command(budgetId: new(999));
        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        Context.Budgets.Should().BeEmpty();
    }

    [Fact]
    public async Task PassCorrectIdToDeleteToRepository()
    {
        // Arrange
        var testBudget = new TestData.DefaultBudget();
        Context.RegisterBudget(testBudget);

        // Act
        var command = new DeleteBudget.Command(budgetId: new(testBudget.Id));
        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        Context.Budgets.Should().BeEmpty();
    }
}
