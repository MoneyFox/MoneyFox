namespace MoneyFox.Core.Tests.Features.BudgetUpdate;

using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core.Features.BudgetUpdate;
using MoneyFox.Domain.Tests.TestFramework;
using static Domain.Tests.TestFramework.BudgetAssertion;

public sealed class UpdateBudgetTests : InMemoryTestBase
{
    private readonly UpdateBudget.Handler handler;

    public UpdateBudgetTests()
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
            SpendingLimit = new(testBudget.SpendingLimit + 11),
            CurrentSpending = testBudget.CurrentSpending + 22,
            Categories = ImmutableList.Create(12, 26)
        };

        // Act
        var command = new UpdateBudget.Command(
            budgetId: new(testBudget.Id),
            name: updatedBudget.Name,
            spendingLimit: updatedBudget.SpendingLimit,
            numberOfMonths: updatedBudget.Interval.NumberOfMonths,
            categories: updatedBudget.Categories);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var loadedBudget = await Context.Budgets.SingleAsync();
        AssertBudget(actual: loadedBudget, expected: updatedBudget);
    }
}
