namespace MoneyFox.Core.Tests.ApplicationCore.UseCases;

using System.Collections.Immutable;
using Core.ApplicationCore.UseCases.BudgetUpdate;
using Microsoft.EntityFrameworkCore;
using static BudgetAssertion;

public sealed class UpdateBudgetShould : InMemoryTestBase
{
    private readonly UpdateBudget.Handler handler;

    public UpdateBudgetShould()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task PassUpdatedAggregateToRepository()
    {
        // Capture
        var testBudget = new TestData.DefaultBudget();
        Context.RegisterBudget(testBudget);

        // Arrange
        var updatedBudget = new TestData.DefaultBudget
        {
            Name = "Drinks",
            SpendingLimit = testBudget.SpendingLimit + 11,
            CurrentSpending = testBudget.CurrentSpending + 22,
            Categories = ImmutableList.Create(12, 26)
        };

        // Act
        var command = new UpdateBudget.Command(
            budgetId: new(testBudget.Id),
            name: updatedBudget.Name,
            spendingLimit: updatedBudget.SpendingLimit,
            budgetTimeRange: updatedBudget.BudgetTimeRange,
            categories: updatedBudget.Categories);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var loadedBudget = await Context.Budgets.SingleAsync();
        AssertBudget(actual: loadedBudget, expected: updatedBudget);
    }
}
