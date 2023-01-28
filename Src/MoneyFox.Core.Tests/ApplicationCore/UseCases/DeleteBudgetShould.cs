namespace MoneyFox.Core.Tests.ApplicationCore.UseCases;

using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Core.ApplicationCore.UseCases.BudgetDeletion;
using Core.Common.Interfaces;
using FluentAssertions;
using NSubstitute;
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
        var command = new DeleteBudget.Command(budgetId: 999);
        await handler.Handle(request: command, cancellationToken: CancellationToken.None);

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
        var command = new DeleteBudget.Command(budgetId: testBudget.Id);
        await handler.Handle(request: command, cancellationToken: CancellationToken.None);

        // Assert
        Context.Budgets.Should().BeEmpty();
    }
}
