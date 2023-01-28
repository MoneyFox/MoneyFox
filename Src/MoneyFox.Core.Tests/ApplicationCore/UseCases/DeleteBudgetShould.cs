namespace MoneyFox.Core.Tests.ApplicationCore.UseCases;

using Core.ApplicationCore.UseCases.BudgetDeletion;
using FluentAssertions;
using TestFramework;

public sealed class DeleteBudgetShould : InMemoryTestBase
{
    private readonly DeleteBudget.Handler handler;

    public DeleteBudgetShould()
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
